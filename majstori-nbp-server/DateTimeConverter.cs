using System.Text.Json;
using System.Text.Json.Serialization;

public class DateTimeConverter : JsonConverter<DateTime>
{
    private const string Format = "dd.MM.yyyy HH:mm:ss";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Pokušaj da parsiraš datum po zadatom formatu
        if (DateTime.TryParseExact(reader.GetString(), Format, null, System.Globalization.DateTimeStyles.None, out var date))
            return date;

        // Ako nije moguće, koristi standardni parser kao fallback
        return DateTime.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format));
    }
}