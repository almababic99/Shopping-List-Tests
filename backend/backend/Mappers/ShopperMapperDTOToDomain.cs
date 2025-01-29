using API.DTOModels;
using Domain.DomainModels;

namespace API.Mappers
{
    public static class ShopperMapperDTOToDomain
    {
        public static Shopper MapToDomain(ShopperDTO shopperDTO)  // This method maps ShopperDTO to Shopper domain model
        {
            return new Shopper
            {
                Id = shopperDTO.Id,
                Name = shopperDTO.Name
            };
        }
    }
}
