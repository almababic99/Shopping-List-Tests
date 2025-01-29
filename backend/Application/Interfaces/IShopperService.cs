using Domain.DomainModels;

namespace Application.Interfaces
{
    public interface IShopperService  // An interface defines the signature (method names, parameters, return types) of methods, while the class that implements the interface provides the specific implementation of those method (ShopperService)
    {
        Task<IEnumerable<Shopper>> GetShoppers();  // asynchronous method that returns a task containing an enumerable collection of Shopper objects

        Task<Shopper> GetShopperById(int id);   // get shopper by id

        Task AddShopper(Shopper shopper);  // add shopper to database

        Task DeleteShopper(int id);  // delete shopper from database

        Task EditShopper(Shopper shopper);   // edit shopper from database
    }
}
