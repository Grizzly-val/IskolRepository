using System.Security.Cryptography;
using System.Text.Json;
using IskolRepository.Core.Interfaces.Infrastructure;
using IskolRepository.Models;

namespace IskolRepository.Core.Services;

/// <summary>
/// Handles file reconciliation and migration from filename-based to GUID-based version history.
/// Provides methods to detect orphaned/lost files, migrate old history folders, and reconcile
/// based on file content hashing.
/// </summary>
public interface IFileReconciliationService
{
    /// <summary>
    /// Validates the integrity of the identity manifest and returns any inconsistencies found.
    /// </summary>
    FileReconciliationReport ValidateManifestIntegrity(string repositoryPath);

    /// <summary>
    /// Automatically migrates filename-based history folders (.metadata/.history/{fileName})
    /// to GUID-based folders (.metadata/.history/{fileId}).
    /// </summary>
    void MigrateHistoryFolders(string repositoryPath);

    /// <summary>
    /// Attempts to reconcile lost files (files on disk not in manifest) by matching content hashes
    /// with version snapshots in existing history folders.
    /// </summary>
    void ReconcileLostFiles(string repositoryPath);

    /// <summary>
    /// Registers all files found on disk that are not yet in the identity manifest.
    /// This is useful for migrating from older versions that didn't have the GUID-based system.
    /// </summary>
    void RegisterAllUnregisteredFiles(string repositoryPath);

    /// <summary>
    /// Computes the SHA256 hash of a file's content for content-based matching.
    /// </summary>
    string ComputeFileHash(string filePath);
}

/// <summary>
/// Report from manifest validation.
/// </summary>
public class FileReconciliationReport
{
    public List<FileIdentity> OrphanedFiles { get; set; } = [];
    public List<string> LostFiles { get; set; } = [];
    public List<string> ReconciliationIssues { get; set; } = [];
    public bool HasIssues => OrphanedFiles.Count > 0 || LostFiles.Count > 0 || ReconciliationIssues.Count > 0;
    public int TotalIssues => OrphanedFiles.Count + LostFiles.Count + ReconciliationIssues.Count;
}

public class FileReconciliationService : IFileReconciliationService
{
    private readonly IFileSystemHelper _fileSystemHelper;
    private readonly IPathProvider _pathProvider;
    private readonly IFileIdentityManager _identityManager;
    private readonly JsonSerializerOptions _jsonOptions;

    public FileReconciliationService(
        IFileSystemHelper fileSystemHelper,
        IPathProvider pathProvider,
        IFileIdentityManager identityManager)
    {
        _fileSystemHelper = fileSystemHelper ?? throw new ArgumentNullException(nameof(fileSystemHelper));
        _pathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
        _identityManager = identityManager ?? throw new ArgumentNullException(nameof(identityManager));

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = false
        };
    }

    public FileReconciliationReport ValidateManifestIntegrity(string repositoryPath)
    {
        var report = new FileReconciliationReport();

        try
        {
            // Find orphaned files (in manifest but not on disk)
            var orphaned = _identityManager.FindOrphaned(repositoryPath);
            report.OrphanedFiles = orphaned;

            if (orphaned.Count > 0)
            {
                report.ReconciliationIssues.Add($"Found {orphaned.Count} orphaned file(s) in manifest not on disk.");
            }

            // Find lost files (on disk but not in manifest)
            var lost = _identityManager.FindLost(repositoryPath);
            report.LostFiles = lost;

            if (lost.Count > 0)
            {
                report.ReconciliationIssues.Add($"Found {lost.Count} lost file(s) on disk not in manifest.");
            }
        }
        catch (Exception ex)
        {
            report.ReconciliationIssues.Add($"Error during validation: {ex.Message}");
        }

        return report;
    }

    public void MigrateHistoryFolders(string repositoryPath)
    {
        try
        {
            var manifest = _identityManager.LoadManifest(repositoryPath);
            var historyRootPath = Path.Combine(repositoryPath, VersionHelper.MetadataFolderName, VersionHelper.HistoryFolderName);

            if (!_fileSystemHelper.DirectoryExists(historyRootPath))
            {
                return; // No history folders to migrate
            }

            // Iterate through all files in the manifest
            foreach (var kvp in manifest.Files)
            {
                var fileIdentity = kvp.Value;
                var fileId = Guid.Parse(kvp.Key);
                var originalFileName = fileIdentity.OriginalFileName;

                // Check if old filename-based folder exists
                var oldHistoryFolder = Path.Combine(historyRootPath, Path.GetFileNameWithoutExtension(originalFileName));
                var newHistoryFolder = Path.Combine(historyRootPath, fileId.ToString());

                // If both old and new folders exist, the migration is already done
                if (_fileSystemHelper.DirectoryExists(newHistoryFolder) && _fileSystemHelper.DirectoryExists(oldHistoryFolder))
                {
                    // Prefer new folder, delete old one
                    try
                    {
                        _fileSystemHelper.DeleteDirectory(oldHistoryFolder);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to delete old history folder: {ex.Message}");
                    }
                    continue;
                }

                // If only old folder exists, rename it to new GUID-based name
                if (_fileSystemHelper.DirectoryExists(oldHistoryFolder) && !_fileSystemHelper.DirectoryExists(newHistoryFolder))
                {
                    try
                    {
                        _fileSystemHelper.MoveDirectory(oldHistoryFolder, newHistoryFolder);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to migrate history folder from {oldHistoryFolder} to {newHistoryFolder}: {ex.Message}");
                    }
                }
            }

            _identityManager.SaveManifest(repositoryPath, manifest);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error during history folder migration: {ex.Message}");
        }
    }

    public void ReconcileLostFiles(string repositoryPath)
    {
        try
        {
            var manifest = _identityManager.LoadManifest(repositoryPath);
            var lostFiles = _identityManager.FindLost(repositoryPath);

            if (lostFiles.Count == 0)
            {
                return; // Nothing to reconcile
            }

            var historyRootPath = Path.Combine(repositoryPath, VersionHelper.MetadataFolderName, VersionHelper.HistoryFolderName);

            foreach (var lostFilePath in lostFiles)
            {
                if (!_fileSystemHelper.FileExists(lostFilePath))
                {
                    continue; // File was deleted
                }

                var lostFileHash = ComputeFileHash(lostFilePath);

                // Try to match lost file against snapshots in history folders
                foreach (var kvp in manifest.Files)
                {
                    var fileId = Guid.Parse(kvp.Key);
                    var historyFolder = Path.Combine(historyRootPath, fileId.ToString());

                    if (!_fileSystemHelper.DirectoryExists(historyFolder))
                    {
                        continue;
                    }

                    // Compare against all snapshots in this history folder
                    var snapshotFiles = _fileSystemHelper.EnumerateFiles(historyFolder)
                        .Where(f => f.StartsWith("v", StringComparison.OrdinalIgnoreCase) && !f.EndsWith("log.json", StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    foreach (var snapshotPath in snapshotFiles)
                    {
                        var snapshotHash = ComputeFileHash(snapshotPath);
                        if (snapshotHash == lostFileHash)
                        {
                            // Found a match! Re-link this file to the manifest
                            _identityManager.UpdateFilePath(repositoryPath, fileId, lostFilePath);
                            System.Diagnostics.Debug.WriteLine($"Reconciled lost file: {lostFilePath} → FileId: {fileId}");
                            goto next_lost_file; // Move to next lost file
                        }
                    }
                }

                next_lost_file:;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error during lost file reconciliation: {ex.Message}");
        }
    }

    public void RegisterAllUnregisteredFiles(string repositoryPath)
    {
        try
        {
            var unregisteredFiles = _identityManager.FindLost(repositoryPath);

            if (unregisteredFiles.Count == 0)
            {
                return; // All files already registered
            }

            foreach (var filePath in unregisteredFiles)
            {
                try
                {
                    // Register this file with a new GUID
                    _identityManager.RegisterFile(repositoryPath, filePath);
                    System.Diagnostics.Debug.WriteLine($"Registered unregistered file: {filePath}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to register file {filePath}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error during unregistered file registration: {ex.Message}");
        }
    }

    public string ComputeFileHash(string filePath)
    {
        try
        {
            using (var sha256 = SHA256.Create())
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var hash = sha256.ComputeHash(fileStream);
                return Convert.ToHexString(hash);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error computing hash for {filePath}: {ex.Message}");
            return string.Empty;
        }
    }
}
