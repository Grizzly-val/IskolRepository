using System.Text.Json.Serialization;
using IskolRepository.Utilities;

namespace IskolRepository.Models;

public class RepoMetadata
{
    [JsonConverter(typeof(DateOnlyDateTimeConverter))]
    public DateTime Deadline { get; set; }

    [JsonConverter(typeof(DateOnlyDateTimeConverter))]
    public DateTime DateAdded { get; set; }

    public string Status { get; set; } = "in-progress";

    [JsonConverter(typeof(DateOnlyDateTimeConverter))]
    public DateTime? Submitted { get; set; } = null;
}
