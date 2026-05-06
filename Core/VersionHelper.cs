using System.Text.Json;
using IskolRepository.Models;

namespace IskolRepository.Core;

public static class VersionHelper
{
    public const string MetadataFolderName = ".metadata";
    public const string HistoryFolderName = ".history";

    /// <summary>
    /// Saves a new version of a file to the GUID-based history folder.
    /// The file's GUID must already be registered in the identity manifest.
    /// </summary>
    /// <param name="repositoryPath">Path to the repository containing the file</param>
    /// <param name="fileId">GUID identifier of the file</param>
    /// <param name="filePath">Current path to the file being versioned</param>
    /// <param name="comment">User comment for this version</param>
    /// <param name="jsonOptions">JSON serialization options</param>
    public static void SaveVersion(string repositoryPath, Guid fileId, string filePath, string comment, JsonSerializerOptions jsonOptions)
    {
        var historyFolder = GetHistoryFolderPath(repositoryPath, fileId);
        Directory.CreateDirectory(historyFolder);

        EnsureMetadataFolderHidden(repositoryPath);

        var versions = ReadVersionLogByHistoryFolder(historyFolder, jsonOptions);
        var nextVersion = versions.Count == 0 ? 1 : versions.Max(v => v.Version) + 1;
        var snapshotPath = Path.Combine(historyFolder, $"v{nextVersion}{Path.GetExtension(filePath)}");

        File.Copy(filePath, snapshotPath, true);
        versions.Add(new FileVersion
        {
            FileId = fileId,
            Version = nextVersion,
            Timestamp = DateTime.Now,
            Comment = comment,
            SnapshotPath = snapshotPath
        });

        SaveVersionLogByHistoryFolder(historyFolder, versions, jsonOptions);
    }

    /// <summary>
    /// Reverts a file to a previous version. Deletes all versions after the target version.
    /// </summary>
    /// <param name="repositoryPath">Path to the repository containing the file</param>
    /// <param name="fileId">GUID identifier of the file</param>
    /// <param name="originalFilePath">Current path to the file being reverted</param>
    /// <param name="selectedVersion">The version to revert to</param>
    /// <param name="snapshotPath">Path to the snapshot file to restore</param>
    /// <param name="jsonOptions">JSON serialization options</param>
    public static void RevertToVersion(string repositoryPath, Guid fileId, string originalFilePath, FileVersion selectedVersion, string snapshotPath, JsonSerializerOptions jsonOptions)
    {
        var historyFolder = GetHistoryFolderPath(repositoryPath, fileId);
        var extension = Path.GetExtension(originalFilePath);
        var versions = ReadVersionLogByHistoryFolder(historyFolder, jsonOptions);

        DeleteFutureVersions(historyFolder, extension, versions, selectedVersion.Version);

        var retainedVersions = versions
            .Where(v => v.Version <= selectedVersion.Version)
            .OrderBy(v => v.Version)
            .ToList();

        SaveVersionLogByHistoryFolder(historyFolder, retainedVersions, jsonOptions);
        File.Copy(snapshotPath, originalFilePath, true);
    }

    /// <summary>
    /// Reads the version log for a file identified by its GUID.
    /// </summary>
    /// <param name="repositoryPath">Path to the repository containing the file</param>
    /// <param name="fileId">GUID identifier of the file</param>
    /// <param name="jsonOptions">JSON serialization options</param>
    /// <returns>List of all versions for the file</returns>
    public static List<FileVersion> ReadVersionLog(string repositoryPath, Guid fileId, JsonSerializerOptions jsonOptions)
    {
        var historyFolder = GetHistoryFolderPath(repositoryPath, fileId);
        return ReadVersionLogByHistoryFolder(historyFolder, jsonOptions);
    }

    /// <summary>
    /// Constructs the path to the history folder for a file identified by its GUID.
    /// Path format: repositoryPath/.metadata/.history/{fileId}/
    /// </summary>
    /// <param name="repositoryPath">Path to the repository</param>
    /// <param name="fileId">GUID identifier of the file</param>
    /// <returns>Absolute path to the history folder for this file</returns>
    public static string GetHistoryFolderPath(string repositoryPath, Guid fileId)
    {
        return Path.Combine(repositoryPath, MetadataFolderName, HistoryFolderName, fileId.ToString());
    }

    private static List<FileVersion> ReadVersionLogByHistoryFolder(string historyFolder, JsonSerializerOptions jsonOptions)
    {
        var logPath = Path.Combine(historyFolder, "log.json");
        if (!File.Exists(logPath))
        {
            return [];
        }

        var json = File.ReadAllText(logPath);
        return JsonSerializer.Deserialize<List<FileVersion>>(json, jsonOptions) ?? [];
    }

    private static void SaveVersionLogByHistoryFolder(string historyFolder, List<FileVersion> versions, JsonSerializerOptions jsonOptions)
    {
        var logPath = Path.Combine(historyFolder, "log.json");
        File.WriteAllText(logPath, JsonSerializer.Serialize(versions.OrderBy(v => v.Version), jsonOptions));
    }

    private static void DeleteFutureVersions(
        string historyFolder,
        string extension,
        List<FileVersion> versions,
        int targetVersion)
    {
        foreach (var version in versions.Where(v => v.Version > targetVersion).ToList())
        {
            var snapshotPath = Path.Combine(historyFolder, $"v{version.Version}{extension}");
            if (File.Exists(snapshotPath))
            {
                File.Delete(snapshotPath);
            }
        }
    }

    private static void EnsureMetadataFolderHidden(string repositoryPath)
    {
        if (string.IsNullOrWhiteSpace(repositoryPath))
        {
            return;
        }

        var metadataFolder = Path.Combine(repositoryPath, MetadataFolderName);
        Directory.CreateDirectory(metadataFolder);

        if (Directory.Exists(metadataFolder))
        {
            var dirInfo = new DirectoryInfo(metadataFolder);
            dirInfo.Attributes |= FileAttributes.Hidden;
        }
    }
}
