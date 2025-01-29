using Application.Interfaces;

namespace Application.Jobs
{
    public class ItemQuantityUpdaterJob    // Recurring job (triggered from Program.cs to run every minute)
    {
        private readonly IItemRepository _itemRepository;
        private readonly IShoppingListRepository _shoppingListRepository;

        public ItemQuantityUpdaterJob(IItemRepository itemRepository, IShoppingListRepository shoppingListRepository)
        {
            _itemRepository = itemRepository;
            _shoppingListRepository = shoppingListRepository;
        }
        public async Task Execute()
        {
            var items = await _itemRepository.GetItems(); // Get all items

            foreach (var item in items)
            {
                var count = await _shoppingListRepository.getCountOfItemInShoppingList(item.Id);  // Get count of item in shopping lists
                item.Quantity = count; // Update quantity
                await _itemRepository.EditItem(item); // Save item to database
            }
        }
    }
}
