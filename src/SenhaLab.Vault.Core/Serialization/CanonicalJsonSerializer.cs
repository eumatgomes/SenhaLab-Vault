using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SenhaLab.Vault.Core.Serialization;

public static class CanonicalJsonSerializer
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };

    private static readonly JsonWriterOptions WriterOptions = new()
    {
        Indented = false,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        SkipValidation = false
    };

    public static byte[] SerializeToUtf8Bytes<T>(T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var json = JsonSerializer.Serialize(value, SerializerOptions);

        return CanonicalizeJson(json);
    }

    public static byte[] CanonicalizeJson(string json)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(json);

        using var document = JsonDocument.Parse(json);
        using var stream = new MemoryStream();

        using (var writer = new Utf8JsonWriter(stream, WriterOptions))
        {
            WriteCanonicalElement(writer, document.RootElement);
        }

        return stream.ToArray();
    }

    private static void WriteCanonicalElement(
        Utf8JsonWriter writer,
        JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                writer.WriteStartObject();

                foreach (var property in element
                    .EnumerateObject()
                    .OrderBy(
                        property => property.Name,
                        UnicodeCodePointComparer.Instance))
                {
                    writer.WritePropertyName(property.Name);
                    WriteCanonicalElement(writer, property.Value);
                }

                writer.WriteEndObject();
                break;

            case JsonValueKind.Array:
                writer.WriteStartArray();

                foreach (var item in element.EnumerateArray())
                {
                    WriteCanonicalElement(writer, item);
                }

                writer.WriteEndArray();
                break;

            case JsonValueKind.String:
                writer.WriteStringValue(element.GetString());
                break;

            case JsonValueKind.Number:
                WriteCanonicalNumber(writer, element);
                break;

            case JsonValueKind.True:
                writer.WriteBooleanValue(true);
                break;

            case JsonValueKind.False:
                writer.WriteBooleanValue(false);
                break;

            case JsonValueKind.Null:
                writer.WriteNullValue();
                break;

            default:
                throw new JsonException(
                    "Tipo de valor JSON não suportado."
                );
        }
    }

    private static void WriteCanonicalNumber(
        Utf8JsonWriter writer,
        JsonElement element)
    {
        var raw = element.GetRawText();

        if (!long.TryParse(
            raw,
            NumberStyles.AllowLeadingSign,
            CultureInfo.InvariantCulture,
            out var integer))
        {
            throw new JsonException(
                "A codificação canônica v1 aceita apenas números inteiros de 64 bits."
            );
        }

        writer.WriteNumberValue(integer);
    }

    private sealed class UnicodeCodePointComparer : IComparer<string>
    {
        public static UnicodeCodePointComparer Instance { get; } = new();

        public int Compare(string? x, string? y)
        {
            if (ReferenceEquals(x, y))
                return 0;

            if (x is null)
                return -1;

            if (y is null)
                return 1;

            var left = x.EnumerateRunes().GetEnumerator();
            var right = y.EnumerateRunes().GetEnumerator();

            while (true)
            {
                var hasLeft = left.MoveNext();
                var hasRight = right.MoveNext();

                if (!hasLeft || !hasRight)
                {
                    if (hasLeft)
                        return 1;

                    if (hasRight)
                        return -1;

                    return 0;
                }

                var comparison =
                    left.Current.Value.CompareTo(right.Current.Value);

                if (comparison != 0)
                    return comparison;
            }
        }
    }
}

