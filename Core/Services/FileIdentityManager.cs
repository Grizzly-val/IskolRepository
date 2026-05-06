using System.Text.Json;
using IskolRepository.Core.Interfaces.Infrastructure;
using IskolRepository.Models;

namespace IskolRepository.Core.Services;

/// <summary>
/// Manages the file identity manifest, which maps file GUIDs to their current paths and metadata.
/// This service provides CRUD operations on the manifest and persists it to .metadata/files.json.
/// </summary>
public interface IFileIdentityManager
{
    /// <summary>
    /// Loads the identity manifest from .metadata/files.json.
    /// If the manifest doesn't exist, creates an empty one.
    /// </summary>
    FileIdentityManifest LoadManifest(string repositoryPath);

    /// <summary>
    /// Registers a new file in the identity manifest, assigning it a unique GUID.
    /// Updates the manifest file on disk.
    /// </summary>
    /// <returns>The assigned GUID for the file.</returns>
    Guid RegisterFile(string repositoryPath, string filePath);

    /// <summary>
    /// Updates a file's path in the manifest (e.g., after a rename or move).
    /// </summary>
    void UpdateFilePath(string repositoryPath, Guid fileId, string newPath);

    /// <summary>
    /// Retrieves the GUID for a file by its current path.
    /// Returns null if the file is not in the manifest.
    /// </summary>
    Guid? GetFileIdByPath(string repositoryPath, string filePath);

    /// <summary>
    /// Retrieves the current path for a file by its GUID.
    /// Returns null if the GUID is not found in the manifest.
    /// </summary>
    string? GetFilePathById(string repositoryPath, Guid fileId);

    /// <summary>
    /// Finds all files in the manifest that don't exist on disk (orphaned entries).
    /// </summary>
    List<FileIdentity> FindOrphaned(string repositoryPath);

    /// <summary>
    /// Finds all files on disk that aren't in the manifest (lost files).
    /// </summary>
    List<string> FindLost(string repositoryPath);

    /// <summary>
    /// Persists the manifest to .metadata/files.json.
    /// </summary>
    void SaveManifest(string repositoryPath, FileIdentityManifest manifest);
}

public class FileIdentityManager : IFileIdentityManager
{
    private const string ManifestFileName = "files.json";
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly IFileSystemHelper _fileSystemHelper;
    private readonly IPathProvider _pathProvider;

    public FileIdentityManager(IFileSystemHelper fileSystemHelper, IPathProvider pathProvider)
    {
        _fileSystemHelper = fileSystemHelper ?? throw new ArgumentNullException(nameof(fileSystemHelper));
        _pathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = false
        };
    }

    public FileIdentityManifest LoadManifest(string repositoryPath)
    {
        var manifestPath = GetManifestPath(repositoryPath);

        if (!_fileSystemHelper.FileExists(manifestPath))
        {
            return new FileIdentityManifest();
        }

        try
        {
            var json = _fileSystemHelper.ReadAllText(manifestPath);
            var manifest = JsonSerializer.Deserialize<FileIdentityManifest>(json, _jsonOptions);
            return manifest ?? new FileIdentityManifest();
        }
        catch (Exception)
        {
            // If manifest is corrupted, create a new one
            // In production, consider logging this event
            return new FileIdentityManifest();
        }
    }

    public Guid RegisterFile(string repositoryPath, string filePath)
    {
        var manifest = LoadManifest(repositoryPath);

        // Generate a new GUID for this file
        var fileId = Guid.NewGuid();
        var fileName = _pathProvider.GetFileName(filePath);

        var fileIdentity = new FileIdentity
        {
            FileId = fileId,
            CurrentPath = filePath,
            OriginalFileName = fileName,
            CreatedDate = DateTime.Now,
            Status = "active"
        };

        // Convert Guid to string key for JSON serialization
        manifest.Files[fileId.ToString()] = fileIdentity;
        manifest.LastUpdated = DateTime.Now;

        SaveManifest(repositoryPath, manifest);

        return fileId;
    }

    public void UpdateFilePath(string repositoryPath, Guid fileId, string newPath)
    {
        var manifest = LoadManifest(repositoryPath);
        var key = fileId.ToString();

        if (manifest.Files.TryGetValue(key, out var fileIdentity))
        {
            fileIdentity.CurrentPath = newPath;
            manifest.LastUpdated = DateTime.Now;
            SaveManifest(repositoryPath, manifest);
        }
    }

    public Guid? GetFileIdByPath(string repositoryPath, string filePath)
    {
        var manifest = LoadManifest(repositoryPath);

        // Normalize paths for comparison (handle case sensitivity on different OS)
        var normalizedPath = NormalizePath(filePath);

        foreach (var kvp in manifest.Files)
        {
            if (NormalizePath(kvp.Value.CurrentPath) == normalizedPath)
            {
                return Guid.Parse(kvp.Key);
            }
        }

        return null;
    }

    public string? GetFilePathById(string repositoryPath, Guid fileId)
    {
        var manifest = LoadManifest(repositoryPath);
        var key = fileId.ToString();

        if (manifest.Files.TryGetValue(key, out var fileIdentity))
        {
            return fileIdentity.CurrentPath;
        }

        return null;
    }

    public List<FileIdentity> FindOrphaned(string repositoryPath)
    {
        var manifest = LoadManifest(repositoryPath);
        var orphaned = new List<FileIdentity>();

        foreach (var kvp in manifest.Files)
        {
            var fileIdentity = kvp.Value;
            if (!_fileSystemHelper.FileExists(fileIdentity.CurrentPath))
            {
                orphaned.Add(fileIdentity);
            }
        }

        return orphaned;
    }

    public List<string> FindLost(string repositoryPath)
    {
        var manifest = LoadManifest(repositoryPath);
        var lost = new List<string>();

        // Recursively scan all files in the repository
        var allFiles = EnumerateRepositoryFiles(repositoryPath);

        foreach (var filePath in allFiles)
        {
            // Check if this file is in the manifest
            var fileId = GetFileIdByPath(repositoryPath, filePath);
            if (fileId == null)
            {
                lost.Add(filePath);
            }
        }

        return lost;
    }

    public void SaveManifest(string repositoryPath, FileIdentityManifest manifest)
    {
        var manifestPath = GetManifestPath(repositoryPath);
        var metadataFolder = Path.GetDirectoryName(manifestPath);

        // Ensure metadata folder exists
        _fileSystemHelper.CreateDirectory(metadataFolder!);

        // Serialize and write manifest
        var json = JsonSerializer.Serialize(manifest, _jsonOptions);
        _fileSystemHelper.WriteAllText(manifestPath, json);
    }

    private string GetManifestPath(string repositoryPath)
    {
        var metadataFolder = Path.Combine(repositoryPath, RepositoryService.MetadataFolderName);
        return Path.Combine(metadataFolder, ManifestFileName);
    }

    private List<string> EnumerateRepositoryFiles(string repositoryPath)
    {
        var files = new List<string>();
        EnumerateFilesRecursive(repositoryPath, files);
        return files;
    }

    private void EnumerateFilesRecursive(string directory, List<string> files)
    {
        try
        {
            // Skip metadata and history folders
            foreach (var file in _fileSystemHelper.EnumerateFiles(directory))
            {
                files.Add(file);
            }

            foreach (var subdir in _fileSystemHelper.EnumerateDirectories(directory))
            {
                var dirName = _pathProvider.GetFileName(subdir);
                if (string.Equals(dirName, RepositoryService.MetadataFolderName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                EnumerateFilesRecursive(subdir, files);
            }
        }
        catch (Exception)
        {
            // Silently skip directories we can't access
        }
    }

    private string NormalizePath(string path)
    {
        // Normalize to forward slashes and lowercase for case-insensitive comparison
        return path.Replace("\\", "/").ToLowerInvariant();
    }
}
