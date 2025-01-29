using API.DTOModels;
using Domain.DomainModels;

namespace API.Mappers
{
    public static class ItemMapperDTOToDomain
    {
        public static Item MapToDomain(ItemDTO itemDTO)  // This method maps ItemDTO to Item domain model
        {
            return new Item
            {
                Id = itemDTO.Id,
                Name = itemDTO.Name,
                Quantity = itemDTO.Quantity,
            };
        }
    }
}
