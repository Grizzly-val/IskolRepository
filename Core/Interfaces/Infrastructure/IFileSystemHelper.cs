namespace IskolRepository.Core.Interfaces.Infrastructure;

/// <summary>
/// Provides abstraction for file system operations.
/// </summary>
public interface IFileSystemHelper
{
    /// <summary>
    /// Creates a directory at the specified path.
    /// </summary>
    void CreateDirectory(string path);

    /// <summary>
    /// Checks if a directory exists.
    /// </summary>
    bool DirectoryExists(string path);

    /// <summary>
    /// Checks if a file exists.
    /// </summary>
    bool FileExists(string path);

    /// <summary>
    /// Checks if a directory is empty.
    /// </summary>
    bool IsDirectoryEmpty(string path);

    /// <summary>
    /// Validates if a name is valid for file/directory creation.
    /// </summary>
    bool IsValidName(string name);

    /// <summary>
    /// Creates a repository file with version tracking.
    /// </summary>
    string? CreateRepositoryFile(string repositoryPath, string fileName, string extension);

    /// <summary>
    /// Enumerates all files in a directory.
    /// </summary>
    IEnumerable<string> EnumerateFiles(string path);

    /// <summary>
    /// Enumerates all directories in a directory.
    /// </summary>
    IEnumerable<string> EnumerateDirectories(string path);

    /// <summary>
    /// Enumerates all file system entries (files and directories).
    /// </summary>
    IEnumerable<string> EnumerateFileSystemEntries(string path);

    /// <summary>
    /// Reads all text from a file.
    /// </summary>
    string ReadAllText(string filePath);

    /// <summary>
    /// Writes all text to a file.
    /// </summary>
    void WriteAllText(string filePath, string content);

    /// <summary>
    /// Sets file attributes (e.g., Hidden).
    /// </summary>
    void SetFileAttributes(string filePath, System.IO.FileAttributes attributes);

    /// <summary>
    /// Gets file attributes.
    /// </summary>
    System.IO.FileAttributes GetFileAttributes(string filePath);
}
