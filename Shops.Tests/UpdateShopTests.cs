using Microsoft.EntityFrameworkCore;
using MockQueryable;
using MockQueryable.Moq;
using Moq;
using Shops.Application.Handlers.Shops.Commands.UpdateShop;
using Shops.Domain.Models;
using Shops.Infrastructure.Persistance;

namespace Shops.Tests;

public class UpdateShopTests
{
    [Fact]
    public async Task Handle_ShouldUpdateName_WhenShopExists()
    {
        // Arrange
        var existing = new Shop { Id = 1, Name = "Old" };
        var shops = new List<Shop> { existing };

        var mockSet = shops.BuildMock().BuildMockDbSet();

        var mockContext = new Mock<IAppDbContext>();
        mockContext.Setup(c => c.Shops).Returns(mockSet.Object);

        var handler = new UpdateShopHandler(mockContext.Object);
        var request = new UpdateShopHandlerRequest(1, "New");

        // Act
        var result = await handler.Handle(request, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("New", result.Data!.Name);
        mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNotFound()
    {
        // Arrange
        var shops = new List<Shop>();
        var mockSet = shops.AsQueryable().BuildMockDbSet();

        var mockContext = new Mock<IAppDbContext>();
        mockContext.Setup(c => c.Shops).Returns(mockSet.Object);

        var handler = new UpdateShopHandler(mockContext.Object);
        var request = new UpdateShopHandlerRequest(99, "DoesNotExist");

        // Act
        var result = await handler.Handle(request, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"Shop with id 99 does not exist.", result.Error);
        mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
    }
}
