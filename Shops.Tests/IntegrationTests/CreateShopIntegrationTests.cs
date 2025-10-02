using FluentAssertions;
using Shops.Application.Common;
using Shops.Application.Handlers.Shops.Commands.CreateShop;
using Shops.Application.Handlers.Shops.Queries.GetShopById;
using Shops.Tests.IntegrationTests.Helper;
using System.Net;
using System.Net.Http.Json;

namespace Shops.Tests.IntegrationTests;

public class CreateShopIntegrationTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;
    public CreateShopIntegrationTests(TestApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
    [Fact]
    public async Task GetShopById_ShouldReturn200_WhenExists()
    {
        // Arrange
        var createRequest = new { Name = "SomeRandomShop" };
        var createResponse = await _client.PostAsJsonAsync("/shop", createRequest);

        var createdResult = await createResponse.Content.ReadFromJsonAsync<Result<CreateShopDto>>();
        var created = createdResult!.Data;

        // Act
        var response = await _client.GetAsync($"/shop/{created!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var shopResult = await response.Content.ReadFromJsonAsync<Result<GetShopByIdHandlerDto>>();
        shopResult.Should().NotBeNull();
        shopResult!.IsSuccess.Should().BeTrue();

        var shop = shopResult.Data;
        shop.Should().NotBeNull();
        shop!.Name.Should().Be("SomeRandomShop");
    }
    [Fact]
    public async Task CreateShop_ShouldReturn400_WhenNameIsInvalid()
    {
        // Arrange
        var invalidRequest = new { Name = "        " };

        // Act
        var response = await _client.PostAsJsonAsync("/shop", invalidRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadAsStringAsync();
        error.Should().Contain("Name of the shop");
    }
}
