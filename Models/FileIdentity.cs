namespace IskolRepository.Models;

/// <summary>
/// Represents the identity and metadata of a file tracked by the version management system.
/// This record is stored in the identity manifest (.metadata/files.json) to maintain a stable
/// mapping between file GUIDs and their current paths, resilient to renames or moves.
/// </summary>
public class FileIdentity
{
    /// <summary>
    /// Unique identifier for this file. Remains constant even if the file is renamed or moved.
    /// </summary>
    public Guid FileId { get; set; }

    /// <summary>
    /// Current file path (absolute or relative to repository root). Updated when the file is renamed or moved.
    /// </summary>
    public string CurrentPath { get; set; } = string.Empty;

    /// <summary>
    /// The name of the file when it was first registered. Useful for recovery or forensics.
    /// </summary>
    public string OriginalFileName { get; set; } = string.Empty;

    /// <summary>
    /// DateTime when this file was first registered in the identity manifest.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Current status of the file identity. Values: "active", "orphaned", "deleted".
    /// "active" = file exists on disk and is tracked normally.
    /// "orphaned" = file no longer exists on disk; history is preserved.
    /// "deleted" = file was explicitly deleted; history retained for potential recovery.
    /// </summary>
    public string Status { get; set; } = "active";
}
