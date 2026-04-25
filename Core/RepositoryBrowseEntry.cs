namespace IskolRepository.Core;

/// <summary>
/// Represents a browsable entry shown in the repository file list.
/// </summary>
public sealed class RepositoryBrowseEntry
{
    public RepositoryBrowseEntry(RepositoryBrowseEntryKind kind, string path, string displayName)
    {
        Kind = kind;
        Path = path;
        DisplayName = displayName;
    }

    public RepositoryBrowseEntryKind Kind { get; }

    public string Path { get; }

    public string DisplayName { get; }
}

public enum RepositoryBrowseEntryKind
{
    Parent,
    Directory,
    File
}
