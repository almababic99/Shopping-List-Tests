using Application.Interfaces;
using Domain.DomainModels;

namespace Application.Services
{
    public class ShoppingListService : IShoppingListService  // ShoppingListService class implements the IShoppingListService interface which means it is expected to define the operations declared in IShoppingListService
    {
        private readonly IShoppingListRepository _shoppingListRepository;

        public ShoppingListService(IShoppingListRepository shoppingListRepository) // The constructor takes an instance of IShoppingListRepository as a parameter and assigns it to a private field _shoppingListRepository. This is a Dependency Injection
        {
            _shoppingListRepository = shoppingListRepository;
        }

        public async Task<IEnumerable<ShoppingList>> GetShoppingLists()  
        {
            return await _shoppingListRepository.GetShoppingLists();
        }

        public async Task<IEnumerable<ShoppingList>> GetShoppingListsByShopperId(int shopperId)
        {
            return await _shoppingListRepository.GetShoppingListsByShopperId(shopperId);
        }

        public async Task DeleteShoppingList(int id)
        {
            await _shoppingListRepository.DeleteShoppingList(id);
        }

        public async Task AddShoppingList(ShoppingList shoppingList)
        {
            await _shoppingListRepository.AddShoppingList(shoppingList);
        }

        public async Task<int> getCountOfItemInShoppingList(int itemId)
        {
            return await _shoppingListRepository.getCountOfItemInShoppingList(itemId);
        }
    }
}
