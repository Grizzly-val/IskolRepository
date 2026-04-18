using IskolRepository.Core.Interfaces.Infrastructure;

namespace IskolRepository.Core.Services.Infrastructure;

/// <summary>
/// Implementation of IPathProvider using System.IO.Path.
/// </summary>
public class PathProviderService : IPathProvider
{
    public string CombinePaths(params string[] paths)
    {
        if (paths == null || paths.Length == 0)
            throw new ArgumentException("At least one path is required.", nameof(paths));

        return Path.Combine(paths);
    }

    public string GetFileName(string path)
    {
        return Path.GetFileName(path);
    }

    public string? GetDirectoryName(string path)
    {
        return Path.GetDirectoryName(path);
    }

    public string GetFileNameWithoutExtension(string path)
    {
        return Path.GetFileNameWithoutExtension(path);
    }

    public string GetExtension(string path)
    {
        return Path.GetExtension(path);
    }

    public string GetFullPath(string path)
    {
        return Path.GetFullPath(path);
    }
}
