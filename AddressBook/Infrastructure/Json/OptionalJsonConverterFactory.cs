// Copyright (c) Pavol Kovalik.
// Licensed under the MIT License.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AddressBook.Infrastructure.Json
{
    public class OptionalJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
            => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Optional<>);

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            Type wrappedType = typeToConvert.GetGenericArguments()[0];
            JsonConverter? converter = (JsonConverter?)Activator.CreateInstance(
                typeof(OptionalJsonConverter<>).MakeGenericType(wrappedType));

            return converter;
        }
    }
}
