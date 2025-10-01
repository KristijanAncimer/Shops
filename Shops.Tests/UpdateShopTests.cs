using Moq;
using Shops.Application.Handlers.Shops.Commands.UpdateShop;
using Shops.Domain.Models;
using Shops.Infrastructure.Persistance;
using Shops.Tests.Common;

namespace Shops.Tests;

public class UpdateShopTests
{
    [Fact]
    public async Task Handle_ShouldUpdateName_WhenShopExists()
    {
        // Arrange
        var existing = new Shop { Id = 1, Name = "Old" };
        var mockSet = DbSetMocking.CreateMockDbSet(new[] { existing });

        var mockContext = new Mock<IAppDbContext>();
        mockContext.Setup(c => c.Shops).Returns(mockSet.Object);

        var handler = new UpdateShopHandler(mockContext.Object);
        var request = new UpdateShopHandlerRequest(1, "New");

        // Act
        var result = await handler.Handle(request, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("New", result.Data!.Name);
        Assert.Equal("New", existing.Name);
        mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNotFound()
    {
        // Arrange
        var mockSet = DbSetMocking.CreateMockDbSet(new List<Shop>());
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
