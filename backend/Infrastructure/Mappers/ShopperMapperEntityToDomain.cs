using Domain.DomainModels;
using Infrastructure.Models;

namespace Infrastructure.Mappers
{
    public static class ShopperMapperEntityToDomain
    {
        public static Shopper MapToDomain(ShopperEntity shopper)  // This method maps ShopperEntity to Shopper domain model
        {
            return new Shopper
            {
                Id = shopper.Id,
                Name = shopper.Name
            };
        }
    }
}
