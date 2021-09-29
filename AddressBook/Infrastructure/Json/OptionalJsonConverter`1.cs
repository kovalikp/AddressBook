// Copyright (c) Pavol Kovalik.
// Licensed under the MIT License.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AddressBook.Infrastructure.Json
{
    public class OptionalJsonConverter<T> : JsonConverter<Optional<T>>
    {
        public override Optional<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            T? value = JsonSerializer.Deserialize<T>(ref reader, options);

            // During deserialization, there is no way to differentiate between nullable and non-nullable reference values.
            // Therefore serializer will set reference value to null in either case. This is consistent with default deserialization.
            return new Optional<T>(value!);

        }
        public override void Write(Utf8JsonWriter writer, Optional<T> value, JsonSerializerOptions options)
        {
            if (!value.TryGetValue(out T? innerValue))
            {
                // Unlike Netownsoft.Json, System.Text.Json does not support general mechanism to suppress property serialization.
                // Newtonsoft.Json can do this via https://www.newtonsoft.com/json/help/html/contractresolver.htm.
                throw new InvalidOperationException("Cannot serialize not-set optional value. Mark member using [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)].");
            }

            JsonSerializer.Serialize(writer, innerValue, options);
        }
    }
}
