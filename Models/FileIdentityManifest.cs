namespace IskolRepository.Models;

/// <summary>
/// The identity manifest stores the mapping of file GUIDs to their current paths and metadata.
/// It is persisted as .metadata/files.json in each repository.
/// </summary>
public class FileIdentityManifest
{
    /// <summary>
    /// Dictionary mapping file GUIDs to their identity records.
    /// This allows quick lookup of a file's current path and metadata by its ID.
    /// </summary>
    public Dictionary<string, FileIdentity> Files { get; set; } = [];

    /// <summary>
    /// Manifest schema version for future compatibility.
    /// Increment this when making breaking changes to the manifest structure.
    /// Current version: 1
    /// </summary>
    public int ManifestVersion { get; set; } = 1;

    /// <summary>
    /// DateTime when the manifest was last updated.
    /// Useful for diagnostics and determining if reconciliation is needed.
    /// </summary>
    public DateTime LastUpdated { get; set; } = DateTime.Now;
}
