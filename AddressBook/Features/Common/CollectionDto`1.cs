// Copyright (c) Pavol Kovalik.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace AddressBook.Features.Common
{
    public class CollectionDto<T>
    {
        public CollectionDto(IReadOnlyCollection<T> value)
        {
            Value = value;
        }

        public IReadOnlyCollection<T> Value { get; }
    }
}
