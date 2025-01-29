using API.DTOModels;

namespace Tests.Builders
{
    public class ShopperDTOBuilder
    {
        private int _id;
        private string _name;

        public static ShopperDTOBuilder WithDefaults()
        {
            return new ShopperDTOBuilder()
                .WithId(1)
                .WithName("ShopperDTO");
        }

        public ShopperDTOBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public ShopperDTOBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ShopperDTO Build()
        {
            return new ShopperDTO
            {
                Id = _id,
                Name = _name
            };
        }
    }
}
