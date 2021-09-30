// Copyright (c) Pavol Kovalik.
// Licensed under the MIT License.

using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using AddressBook.Data;
using AddressBook.Models;
using MediatR;

namespace AddressBook.Features.Contacts
{
    public class UpdateContactCommand : IRequest<ContactDto?>
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string? Name { get; set; }

        public class Handler : IRequestHandler<UpdateContactCommand, ContactDto?>
        {
            private readonly AddressBookContext _db;

            public Handler(AddressBookContext db)
            {
                _db = db;
            }

            public async Task<ContactDto?> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
            {
                Contact? contact = await _db.Contacts.FindAsync(new object[] { request.Id }, cancellationToken);
                if (contact is null)
                    return null;

                if (request.Name is string name)
                    contact.Name = name;

                await _db.SaveChangesAsync(cancellationToken);

                return new ContactDto(contact);
            }
        }
    }
}
