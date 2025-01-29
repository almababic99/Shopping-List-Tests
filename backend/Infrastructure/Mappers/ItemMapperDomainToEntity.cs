using Domain.DomainModels;
using Infrastructure.Models;

namespace Infrastructure.Mappers
{
    public static class ItemMapperDomainToEntity
    {
        public static ItemEntity MapToEntity(Item item)  // This method maps Item domain model to ItemEntity
        {
            return new ItemEntity
            {
                Id = item.Id,
                Name = item.Name,
                Quantity = item.Quantity,
            };
        }
    }
}
