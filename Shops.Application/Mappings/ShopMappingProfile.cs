using AutoMapper;
using Shops.Application.Handlers.Shops.Commands.CreateShop;
using Shops.Application.Handlers.Shops.Commands.UpdateShop;
using Shops.Application.Handlers.Shops.Queries.GetShopById;
using Shops.Application.Handlers.Shops.Queries.GetShops;
using Shops.Domain.Models;

namespace Shops.Application.Mappings;

public class ShopMappingProfile : Profile
{
    public ShopMappingProfile()
    {
        CreateMap<Shop, UpdateShopHandlerDto>();
        CreateMap<Shop, CreateShopDto>();
        CreateMap<Shop, GetShopByIdHandlerDto>();
        CreateMap<Shop, GetShopsHandlerDto>();
    }
}
