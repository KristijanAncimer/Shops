using Core.Middlewares.Exceptions;
using FluentValidation;
using MediatR;
using MockQueryable.Moq;
using Moq;
using Shops.Application.Handlers.Shops.Commands.DeleteShop;
using Shops.Domain.Models;
using Shops.Infrastructure.Persistance;

namespace Shops.Tests.UnitTests;

public class DeleteShopTests
{
    [Fact]
    public async Task Handle_ShouldRemoveShop_WhenExists()
    {
        // Arrange
        var existing = new Shop { Id = 1, Name = "ToDelete" };
        var shops = new List<Shop> { existing };

        var mockSet = shops.AsQueryable().BuildMockDbSet();
        var mockContext = new Mock<IAppDbContext>();
        mockContext.Setup(c => c.Shops).Returns(mockSet.Object);

        var handler = new DeleteShopHandler(mockContext.Object);
        var request = new DeleteShopHandlerRequest(1);

        // Act
        var result = await handler.Handle(request, default);

        // Assert
        Assert.Equal(Unit.Value, result);
        mockSet.Verify(s => s.Remove(It.Is<Shop>(x => x.Id == 1)), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenNotFound()
    {
        // Arrange
        var shops = new List<Shop>();
        var mockSet = shops.AsQueryable().BuildMockDbSet();

        var mockContext = new Mock<IAppDbContext>();
        mockContext.Setup(c => c.Shops).Returns(mockSet.Object);

        var handler = new DeleteShopHandler(mockContext.Object);
        var request = new DeleteShopHandlerRequest(99);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(request, default));

        Assert.Equal("Shop with id 99 was not found.", ex.Message);
    }
}
