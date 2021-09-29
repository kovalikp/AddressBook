// Copyright (c) Pavol Kovalik.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AddressBook.Infrastructure.Json;

namespace AddressBook
{
    [JsonConverter(typeof(OptionalJsonConverterFactory))]
    public struct Optional<T> : IEquatable<Optional<T>>
    {
        public T _value;

        public bool _hasValue;

        public bool HasValue => _hasValue;

        public T Value => _hasValue ? _value : throw new InvalidOperationException("");

        public Optional(T value)
        {
            _value = value;
            _hasValue = true;
        }

        public T? ValueOrDefault() => _value;

        public bool TryGetValue([MaybeNullWhen(false)] out T value)
        {
            value = _value;
            return _hasValue;
        }

        public bool Equals(Optional<T> other)
            => Equals(other, EqualityComparer<T>.Default);

        public bool Equals(Optional<T> other, IEqualityComparer<T> comparer)
            => _hasValue == other._hasValue && comparer.Equals(_value, other._value);

        public override bool Equals(object? obj)
            => Equals(obj, EqualityComparer<T>.Default);

        public bool Equals(object? obj, IEqualityComparer<T> comparer)
        {
            return obj switch
            {
                Optional<T> other => Equals(other, comparer),
                T otherValue => _hasValue && comparer.Equals(_value, otherValue),
                _ => false
            };
        }

        public override int GetHashCode()
            => HashCode.Combine(HasValue, Value);

        public static bool operator ==(Optional<T> left, Optional<T> right)
            => left.Equals(right);

        public static bool operator !=(Optional<T> left, Optional<T> right)
            => !(left == right);

        public static implicit operator Optional<T>(T value)
            => new(value);
    }
}
