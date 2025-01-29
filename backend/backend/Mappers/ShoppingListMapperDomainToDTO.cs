using API.DTOModels;
using Application.Interfaces;
using Domain.DomainModels;

namespace API.Mappers
{
    public static class ShoppingListMapperDomainToDTO
    {
        public static async Task<ShoppingListDTO> MapToDTO(ShoppingList shoppingList, IShopperService shopperService, IItemService itemService)  // This method maps ShoppingList domain to ShoppingListDTO
        {
            // Fetch shopper details by shopperId
            var shopper = await shopperService.GetShopperById(shoppingList.ShopperId.GetValueOrDefault());

            // map dto shopping list to a DTO and fetch items one by one
            var shoppingListDTO = new ShoppingListDTO
            {
                Id = shoppingList.Id,
                Shopper = shopper != null ? new ShopperDTO
                {
                    Id = shopper.Id,
                    Name = shopper.Name
                } : null,

                // Mapping ShoppingListItems to ShoppingListItemDTOs
                Items = new List<ShoppingListItemDTO>()
            };

            // Iterate over each shopping list item, fetch item details and map them
            foreach (var shoppingListItem in shoppingList.Items)
            {
                var itemDetails = await itemService.GetItemById(shoppingListItem.ItemId);

                if (itemDetails != null)
                {
                    shoppingListDTO.Items.Add(new ShoppingListItemDTO
                    {
                        Id = shoppingListItem.Id,
                        Item = new ItemDTO
                        {
                            Id = itemDetails.Id,
                            Name = itemDetails.Name
                        }
                    });
                }
            }

            return shoppingListDTO;
        }
    }
}
