using System.IO;

namespace IskolRepository.Core;

/// <summary>
/// Represents the type of a tree node in the repository hierarchy.
/// </summary>
public enum NodeType
{
    Semester,
    Subject,
    Repository,
    SubRepository,
    File
}

/// <summary>
/// Contains metadata for a TreeNode tag, storing the path and node type.
/// </summary>
public sealed class NodeData
{
    public NodeData(string path, NodeType nodeType, bool isValidFile = true)
    {
        
        Path = path;
        FileName = System.IO.Path.GetFileName(path);
        NodeType = nodeType;
        IsValidFile = isValidFile;
    }

    public string FileName { get; }

    public string Path { get; }

    public NodeType NodeType { get; }

    /// <summary>
    /// Indicates if a File node is valid (inside a repository).
    /// Only meaningful when NodeType is File. Defaults to true for non-File nodes.
    /// </summary>
    public bool IsValidFile { get; }
}

/// <summary>
/// Represents a version list item for display in the version history list.
/// </summary>
public sealed class VersionListItem
{
    public VersionListItem(string snapshotPath, Models.FileVersion version)
    {
        SnapshotPath = snapshotPath;
        Version = version;
    }

    public string SnapshotPath { get; }

    public Models.FileVersion Version { get; }

    public override string ToString()
    {
        return $"v{Version.Version} - {Version.Timestamp:yyyy-MM-dd HH:mm} - {Version.Comment}";
    }
}
