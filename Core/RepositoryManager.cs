using System.Text.Json;
using System.Windows.Forms;
using IskolRepository.Models;

namespace IskolRepository.Core;

/// <summary>
/// Manages repository-related operations including metadata handling and selection.
/// </summary>
public static class RepositoryManager
{
    private const string MetadataFileName = "metadata.json";

    public static RepoMetadata EnsureMetadata(string repositoryPath, JsonSerializerOptions jsonOptions)
    {
        var metadataPath = Path.Combine(repositoryPath, MetadataFileName);
        if (!File.Exists(metadataPath))
        {
            var metadata = new RepoMetadata
            {
                Deadline = DateTime.Today,
                DateAdded = DateTime.Today,
                Status = "in-progress"
            };

            SaveMetadata(repositoryPath, metadata, jsonOptions);
            return metadata;
        }

        var json = File.ReadAllText(metadataPath);
        var metadataFromFile = JsonSerializer.Deserialize<RepoMetadata>(json, jsonOptions);
        if (metadataFromFile is null || !ValidationHelper.IsValidStatus(metadataFromFile.Status, new[] { "in-progress", "completed", "late" }))
        {
            throw new InvalidOperationException("The repository metadata is invalid.");
        }

        return metadataFromFile;
    }

    public static void SaveMetadata(string repositoryPath, RepoMetadata metadata, JsonSerializerOptions jsonOptions, string[]? validStatuses = null)
    {
        validStatuses ??= new[] { "in-progress", "completed", "late" };

        if (!ValidationHelper.IsValidStatus(metadata.Status, validStatuses))
        {
            throw new InvalidOperationException("Metadata status is invalid.");
        }

        var metadataPath = Path.Combine(repositoryPath, MetadataFileName);
        File.WriteAllText(metadataPath, JsonSerializer.Serialize(metadata, jsonOptions));
    }

    public static string? FindRepositoryRoot(string startPath)
    {
        var currentPath = startPath;
        while (!string.IsNullOrWhiteSpace(currentPath))
        {
            if (ValidationHelper.IsRepositoryFolder(currentPath))
            {
                return currentPath;
            }

            currentPath = Path.GetDirectoryName(currentPath);
        }

        return null;
    }
}
