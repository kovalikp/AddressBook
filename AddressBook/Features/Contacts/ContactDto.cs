// Copyright (c) Pavol Kovalik.
// Licensed under the MIT License.

using AddressBook.Models;

namespace AddressBook.Features.Contacts
{
    public class ContactDto
    {
        public ContactDto(Contact contact)
        {
            Id = contact.Id;
            Name = contact.Name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}
