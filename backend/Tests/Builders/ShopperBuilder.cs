using Domain.DomainModels;

namespace Tests.Builders
{
    public class ShopperBuilder
    {
        private int _id;
        private string _name;

        public static ShopperBuilder WithDefaults()
        {
            return new ShopperBuilder()
                .WithId(1)
                .WithName("Shopper");
        }

        public ShopperBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public ShopperBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public Shopper Build()
        {
            return new Shopper
            {
                Id = _id,
                Name = _name
            };
        }
    }
}
