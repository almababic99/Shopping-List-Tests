using Domain.DomainModels;

namespace Application.Interfaces
{
    public interface IItemService // An interface defines the signature (method names, parameters, return types) of methods, while the class that implements the interface provides the specific implementation of those method (ItemService)
    {
        Task<IEnumerable<Item>> GetItems();  // get all items

        Task<Item> GetItemById(int id);   // get item by id

        Task AddItem(Item item);  // add item to database

        Task DeleteItem(int id);  // delete item from database

        Task EditItem(Item item);   // edit item from database
    }
}
