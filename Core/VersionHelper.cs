using System.Text.Json;
using IskolRepository.Models;

namespace IskolRepository.Core;

public static class VersionHelper
{
    public const string HistoryFolderName = ".history";

    public static void SaveVersion(string filePath, string comment, JsonSerializerOptions jsonOptions)
    {
        var historyFolder = GetHistoryFolderPath(filePath);
        Directory.CreateDirectory(historyFolder);

        var versions = ReadVersionLogByHistoryFolder(historyFolder, jsonOptions);
        var nextVersion = versions.Count == 0 ? 1 : versions.Max(v => v.Version) + 1;
        var snapshotPath = Path.Combine(historyFolder, $"v{nextVersion}{Path.GetExtension(filePath)}");

        File.Copy(filePath, snapshotPath, true);
        versions.Add(new FileVersion
        {
            Version = nextVersion,
            Timestamp = DateTime.Now,
            Comment = comment
        });

        SaveVersionLogByHistoryFolder(historyFolder, versions, jsonOptions);
    }

    public static void RevertToVersion(string originalFilePath, FileVersion selectedVersion, string snapshotPath, JsonSerializerOptions jsonOptions)
    {
        var historyFolder = GetHistoryFolderPath(originalFilePath);
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

    public static List<FileVersion> ReadVersionLog(string filePath, JsonSerializerOptions jsonOptions)
    {
        var historyFolder = GetHistoryFolderPath(filePath);
        return ReadVersionLogByHistoryFolder(historyFolder, jsonOptions);
    }

    public static string GetHistoryFolderPath(string filePath)
    {
        var repositoryPath = Path.GetDirectoryName(filePath)
            ?? throw new InvalidOperationException("File path is missing a parent repository.");

        return Path.Combine(repositoryPath, HistoryFolderName, Path.GetFileNameWithoutExtension(filePath));
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
}
