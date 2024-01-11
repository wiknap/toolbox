using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wiknap.PayNow.Converters;

public abstract class CustomEnumJsonConverter<T> : JsonConverter<T> where T : Enum
{
    private readonly IReadOnlyDictionary<T, string> mappingDictionary;

    protected CustomEnumJsonConverter(IReadOnlyDictionary<T, string> mappingDictionary)
    {
        this.mappingDictionary = mappingDictionary;
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stringValue = reader.GetString();

        foreach (var mapping in mappingDictionary)
        {
            if (mapping.Value == stringValue)
                return mapping.Key;
        }

        throw new JsonException($"Unable to deserialize \"{stringValue}\" to {typeToConvert} type");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (!mappingDictionary.TryGetValue(value, out var stringValue))
            throw new JsonException($"Unable to serialize \"{value}\" to {nameof(T)} type");

        writer.WriteStringValue(stringValue);
    }
}
