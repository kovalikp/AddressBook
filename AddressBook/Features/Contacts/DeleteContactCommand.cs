// Copyright (c) Pavol Kovalik.
// Licensed under the MIT License.

using System.Threading;
using System.Threading.Tasks;
using AddressBook.Data;
using AddressBook.Models;
using MediatR;

namespace AddressBook.Features.Contacts
{
    public class DeleteContactCommand : IRequest<bool>
    {
        public DeleteContactCommand(int id)
        {
            Id = id;
        }

        public int Id { get; }

        public class Handler : IRequestHandler<DeleteContactCommand, bool>
        {
            private readonly AddressBookContext _db;

            public Handler(AddressBookContext db)
            {
                _db = db;
            }

            public async Task<bool> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
            {
                Contact? contact = await _db.Contacts.FindAsync(new object[] { request.Id }, cancellationToken);
                if (contact is null)
                    return false;

                _db.Contacts.Remove(contact);
                await _db.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
