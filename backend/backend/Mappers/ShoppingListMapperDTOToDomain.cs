using API.DTOModels;
using Application.Interfaces;
using Domain.DomainModels;

namespace API.Mappers
{
    public static class ShoppingListMapperDTOToDomain
    {
        public static async Task<ShoppingList> MapToDomain(ShoppingListDTO shoppingListDTO, IShopperService shopperService, IItemService itemService)  // This method maps ShoppingListDTO to ShoppingList domain
        {
            // map ShoppingListDTO to ShoppingList domain
            var shoppingList = new ShoppingList
            {
                Id = shoppingListDTO.Id,
                ShopperId = shoppingListDTO.Shopper?.Id,
                Items = new List<ShoppingListItem>()
            };

            // map each ShoppingListItemDTO to ShoppingListITem domain
            foreach (var shoppingListItemDTO in shoppingListDTO.Items)
            {
                if (shoppingListItemDTO?.Item?.Id != null)
                {
                    var item = await itemService.GetItemById(shoppingListItemDTO.Item.Id);
                    if (item != null)
                    {
                        shoppingList.Items.Add(new ShoppingListItem
                        {
                            ItemId = item.Id,
                            ShoppingListId = shoppingList.Id
                        });
                    }
                }
            }

            return shoppingList;
        }
    }
}
