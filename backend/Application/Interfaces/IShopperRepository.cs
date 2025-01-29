using Domain.DomainModels;

namespace Application.Interfaces
{
    public interface IShopperRepository  // An interface defines the signature (method names, parameters, return types) of methods, while the class that implements the interface provides the specific implementation of those methods (ShopperRepository)
    {
        Task<IEnumerable<Shopper>> GetShoppers();  // get all shoppers

        Task<Shopper> GetShopperById(int id);  // get shopper by id

        Task<Shopper> GetShopper(string name);  // get shopper by name 

        Task AddShopper(Shopper shopper);  // add shopper to database

        Task DeleteShopper(int id);  // delete shopper from database

        Task EditShopper(Shopper shopper);   // edit shopper from database
    }
}
