using IskolRepository.Core.Interfaces.Infrastructure;

namespace IskolRepository.Core.Services.Infrastructure;

/// <summary>
/// Implementation of IFileSystemHelper using System.IO.
/// </summary>
public class FileSystemService : IFileSystemHelper
{
    public void CreateDirectory(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Path cannot be null or empty.", nameof(path));

        Directory.CreateDirectory(path);
    }

    public bool DirectoryExists(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;

        return Directory.Exists(path);
    }

    public bool FileExists(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;

        return File.Exists(path);
    }

    public bool IsDirectoryEmpty(string path)
    {
        if (!DirectoryExists(path))
            return true;

        return !Directory.EnumerateFileSystemEntries(path).Any();
    }

    public bool IsValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        var invalidChars = Path.GetInvalidFileNameChars();
        return !name.Any(c => invalidChars.Contains(c));
    }

    public string? CreateRepositoryFile(string repositoryPath, string fileName, string extension)
    {
        // This should delegate to FileSystemHelper from existing code if it has this logic
        // For now, we'll create a placeholder that can be implemented later
        try
        {
            var filePath = Path.Combine(repositoryPath, fileName + extension);
            if (FileExists(filePath))
                return null;

            // Create empty file
            File.WriteAllText(filePath, string.Empty);
            return filePath;
        }
        catch
        {
            return null;
        }
    }

    public IEnumerable<string> EnumerateFiles(string path)
    {
        if (!DirectoryExists(path))
            return Enumerable.Empty<string>();

        try
        {
            return Directory.EnumerateFiles(path);
        }
        catch
        {
            return Enumerable.Empty<string>();
        }
    }

    public IEnumerable<string> EnumerateDirectories(string path)
    {
        if (!DirectoryExists(path))
            return Enumerable.Empty<string>();

        try
        {
            return Directory.EnumerateDirectories(path);
        }
        catch
        {
            return Enumerable.Empty<string>();
        }
    }

    public IEnumerable<string> EnumerateFileSystemEntries(string path)
    {
        if (!DirectoryExists(path))
            return Enumerable.Empty<string>();

        try
        {
            return Directory.EnumerateFileSystemEntries(path);
        }
        catch
        {
            return Enumerable.Empty<string>();
        }
    }

    public string ReadAllText(string filePath)
    {
        if (!FileExists(filePath))
            throw new FileNotFoundException("File not found.", filePath);

        return File.ReadAllText(filePath);
    }

    public void WriteAllText(string filePath, string content)
    {
        File.WriteAllText(filePath, content);
    }

    public void SetFileAttributes(string filePath, System.IO.FileAttributes attributes)
    {
        if (!FileExists(filePath))
            throw new FileNotFoundException("File not found.", filePath);

        File.SetAttributes(filePath, attributes);
    }

    public System.IO.FileAttributes GetFileAttributes(string filePath)
    {
        if (!FileExists(filePath))
            throw new FileNotFoundException("File not found.", filePath);

        return File.GetAttributes(filePath);
    }
}
