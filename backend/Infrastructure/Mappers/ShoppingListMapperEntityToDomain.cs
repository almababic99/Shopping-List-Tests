using Domain.DomainModels;
using Infrastructure.EntityModels;

namespace Infrastructure.Mappers
{
    public static class ShoppingListMapperEntityToDomain
    {
        public static ShoppingList MapToDomain(ShoppingListEntity shoppingListEntity)  // This method maps ShoppingListEntity to ShoppingList domain
        {
            return new ShoppingList
            {
                Id = shoppingListEntity.Id,
                ShopperId = shoppingListEntity.ShopperId,
                Items = shoppingListEntity.Items.Select(itemEntity => new ShoppingListItem
                {
                    Id = itemEntity.Id,
                    ShoppingListId = itemEntity.ShoppingListId,
                    ItemId = itemEntity.ItemId,
                }).ToList()
            };
        }
    }
}
