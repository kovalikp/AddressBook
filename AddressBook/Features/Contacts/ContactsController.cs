// Copyright (c) Pavol Kovalik.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Features.Contacts
{
    [Route("[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        public const string GetRoute = "GetContact";

        private readonly IMediator _mediator;

        public ContactsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        [HttpGet()]
        public async Task<ActionResult<ContactsDto>> List(CancellationToken cancellationToken)
        {
            ContactsDto result = await _mediator.Send(new ListContactsQuery());
            return result;
        }


        [HttpGet("{id}", Name = GetRoute)]
        public async Task<ActionResult<ContactDto>> Get(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetContactQuery(id), cancellationToken);

            return result is not null ? result : NotFound();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ContactDto>> Update(int id, UpdateContactCommand request, CancellationToken cancellationToken)
        {
            request.Id = id;
            ContactDto? result = await _mediator.Send(request, cancellationToken);

            return result is not null ? result : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ContactDto>> Delete(int id, CancellationToken cancellationToken)
        {
            bool result = await _mediator.Send(new DeleteContactCommand(id), cancellationToken);

            return result ? Ok() : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<ContactDto>> Create(CreateContactCommand request, CancellationToken cancellationToken)
        {
            ContactDto result = await _mediator.Send(request, cancellationToken);
            return CreatedAtRoute(GetRoute, new { id = result.Id }, result);
        }
    }
}
