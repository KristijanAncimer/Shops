using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using Shops.Application.Handlers.Shops.Commands.CreateShop;
using Shops.Domain.Models;
using Shops.Infrastructure.Persistance;

namespace Shops.Tests.UnitTests;

public class CreateShopTests
{
    [Fact]
    public async Task Handle_ShouldAddNewShop_AndReturnId()
    {
        // Arrange
        var shops = new List<Shop>();
        var mockSet = shops.AsQueryable().BuildMockDbSet();

        var mockContext = new Mock<IAppDbContext>();
        var mockLogger = new Mock<ILogger<CreateShopHandler>>();
        mockContext.Setup(c => c.Shops).Returns(mockSet.Object);

        var handler = new CreateShopHandler(mockContext.Object, mockLogger.Object);
        var request = new CreateShopHandlerRequest("Test Shop");

        // Act
        var result = await handler.Handle(request, default);

        // Assert
        mockSet.Verify(s => s.AddAsync(It.IsAny<Shop>(), default), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

        Assert.True(result.IsSuccess);
        Assert.Equal("Test Shop", result?.Data?.Name);
    }
}
