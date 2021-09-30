// Copyright (c) Pavol Kovalik.
// Licensed under the MIT License.

using AddressBook.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Data
{
    public class AddressBookContext : DbContext
    {
        public AddressBookContext(DbContextOptions<AddressBookContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts => Set<Contact>();
    }
}
