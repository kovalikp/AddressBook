// Copyright (c) Pavol Kovalik.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AddressBook.Data;
using AddressBook.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Features.Contacts
{
    public class ListContactsQuery : IRequest<ContactsDto>
    {
        public class Handler : IRequestHandler<ListContactsQuery, ContactsDto>
        {
            private readonly AddressBookContext _db;

            public Handler(AddressBookContext db)
            {
                _db = db;
            }

            public async Task<ContactsDto> Handle(ListContactsQuery request, CancellationToken cancellationToken)
            {
                List<Contact> contacts = await _db.Contacts.ToListAsync(cancellationToken);
                return new ContactsDto(contacts.Select(x => new ContactDto(x)).ToList());
            }
        }
    }
}
