using Infrastructure.Models;

namespace Tests.Builders
{
    public class ItemEntityBuilder
    {
        private int _id;
        private string _name;
        private int _quantity;

        public static ItemEntityBuilder WithDefaults()
        {
            return new ItemEntityBuilder()
                .WithId(1)
                .WithName("Item")
                .WithQuantity(0);
        }

        public ItemEntityBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public ItemEntityBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ItemEntityBuilder WithQuantity(int quantity)
        {
            _quantity = quantity;
            return this;
        }

        public ItemEntity Build()
        {
            return new ItemEntity
            {
                Id = _id,
                Name = _name,
                Quantity = _quantity
            };
        }
    }
}
