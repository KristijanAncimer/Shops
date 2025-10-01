using MediatR;
using Moq;
using Shops.Application.Handlers.Shops.Commands.CreateShop;
using Shops.Domain.Models;
using Shops.Infrastructure.Persistance;
using Shops.Tests.Common;

namespace Shops.Tests;

public class CreateShopTests
{
    [Fact]
    public async Task Handle_ShouldAddNewShop_AndReturnId()
    {
        // Arrange
        var shops = new List<Shop>();
        var mockSet = DbSetMocking.CreateMockDbSet(shops);

        var mockContext = new Mock<IAppDbContext>();
        mockContext.Setup(c => c.Shops).Returns(mockSet.Object);

        var handler = new CreateShopHandler(mockContext.Object);
        var request = new CreateShopHandlerRequest("Test Shop");

        // Act
        var result = await handler.Handle(request, default);

        // Assert
        mockSet.Verify(s => s.Add(It.IsAny<Shop>()), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

        Assert.NotNull(result);
        Assert.Equal("Test Shop", result.Name);
        Assert.NotEqual(1, result.Id);
    }
}
