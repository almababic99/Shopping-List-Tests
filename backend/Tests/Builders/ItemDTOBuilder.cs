using API.DTOModels;

namespace Tests.Builders
{
    public class ItemDTOBuilder
    {
        private int _id;
        private string _name;
        private int _quantity;

        public static ItemDTOBuilder WithDefaults()
        {
            return new ItemDTOBuilder()
                .WithId(1)
                .WithName("Item")
                .WithQuantity(0);
        }

        public ItemDTOBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public ItemDTOBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ItemDTOBuilder WithQuantity(int quantity)
        {
            _quantity = quantity;
            return this;
        }

        public ItemDTO Build()
        {
            return new ItemDTO
            {
                Id = _id,
                Name = _name,
                Quantity = _quantity
            };
        }
    }
}
