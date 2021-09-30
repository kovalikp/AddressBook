using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Xunit;

namespace AddressBook.Infrastructure.Json
{
    public class OptionalSerializationTests
    {
        private static readonly JsonSerializerOptions s_jsonSerializerOptions = new(JsonSerializerDefaults.Web);

        [Theory]
        [InlineData("{}", -1)]
        [InlineData(@"{""property"":0}", 0)]
        public void Json_can_be_deserialized_as_optional_int_property(string json, int propertyValue)

        {
            ValueProperty? value = JsonSerializer.Deserialize<ValueProperty>(json, s_jsonSerializerOptions);
            value!.Property.Should().Be(new Optional<int>(propertyValue));
        }

        [Theory]
        [InlineData("{}", -1)]
        [InlineData(@"{""property"":0}", 0)]
        [InlineData(@"{""property"":null}", null)]
        public void Json_can_be_deserialized_as_optional_nullable_int_property(string json, int? propertyValue)
        {
            NullableValueProperty? value = JsonSerializer.Deserialize<NullableValueProperty>(json, s_jsonSerializerOptions);
            value!.Property.Should().Be(new Optional<int?>(propertyValue));

        }

        [Theory]
        [InlineData("{}", "default")]
        [InlineData(@"{""property"":""""}", "")]
        [InlineData(@"{""property"":null}", null)]
        public void Json_can_be_deserialized_as_optional_nullable_string_property(string json, string propertyValue)
        {
            NullableReferenceProperty? value = JsonSerializer.Deserialize<NullableReferenceProperty>(json, s_jsonSerializerOptions);
            value!.Property.Should().Be(new Optional<string>(propertyValue));
        }

        [Theory]
        [InlineData("{}", "default")]
        [InlineData(@"{""property"":""""}", "")]
        public void Json_can_be_deserialized_as_optional_string_property(string json, string propertyValue)
        {
            ReferenceProperty? value = JsonSerializer.Deserialize<ReferenceProperty>(json, s_jsonSerializerOptions);
            value!.Property.Should().Be(new Optional<string>(propertyValue));

        }

        [Fact]
        public void Optional_property_without_value_is_not_serialized()
        {
            var value = new NullableReferenceProperty { Property = default };
            string? json = JsonSerializer.Serialize(value, s_jsonSerializerOptions);
            json.Should().Be("{}");
        }

        [Fact]
        public void Optional_property_with_value_is_serialized()
        {
            var value = new NullableReferenceProperty { Property = null };
            string? json = JsonSerializer.Serialize(value, s_jsonSerializerOptions);
            json.Should().Be(@"{""property"":null}");
        }

        public class NullableReferenceProperty
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public Optional<string?> Property { get; set; } = "default";
        }

        public class ReferenceProperty
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public Optional<string> Property { get; set; } = "default";
        }

        public class NullableValueProperty
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public Optional<int?> Property { get; set; } = -1;
        }

        public class ValueProperty
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public Optional<int> Property { get; set; } = -1;
        }
    }
}
