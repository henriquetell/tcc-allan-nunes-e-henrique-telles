using Newtonsoft.Json;
using System;

namespace Framework.ValueObjects
{
    internal class StringValueJsonConverter<TValueObject> : JsonConverter
        where TValueObject : IStringValueObject
    {
        public override bool CanConvert(Type objectType) => typeof(TValueObject).IsAssignableFrom(objectType);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
            typeof(TValueObject).IsAssignableFrom(objectType)
                ? Activator.CreateInstance(typeof(TValueObject), reader.Value?.ToString())
                : new InvalidCastException();

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is TValueObject)
                writer.WriteValue(((TValueObject)value).Value);
        }
    }
}
