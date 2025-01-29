using API.DTOModels;
using Domain.DomainModels;

namespace API.Mappers
{
    public static class ShopperMapperDomainToDTO
    {
        public static ShopperDTO MapToDTO(Shopper shopper)  // This method maps Shopper domain model to ShopperDTO
        {
            return new ShopperDTO
            {
                Id = shopper.Id,
                Name = shopper.Name
            };
        }
    }
}



