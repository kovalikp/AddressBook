// Copyright (c) Pavol Kovalik.
// Licensed under the MIT License.

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AddressBook.Models;
using FluentAssertions;
using Xunit;
using static AddressBook.DatabaseFixture;

namespace AddressBook.HttpTests
{
    public class ContactsTests : IntegrationTestBase, IClassFixture<AddressBookApplicationFactory>
    {
        private readonly HttpClient _client;

        public ContactsTests(AddressBookApplicationFactory factory)
        {
            _client = factory.CreateDefaultClient();
        }

        [Fact]
        public async Task List_contacts_returns_200OK()
        {
            HttpResponseMessage response = await _client.GetAsync($"/Contacts");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_contact_returns_200OK()
        {
            var contact = new Contact("Test");
            await InsertAsync(contact);
            HttpResponseMessage response = await _client.GetAsync($"/Contacts/{contact.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_contact_returns_200OK()
        {
            var contact = new Contact("Test");
            await InsertAsync(contact);
            HttpResponseMessage response = await _client.DeleteAsync($"/Contacts/{contact.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Update_contact_resturns_200OK()
        {
            var contact = new Contact("Test");
            await InsertAsync(contact);
            HttpResponseMessage response = await _client.PatchAsJsonAsync($"/Contacts/{contact.Id}", new { name = "test" });
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("GET", "/Contacts/0")]
        [InlineData("DELETE", "/Contacts/0")]
        [InlineData("PATCH", "/Contacts/0", "{}")]
        public async Task Invalid_contact_id_route_should_return_status_404NotFound(string method, string route, string? content = null)
        {
            HttpRequestMessage request = new() { Method = new HttpMethod(method), RequestUri = new Uri(route, UriKind.Relative) };
            if (content is not null)
                request.Content = new JsonStringContent(content);

            HttpResponseMessage response = await _client.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_contact_should_return_status_201Created()
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("/Contacts", new { name = "test" });
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}
