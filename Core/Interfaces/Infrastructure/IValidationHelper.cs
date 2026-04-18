using System.Windows.Forms;

namespace IskolRepository.Core.Interfaces.Infrastructure;

/// <summary>
/// Provides abstraction for validation operations.
/// </summary>
public interface IValidationHelper
{
    /// <summary>
    /// Checks if a path is a valid repository folder.
    /// </summary>
    bool IsRepositoryFolder(string path);

    /// <summary>
    /// Checks if a TreeNode is inside a repository.
    /// </summary>
    bool IsInsideRepository(TreeNode node);

    /// <summary>
    /// Applies validation colors to tree nodes.
    /// </summary>
    void ApplyNodeValidationColors(TreeNode node);

    /// <summary>
    /// Validates if a status is valid.
    /// </summary>
    bool IsValidStatus(string? status);

    /// <summary>
    /// Checks if a file is system-managed (metadata, marker files).
    /// </summary>
    bool IsSystemManagedFile(string filePath, string semesterMarkerFileName);

    /// <summary>
    /// Validates if a name is valid for files and folders.
    /// </summary>
    bool IsValidName(string name);
}
