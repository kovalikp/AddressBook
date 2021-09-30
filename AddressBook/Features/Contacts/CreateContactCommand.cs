// Copyright (c) Pavol Kovalik.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using AddressBook.Data;
using AddressBook.Models;
using MediatR;

namespace AddressBook.Features.Contacts
{
    public class CreateContactCommand : IRequest<ContactDto>
    {
        public string? Name { get; set; }

        public class Handler : IRequestHandler<CreateContactCommand, ContactDto>
        {
            private readonly AddressBookContext _db;

            public Handler(AddressBookContext db)
            {
                _db = db;
            }

            public async Task<ContactDto> Handle(CreateContactCommand request, CancellationToken cancellationToken)
            {
                Contact contact = new(
                    name: request.Name ?? throw new ArgumentException("Invalid request.", nameof(request)));

                await _db.Contacts.AddAsync(contact, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);

                return new ContactDto(contact);
            }
        }
    }
}
