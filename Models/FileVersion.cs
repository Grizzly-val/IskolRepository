namespace IskolRepository.Models;

public class FileVersion
{
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

    public override string ToString()
    {
        return $"v{Version} - {Timestamp:yyyy-MM-dd HH:mm:ss} - {Comment}";
    }
}
