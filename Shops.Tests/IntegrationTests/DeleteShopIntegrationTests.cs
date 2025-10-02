using FluentAssertions;
using Shops.Application.Common;
using Shops.Application.Handlers.Shops.Commands.CreateShop;
using Shops.Application.Handlers.Shops.Queries.GetShopById;
using Shops.Tests.IntegrationTests.Helper;
using System.Net;
using System.Net.Http.Json;

namespace Shops.Tests.IntegrationTests;

public class DeleteShopIntegrationTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;
    public DeleteShopIntegrationTests(TestApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
    [Fact]
    public async Task DeleteShop_ShouldReturn204_WhenExists()
    {
        // Arrange
        var createRequest = new { Name = "DeleteMe" };
        var createResponse = await _client.PostAsJsonAsync("/shop", createRequest);
        var createdResult = await createResponse.Content.ReadFromJsonAsync<Result<CreateShopDto>>();
        var created = createdResult!.Data;

        // Act
        var response = await _client.DeleteAsync($"/shop/{created!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/shop/{created.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResult = await getResponse.Content.ReadFromJsonAsync<Result<GetShopByIdHandlerDto>>();
        getResult!.IsSuccess.Should().BeFalse();
        getResult.Error.Should().Contain($"Shop with id {created.Id} was not found.");
    }

    [Fact]
    public async Task DeleteShop_ShouldReturn400_WhenIdIsInvalid()
    {
        // Act
        var response = await _client.DeleteAsync($"/shop/{-1}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadAsStringAsync();
        error.Should().Contain("Shop ID must be greater than 0");
    }
}
