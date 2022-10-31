using Newtonsoft.Json;
using Ow.Net.Data;

namespace Ow.Net;

public sealed class PlatformJsonConverter : JsonConverter {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        => writer.WriteValue(((Platform)value).ToString());

    public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        => Platform.Parse((string)reader.Value).ValueOrDefault();

    public override bool CanConvert(Type objectType)
        => objectType == typeof(Platform);
}
