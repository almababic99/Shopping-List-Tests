using Domain.DomainModels;
using Infrastructure.EntityModels;

namespace Infrastructure.Mappers
{
    public static class ShoppingListMapperDomainToEntity
    {
        public static ShoppingListEntity MapToEntity(ShoppingList shoppingList)  // This method maps ShoppingList domain to ShoppingListEntity
        {
            return new ShoppingListEntity
            {
                Id = shoppingList.Id,
                ShopperId = shoppingList.ShopperId,
                Items = shoppingList.Items.Select(item => new ShoppingListItemEntity
                {
                    Id = item.Id,
                    ShoppingListId = item.ShoppingListId,
                    ItemId = item.ItemId,
                    ShoppingList = new ShoppingListEntity
                    {
                        Id = shoppingList.Id
                    }
                }).ToList()
            };
        }
    }
}
