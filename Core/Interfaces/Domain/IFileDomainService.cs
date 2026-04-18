using System.Windows.Forms;

namespace IskolRepository.Core.Interfaces.Domain;

/// <summary>
/// Domain service for file operations.
/// </summary>
public interface IFileDomainService
{
    /// <summary>
    /// Loads files from a repository and populates a ListView.
    /// </summary>
    void LoadFiles(string repositoryPath, ListView filesListView, string semesterMarkerFileName);

    /// <summary>
    /// Creates a folder with validation.
    /// </summary>
    /// <returns>True if successful, error message in out parameter</returns>
    bool CreateFolder(string parentPath, string name, string folderType, out string? error);

    /// <summary>
    /// Creates a repository file.
    /// </summary>
    /// <returns>Created file path if successful, null otherwise</returns>
    string? CreateRepositoryFile(string repositoryPath, string fileName, string extension, out string? error);

    /// <summary>
    /// Opens a file with the default application.
    /// </summary>
    void OpenFile(string filePath, Action<string> onFileExited);
}
