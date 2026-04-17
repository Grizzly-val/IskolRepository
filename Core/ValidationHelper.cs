using System.Windows.Forms;
using IskolRepository.Models;

namespace IskolRepository.Core;

/// <summary>
/// Provides validation and color management for tree nodes and repository detection.
/// </summary>
public static class ValidationHelper
{
    private const string MetadataFileName = "metadata.json";

    public static bool IsRepositoryFolder(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return false;
        }

        if (!Directory.Exists(path))
        {
            return false;
        }

        var metadataPath = Path.Combine(path, MetadataFileName);
        return File.Exists(metadataPath);
    }

    public static bool IsInsideRepository(TreeNode node)
    {
        if (node is null)
        {
            return false;
        }

        string? path = null;
        if (node.Tag is NodeData nd)
        {
            path = nd.Path;
        }
        else if (node.Tag is string s)
        {
            path = s;
        }

        if (string.IsNullOrWhiteSpace(path))
        {
            return false;
        }

        // Start from the node's path. If it's a file, move to its directory.
        var currentPath = path;
        if (File.Exists(currentPath))
        {
            currentPath = Path.GetDirectoryName(currentPath);
        }

        while (!string.IsNullOrWhiteSpace(currentPath))
        {
            if (IsRepositoryFolder(currentPath))
            {
                return true;
            }

            currentPath = Path.GetDirectoryName(currentPath);
        }

        return false;
    }

    public static void ApplyNodeValidationColors(TreeNode node)
    {
        if (node is null)
        {
            return;
        }

        node.ForeColor = IsInsideRepository(node) ? SystemColors.WindowText : Color.Red;

        foreach (TreeNode child in node.Nodes)
        {
            ApplyNodeValidationColors(child);
        }
    }

    public static bool IsValidStatus(string? status, string[] validStatuses)
    {
        return !string.IsNullOrWhiteSpace(status)
            && validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase);
    }

    public static bool IsSystemManagedFile(string filePath, string semesterMarkerFileName)
    {
        var fileName = Path.GetFileName(filePath);
        return string.Equals(fileName, MetadataFileName, StringComparison.OrdinalIgnoreCase)
            || string.Equals(fileName, semesterMarkerFileName, StringComparison.OrdinalIgnoreCase);
    }
}
