using Infrastructure.EntityModels;
using Infrastructure.Models;

namespace Tests.Builders
{
    public class ShoppingListEntityBuilder
    {
        private int _id;
        private int? _shopperId;
        private ShopperEntity? _shopper;
        private List<ShoppingListItemEntity> _items = new List<ShoppingListItemEntity>();

        public static ShoppingListEntityBuilder WithDefaults()
        {
            return new ShoppingListEntityBuilder()
                .WithId(1)
                .WithShopperId(1)
                .WithShopper(new ShopperEntity { Id = 1, Name = "John Doe" })
                .WithItems(new List<ShoppingListItemEntity>());
        }

        public ShoppingListEntityBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public ShoppingListEntityBuilder WithShopperId(int? shopperId)
        {
            _shopperId = shopperId;
            return this;
        }

        public ShoppingListEntityBuilder WithShopper(ShopperEntity shopper)
        {
            _shopper = shopper;
            return this;
        }

        public ShoppingListEntityBuilder WithItems(List<ShoppingListItemEntity> items)
        {
            _items = items ?? new List<ShoppingListItemEntity>();
            return this;
        }

        public ShoppingListEntity Build()
        {
            return new ShoppingListEntity
            {
                Id = _id,
                ShopperId = _shopperId,
                Shopper = _shopper,
                Items = _items
            };
        }
    }
}
