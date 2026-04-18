using System.Windows.Forms;
using IskolRepository.Models;

namespace IskolRepository.Core.Interfaces.Application;

/// <summary>
/// Application service for file use cases.
/// </summary>
public interface IFileApplicationService
{
    /// <summary>
    /// Creates a new file in a repository.
    /// </summary>
    /// <returns>File path if successful</returns>
    /// <exception cref="InvalidOperationException">Thrown if creation fails</exception>
    string CreateFile(string repositoryPath, string fileName, string extension);

    /// <summary>
    /// Opens a file and tracks when it closes.
    /// </summary>
    void OpenAndTrackFile(string filePath, Action<string> onFileClosed);

    /// <summary>
    /// Reverts a file to a previous version.
    /// </summary>
    void RevertFileVersion(string filePath, FileVersion selectedVersion);

    /// <summary>
    /// Loads files from a repository and populates the listView.
    /// </summary>
    void LoadFiles(string repositoryPath, ListView filesListView, string semesterMarkerFileName);
}
