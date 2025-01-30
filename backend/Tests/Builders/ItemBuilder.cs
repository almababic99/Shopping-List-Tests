using Domain.DomainModels;

namespace Tests.Builders
{
    public class ItemBuilder
    {
        private int _id;
        private string _name;
        private int _quantity;

        public static ItemBuilder WithDefaults()
        {
            return new ItemBuilder()
                .WithId(1)
                .WithName("Item")
                .WithQuantity(0);
        }

        public ItemBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public ItemBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ItemBuilder WithQuantity(int quantity)
        {
            _quantity = quantity;
            return this;
        }

        public Item Build()
        {
            return new Item
            {
                Id = _id,
                Name = _name,
                Quantity = _quantity
            };
        }
    }
}
