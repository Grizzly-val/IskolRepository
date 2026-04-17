using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IskolRepository.Utilities;

public class DateOnlyDateTimeConverter : JsonConverter<DateTime>
{
    private const string DateFormat = "yyyy-MM-dd";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (DateTime.TryParseExact(
            value,
            DateFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var parsedDate))
        {
            return parsedDate;
        }

        return DateTime.Parse(value ?? string.Empty, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(DateFormat, CultureInfo.InvariantCulture));
    }
}
