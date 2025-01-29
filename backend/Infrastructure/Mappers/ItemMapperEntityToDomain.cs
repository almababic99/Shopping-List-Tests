using Domain.DomainModels;
using Infrastructure.Models;

namespace Infrastructure.Mappers
{
    public static class ItemMapperEntityToDomain
    {
        public static Item MapToDomain(ItemEntity item)  // This method maps ItemEntity model to Item domain model
        {
            return new Item
            {
                Id = item.Id,
                Name = item.Name,
                Quantity = item.Quantity,
            };
        }
    }
}
