using Core.Middlewares.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
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
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);

        var mockCache = new Mock<IDistributedCache>();

        var handler = new DeleteShopHandler(mockContext.Object, mockCache.Object);
        var request = new DeleteShopHandlerRequest(1);

        // Act
        var result = await handler.Handle(request, default);

        // Assert
        Assert.Equal(Unit.Value, result);
        mockSet.Verify(s => s.Remove(It.Is<Shop>(x => x.Id == 1)), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }
}
