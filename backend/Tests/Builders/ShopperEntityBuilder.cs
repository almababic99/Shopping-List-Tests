using Infrastructure.Models;

namespace Tests.Builders
{
    public class ShopperEntityBuilder
    {
        private int _id;
        private string _name;

        public static ShopperEntityBuilder WithDefaults()
        {
            return new ShopperEntityBuilder()
                .WithId(1)
                .WithName("ShopperEntity");
        }

        public ShopperEntityBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public ShopperEntityBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ShopperEntity Build()
        {
            return new ShopperEntity
            {
                Id = _id,
                Name = _name
            };
        }
    }
}
