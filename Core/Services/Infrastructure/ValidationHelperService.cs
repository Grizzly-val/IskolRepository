using IskolRepository.Core.Interfaces.Infrastructure;
using IskolRepository.Core.Services;
using System.Windows.Forms;

namespace IskolRepository.Core.Services.Infrastructure;

/// <summary>
/// Implementation of IValidationHelper.
/// </summary>
public class ValidationHelperService : IValidationHelper
{
    private static readonly string[] ValidStatuses = ["in-progress", "completed", "late"];
    private readonly IFileSystemHelper _fileSystemHelper;

    public ValidationHelperService(IFileSystemHelper fileSystemHelper)
    {
        _fileSystemHelper = fileSystemHelper ?? throw new ArgumentNullException(nameof(fileSystemHelper));
    }

    public bool IsRepositoryFolder(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;

        if (!_fileSystemHelper.DirectoryExists(path))
            return false;

        var metadataPath = Path.Combine(
            path,
            RepositoryService.MetadataFolderName,
            RepositoryService.MetadataFileName);
        return _fileSystemHelper.FileExists(metadataPath);
    }

    public bool IsValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        var trimmedName = name.Trim();
        return trimmedName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0
            && !trimmedName.EndsWith('.')
            && !trimmedName.EndsWith(' ');
    }

    public bool IsInsideRepository(TreeNode node)
    {
        if (node is null)
            return false;

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
            return false;

        var currentPath = path;
        if (_fileSystemHelper.FileExists(currentPath))
        {
            currentPath = Path.GetDirectoryName(currentPath);
        }

        while (!string.IsNullOrWhiteSpace(currentPath))
        {
            if (IsRepositoryFolder(currentPath))
                return true;

            currentPath = Path.GetDirectoryName(currentPath);
        }

        return false;
    }

    /// <summary>
    /// Determines if a node should be marked as invalid (red).
    /// A node is invalid if it's a non-repository item directly under a Subject,
    /// i.e., files or directories that should be inside repositories instead.
    /// </summary>
    private bool IsInvalidNode(TreeNode node)
    {
        if (node?.Tag is not NodeData nodeData)
            return false;

        // Subject folders and above are always valid
        if (nodeData.NodeType == NodeType.Semester || nodeData.NodeType == NodeType.Subject)
            return false;

        // Get the parent node
        var parentNode = node.Parent;
        if (parentNode?.Tag is not NodeData parentData)
            return false;

        // Only mark as invalid if parent is Subject AND this is NOT a repository folder
        if (parentData.NodeType == NodeType.Subject)
        {
            return !IsRepositoryFolder(nodeData.Path);
        }

        return false;
    }

    public void ApplyNodeValidationColors(TreeNode node)
    {
        if (node is null)
            return;

        // Color the current node red if it's invalid
        node.ForeColor = IsInvalidNode(node) ? Color.Red : SystemColors.WindowText;

        // Recursively apply to all children
        foreach (TreeNode child in node.Nodes)
        {
            ApplyNodeValidationColors(child);
        }
    }

    public bool IsValidStatus(string? status)
    {
        return !string.IsNullOrWhiteSpace(status)
            && ValidStatuses.Contains(status, StringComparer.OrdinalIgnoreCase);
    }

    public bool IsSystemManagedFile(string filePath, string semesterMarkerFileName)
    {
        var fileName = Path.GetFileName(filePath);
        var parentFolderName = Path.GetFileName(Path.GetDirectoryName(filePath) ?? string.Empty);
        var isMetadataJson = string.Equals(fileName, RepositoryService.MetadataFileName, StringComparison.OrdinalIgnoreCase)
            && string.Equals(parentFolderName, RepositoryService.MetadataFolderName, StringComparison.OrdinalIgnoreCase);

        return isMetadataJson
            || string.Equals(fileName, semesterMarkerFileName, StringComparison.OrdinalIgnoreCase);
    }
}
