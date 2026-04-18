namespace IskolRepository.Core.Interfaces.Infrastructure;

/// <summary>
/// Provides abstraction for path operations.
/// </summary>
public interface IPathProvider
{
    /// <summary>
    /// Combines multiple path segments.
    /// </summary>
    string CombinePaths(params string[] paths);

    /// <summary>
    /// Gets the file name from a path.
    /// </summary>
    string GetFileName(string path);

    /// <summary>
    /// Gets the directory name from a path.
    /// </summary>
    string? GetDirectoryName(string path);

    /// <summary>
    /// Gets the file name without extension.
    /// </summary>
    string GetFileNameWithoutExtension(string path);

    /// <summary>
    /// Gets the extension of a file.
    /// </summary>
    string GetExtension(string path);

    /// <summary>
    /// Gets the full path for a relative path.
    /// </summary>
    string GetFullPath(string path);
}
