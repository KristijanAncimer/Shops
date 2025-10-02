using FluentAssertions;
using Shops.Application.Common;
using Shops.Application.Handlers.Shops.Commands.CreateShop;
using Shops.Application.Handlers.Shops.Commands.UpdateShop;
using Shops.Tests.IntegrationTests.Helper;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Shops.Tests.IntegrationTests;

public class UpdateShopIntegrationTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;
    public UpdateShopIntegrationTests(TestApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
    [Fact]
    public async Task UpdateShop_ShouldReturn200_WhenExists()
    {
        // Arrange
        var createRequest = new { Name = "OriginalName" };
        var createResponse = await _client.PostAsJsonAsync("/shop", createRequest);
        var createdResult = await createResponse.Content.ReadFromJsonAsync<Result<CreateShopDto>>();
        var created = createdResult!.Data;

        var updateRequest = new { Name = "NewName" };

        // Act
        var updateResponse = await _client.PutAsJsonAsync($"/shop/{created!.Id}", updateRequest);

        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var updateResult = await updateResponse.Content.ReadFromJsonAsync<Result<UpdateShopHandlerDto>>();

        updateResult.Should().NotBeNull();
        updateResult!.IsSuccess.Should().BeTrue();
        updateResult.Data!.Id.Should().Be(created.Id);
        updateResult.Data!.Name.Should().Be("NewName");
    }

    [Fact]
    public async Task UpdateShop_ShouldReturn400_WhenIdIsInvalid()
    {
        // Arrange
        var updateRequest = new { Name = "ValidName" };

        // Act
        var response = await _client.PutAsJsonAsync($"/shop/{-5}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadAsStringAsync();
        error.Should().Contain("Shop ID must be greater than 0");
    }
    [Fact]
    public async Task UpdateShop_ShouldReturn400_WhenNameIsWhitespace()
    {
        // Arrange
        var createRequest = new { Name = "InitialShop" };
        var createResponse = await _client.PostAsJsonAsync("/shop", createRequest);
        var createdResult = await createResponse.Content.ReadFromJsonAsync<Result<CreateShopDto>>();
        var created = createdResult!.Data;

        var updateRequest = new { Name = "     " };

        // Act
        var response = await _client.PutAsJsonAsync($"/shop/{created!.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadAsStringAsync();
        error.Should().Contain("must not contain only whitespace");
    }
}
