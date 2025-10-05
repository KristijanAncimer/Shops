using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using MockQueryable.Moq;
using Moq;
using Shops.Application.Handlers.Shops.Commands.UpdateShop;
using Shops.Domain.Models;
using Shops.Infrastructure.Persistance;

namespace Shops.Tests.UnitTests;

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
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<UpdateShopHandlerDto>(It.IsAny<Shop>()))
                  .Returns((Shop s) => new UpdateShopHandlerDto
                  {
                      Id = s.Id,
                      Name = s.Name,
                      CreatedAt = s.CreatedAt,
                      UpdatedAt = s.UpdatedAt
                  });

        var mockCache = new Mock<IDistributedCache>();

        var handler = new UpdateShopHandler(mockContext.Object, mockMapper.Object, mockCache.Object);
        var request = new UpdateShopHandlerRequest(1, "New");

        // Act
        var result = await handler.Handle(request, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("New", result.Data!.Name);
        mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }
}
