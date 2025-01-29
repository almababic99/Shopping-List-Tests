using Domain.DomainModels;

namespace Application.Interfaces
{
    public interface IShoppingListRepository  // An interface defines the signature (method names, parameters, return types) of methods, while the class that implements the interface provides the specific implementation of those methods (ShoppingListRepository)
    {
        Task<IEnumerable<ShoppingList>> GetShoppingLists();  

        Task<IEnumerable<ShoppingList>> GetShoppingListsByShopperId(int shopperId); 

        Task DeleteShoppingList(int id);

        Task AddShoppingList(ShoppingList shoppingList);

        Task<int> getCountOfItemInShoppingList(int itemId);
    }
}
