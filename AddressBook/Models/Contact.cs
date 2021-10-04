// Copyright (c) Pavol Kovalik.
// Licensed under the MIT License.

namespace AddressBook.Models
{
    public class Contact
    {
        public Contact(string name, int id = default)
        {
            Name = name;
            Id = id;
        }

        public int Id { get; private set; }

        public string Name { get; set; }
    }
}
