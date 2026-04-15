namespace IskolRepository;

public class FileVersion
{
    public int Version { get; set; }

    public DateTime Timestamp { get; set; }

    public string Comment { get; set; } = string.Empty;
}
