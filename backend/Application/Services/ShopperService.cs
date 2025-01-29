using Application.Interfaces;
using Domain.DomainModels;

namespace Application.Services
{
    public class ShopperService : IShopperService  // ShopperService class implements the IShopperService interface which means it is expected to define the operations declared in IShopperService
    {
        private readonly IShopperRepository _shopperRepository;

        public ShopperService(IShopperRepository shopperRepository) // The constructor takes an instance of IShopperRepository as a parameter and assigns it to a private field _shopperRepository. This is a Dependency Injection
        {
            _shopperRepository = shopperRepository;
        }

        public async Task<IEnumerable<Shopper>> GetShoppers()  
        {
            return await _shopperRepository.GetShoppers();
        }

        public async Task<Shopper> GetShopperById(int id)
        {
            return await _shopperRepository.GetShopperById(id);
        }

        public async Task AddShopper(Shopper shopper)
        {
            var existingShopper = await _shopperRepository.GetShopper(shopper.Name);

            if (existingShopper != null)
            {
                throw new InvalidOperationException("Shopper with the same name already exists");  // if the name already exists it throws an error
            }

            await _shopperRepository.AddShopper(shopper);
        }

        public async Task DeleteShopper(int id)
        {
            await _shopperRepository.DeleteShopper(id);
        }

        public async Task EditShopper(Shopper shopper)
        {
            await _shopperRepository.EditShopper(shopper);
        }
    }
}


