// Copyright (c) Pavol Kovalik.
// Licensed under the MIT License.

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace AddressBook.HttpTests
{
    public class ContactsTests : IClassFixture<AddressBookApplicationFactory>
    {
        private readonly HttpClient _client;

        public ContactsTests(AddressBookApplicationFactory factory)
        {
            _client = factory.CreateDefaultClient();
        }

        [Theory]
        [InlineData("GET", "/Contacts")]
        [InlineData("GET", "/Contacts/1")]
        [InlineData("DELETE", "/Contacts/1")]
        public async Task Contacts_route_should_return_status_200OK(string method, string route)
        {
            HttpRequestMessage request = new() { Method = new HttpMethod(method), RequestUri = new Uri(route, UriKind.Relative) };
            HttpResponseMessage response = await _client.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("GET", "/Contacts/0")]
        [InlineData("DELETE", "/Contacts/0")]
        public async Task Contacts_route_should_return_status_404NotFound(string method, string route)
        {
            HttpRequestMessage request = new() { Method = new HttpMethod(method), RequestUri = new Uri(route, UriKind.Relative) };
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
