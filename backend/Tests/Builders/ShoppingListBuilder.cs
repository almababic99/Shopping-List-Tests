using Domain.DomainModels;

namespace Tests.Builders
{
    public class ShoppingListBuilder
    {
        private int _id;
        private int? _shopperId;
        private List<ShoppingListItem> _items = new List<ShoppingListItem>();

        public static ShoppingListBuilder WithDefaults()
        {
            return new ShoppingListBuilder()
                .WithId(1)
                .WithShopperId(1)
                .WithItems(new List<ShoppingListItem>());
        }

        public ShoppingListBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public ShoppingListBuilder WithShopperId(int? shopperId)
        {
            _shopperId = shopperId;
            return this;
        }

        public ShoppingListBuilder WithItems(List<ShoppingListItem> items)
        {
            _items = items ?? new List<ShoppingListItem>();
            return this;
        }

        public ShoppingList Build()
        {
            return new ShoppingList
            {
                Id = _id,
                ShopperId = _shopperId,
                Items = _items
            };
        }
    }
}
