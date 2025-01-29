using Application.Interfaces;
using Domain.DomainModels;
using Infrastructure.Data;
using Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository // ItemRepository class implements IItemRepository interface which means it is expected to define the operations declared in IItemRepository
    {
        private readonly ShoppingListDbContext _shoppingListDbContext;
        public ItemRepository(ShoppingListDbContext shoppingListDbContext)  // The constructor takes an instance of ShoppingListDbContext as a parameter and assigns it to a private field _shoppingListDbContext. This is a Dependency Injection
        {
            _shoppingListDbContext = shoppingListDbContext;
        }

        public async Task<IEnumerable<Item>> GetItems()  // get all items
        {
            var items = await _shoppingListDbContext.Items.ToListAsync();  // Get all Item entities from the DB
            return items.Select(item => ItemMapperEntityToDomain.MapToDomain(item));  // Map each Item entity to ItemDomain and return as an IEnumerable<Item>
        }

        public async Task<Item?> GetItem(string name)  // get item by name
        {
            var itemEntity = await _shoppingListDbContext.Items.FirstOrDefaultAsync(i => i.Name.ToLower() == name.ToLower());
            return itemEntity != null ? ItemMapperEntityToDomain.MapToDomain(itemEntity) : null;
            // if itemEntity is not null method ItemMapperEntityToDomain.MapToDomain(itemEntity) is called to map entity model to domain model
            // if itemEntity is null it returns null
        }

        public async Task<Item?> GetItemById(int id)  // get item by id
        {
            var itemEntity = await _shoppingListDbContext.Items.FirstOrDefaultAsync(i =>i.Id == id);
            return itemEntity != null ? ItemMapperEntityToDomain.MapToDomain(itemEntity) : null;
            // if itemEntity is not null method ItemMapperEntityToDomain.MapToDomain(itemEntity) is called to map entity model to domain model
            // if itemEntity is null it returns null
        }

        public async Task AddItem(Item item)  // add item to database
        {
            // if the name of an item is not already in database we can add it:
            var itemEntity = ItemMapperDomainToEntity.MapToEntity(item);  // mapping domain to entity

            await _shoppingListDbContext.Items.AddAsync(itemEntity);  // adding entity to DbContext

            await _shoppingListDbContext.SaveChangesAsync();   // saving changes to database
        }

        public async Task DeleteItem(int id)   // delete item from database
        {
            var itemInShoppingList = await _shoppingListDbContext.ShoppingListItems.FirstOrDefaultAsync(i => i.ItemId == id);   // checking if item is in a shopping list

            if (itemInShoppingList != null)   // if item is in a shopping list it can't be deleted
            {
                throw new InvalidOperationException("Item is in a shopping list and it can't be deleted");
            }

            else    // if item is not in a shopping list it can be deleted
            {
                var itemEntity = await _shoppingListDbContext.Items.FirstOrDefaultAsync(i => i.Id == id);

                if (itemEntity != null)
                {
                    _shoppingListDbContext.Items.Remove(itemEntity);
                    await _shoppingListDbContext.SaveChangesAsync();
                }
            }
        }

        public async Task EditItem(Item item)   // edit item from database
        {
            var itemEntity = ItemMapperDomainToEntity.MapToEntity(item);   // mapping domain to entity

            var itemToEdit = await _shoppingListDbContext.Items.FirstOrDefaultAsync(i =>i.Id == itemEntity.Id);

            if (itemToEdit != null)
            {
                itemToEdit.Name = itemEntity.Name;   // Assign the edited name to the item to be saved
                itemToEdit.Quantity = itemEntity.Quantity;  // Update quantity field

                _shoppingListDbContext.Entry(itemToEdit).State = EntityState.Modified;   // mark entity as modified in Entity Framework
                await _shoppingListDbContext.SaveChangesAsync();  // Save the changes to the database
            }
        }
    }
}
