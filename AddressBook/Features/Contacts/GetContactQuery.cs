// Copyright (c) Pavol Kovalik.
// Licensed under the MIT License.

using System.Threading;
using System.Threading.Tasks;
using AddressBook.Data;
using AddressBook.Models;
using MediatR;

namespace AddressBook.Features.Contacts
{
    public class GetContactQuery : IRequest<ContactDto?>
    {
        public GetContactQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }

        public class Handler : IRequestHandler<GetContactQuery, ContactDto?>
        {
            private readonly AddressBookContext _db;

            public Handler(AddressBookContext db)
            {
                _db = db;
            }

            public async Task<ContactDto?> Handle(GetContactQuery request, CancellationToken cancellationToken)
            {
                Contact? contact = await _db.Contacts.FindAsync(new object[] { request.Id }, cancellationToken);
                return contact is null ? null : new ContactDto(contact);
            }
        }
    }
}
