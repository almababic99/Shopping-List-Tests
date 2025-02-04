using API.DTOModels;

namespace Tests.Builders
{
    public class ShoppingListDTOBuilder
    {
        private int _id;
        private ShopperDTO? _shopper;
        private List<ShoppingListItemDTO> _items = new List<ShoppingListItemDTO>();

        public static ShoppingListDTOBuilder WithDefaults()
        {
            return new ShoppingListDTOBuilder()
                .WithId(1)
                .WithShopper(new ShopperDTO { Id = 1, Name = "John Doe" })
                .WithItems(new List<ShoppingListItemDTO>());
        }

        public ShoppingListDTOBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public ShoppingListDTOBuilder WithShopper(ShopperDTO shopper)
        {
            _shopper = shopper;
            return this;
        }

        public ShoppingListDTOBuilder WithItems(List<ShoppingListItemDTO> items)
        {
            _items = items ?? new List<ShoppingListItemDTO>();
            return this;
        }

        public ShoppingListDTO Build()
        {
            return new ShoppingListDTO
            {
                Id = _id,
                Shopper = _shopper,
                Items = _items
            };
        }

    }
}
