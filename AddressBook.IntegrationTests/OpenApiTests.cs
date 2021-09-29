using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace AddressBook
{
    public class OpenApiTests : IClassFixture<AddressBookApplicationFactory>
    {
        private readonly HttpClient _client;

        public OpenApiTests(AddressBookApplicationFactory factory)
        {
            _client = factory.CreateDefaultClient();
        }

        [Fact]
        public async Task OpenApiEndpointShouldReturnOO()
        {
            HttpResponseMessage response = await _client.GetAsync("/swagger/v1/swagger.json");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}