using System.Windows.Forms;
using IskolRepository.Core.Interfaces.Infrastructure;

namespace IskolRepository.Core.Services.Infrastructure;

/// <summary>
/// Implementation of IValidationHelper.
/// </summary>
public class ValidationHelperService : IValidationHelper
{
    private const string MetadataFileName = "metadata.json";
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

        var metadataPath = Path.Combine(path, MetadataFileName);
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

    public void ApplyNodeValidationColors(TreeNode node)
    {
        if (node is null)
            return;

        node.ForeColor = IsInsideRepository(node) ? SystemColors.WindowText : Color.Red;

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
        return string.Equals(fileName, MetadataFileName, StringComparison.OrdinalIgnoreCase)
            || string.Equals(fileName, semesterMarkerFileName, StringComparison.OrdinalIgnoreCase);
    }
}
