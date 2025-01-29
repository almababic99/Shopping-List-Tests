using Application.Interfaces;
using Domain.DomainModels;

namespace Application.Services
{
    public class ItemService : IItemService  // ItemService class implements the IItemService interface which means it is expected to define the operations declared in IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository) // The constructor takes an instance of IItemRepository as a parameter and assigns it to a private field _itemRepository. This is a Dependency Injection
        {
            _itemRepository = itemRepository;
        }

        public async Task<IEnumerable<Item>> GetItems()  
        {
            return await _itemRepository.GetItems();
        }

        public async Task<Item> GetItemById(int id)
        {
            return await _itemRepository.GetItemById(id);
        }

        public async Task AddItem(Item item)
        {
            var existingItem = await _itemRepository.GetItem(item.Name);

            if (existingItem != null)
            {
                throw new InvalidOperationException("An item with the same name already exists");  // if the name already exists it throws an error
            }

            await _itemRepository.AddItem(item);  
        }

        public async Task DeleteItem(int id)
        {
            await _itemRepository.DeleteItem(id);
        }

        public async Task EditItem(Item item)
        {
            await _itemRepository.EditItem(item);
        }
    }
}
