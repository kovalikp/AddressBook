// Copyright (c) Pavol Kovalik.
// Licensed under the MIT License.

using System.Collections.Generic;
using AddressBook.Features.Common;

namespace AddressBook.Features.Contacts
{
    public class ContactsDto : CollectionDto<ContactDto>
    {
        public ContactsDto(IReadOnlyCollection<ContactDto> value) : base(value)
        {
        }
    }
}
