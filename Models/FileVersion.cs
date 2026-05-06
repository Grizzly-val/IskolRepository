namespace IskolRepository.Models;

public class FileVersion
{
    /// <summary>
    /// Unique identifier of the file. Remains constant even if the file is renamed.
    /// Used to associate version snapshots with their file identity across renames.
    /// </summary>
    public Guid FileId { get; set; }

    public int Version { get; set; }

    public DateTime Timestamp { get; set; }

    public string Comment { get; set; } = string.Empty;

    public string SnapshotPath { get; set; } = string.Empty;

    public FileVersion() { }

    public FileVersion(int version, DateTime timestamp, string comment, string snapshotPath)
    {
        Version = version;
        Timestamp = timestamp;
        Comment = comment;
        SnapshotPath = snapshotPath;
    }

    public FileVersion(Guid fileId, int version, DateTime timestamp, string comment, string snapshotPath)
    {
        FileId = fileId;
        Version = version;
        Timestamp = timestamp;
        Comment = comment;
        SnapshotPath = snapshotPath;
    }

    public override string ToString()
    {
        return $"v{Version} - {Timestamp:yyyy-MM-dd HH:mm:ss} - {Comment}";
    }
}
