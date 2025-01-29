using Application.Interfaces;
using Application.Jobs;
using Domain.DomainModels;
using Hangfire;
using Infrastructure.Data;
using Infrastructure.EntityModels;
using Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ShoppingListRepository : IShoppingListRepository
    {
        private readonly ShoppingListDbContext _shoppingListDbContext;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public ShoppingListRepository(ShoppingListDbContext shoppingListDbContext, IBackgroundJobClient backgroundJobClient)  // The constructor takes an instance of ShoppingListDbContext as a parameter and assigns it to a private field _shoppingListDbContext. This is a Dependency Injection
        {
            _shoppingListDbContext = shoppingListDbContext;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<IEnumerable<ShoppingList>> GetShoppingLists()   // get all shopping lists with Shopper and Items included (shopper and item names)                                                       
        {
            var shoppingLists = await _shoppingListDbContext.ShoppingLists   // without include this would only load ShoppingList data without Shopper and Items
                .Include(s => s.Shopper)   // Include Shopper from ShoppingListDTO
                .Include(s => s.Items)     // Include Items from ShoppingListDTO
                .ToListAsync();  // Get all ShoppingList entities from the DB
            return shoppingLists.Select(shoppingList => ShoppingListMapperEntityToDomain.MapToDomain(shoppingList));  // Map each ShoppingList entity to ShoppingListDomain and return as an IEnumerable<ShoppingList>
        }

        public async Task<IEnumerable<ShoppingList>> GetShoppingListsByShopperId(int shopperId)   // get shopping lists by shopper id
        {
            var shoppingLists = await _shoppingListDbContext.ShoppingLists
                .Where(s => s.ShopperId == shopperId)  // Filter by ShopperId
                .Include(s => s.Shopper)   // Include Shopper details
                .Include(s => s.Items)     // Include Items details
                .ToListAsync();
            return shoppingLists.Select(shoppingList => ShoppingListMapperEntityToDomain.MapToDomain(shoppingList));
        }

        public async Task DeleteShoppingList(int id)   // delete shopping list from database
        {
            var shoppingListEntity = await _shoppingListDbContext.ShoppingLists.FirstOrDefaultAsync(i => i.Id == id);

            if (shoppingListEntity != null)
            {
                _shoppingListDbContext.ShoppingLists.Remove(shoppingListEntity);
                await _shoppingListDbContext.SaveChangesAsync();
            }
        }

        public async Task AddShoppingList(ShoppingList shoppingList)  // add shopping list and shopping list items to database
        {
            var shoppingListEntity = ShoppingListMapperDomainToEntity.MapToEntity(shoppingList);  // mapping domain to entity

            await _shoppingListDbContext.ShoppingLists.AddAsync(shoppingListEntity);  // adding entity to DbContext

            await _shoppingListDbContext.SaveChangesAsync();   // saving changes to database

            foreach (var item in shoppingList.Items)
            {
                // Check if item already exists in the shopping list before adding
                var existingItem = await _shoppingListDbContext.ShoppingListItems
                    .FirstOrDefaultAsync(s => s.ShoppingListId == shoppingListEntity.Id && s.ItemId == item.ItemId);

                if (existingItem == null)
                {
                    // Map shopping list items domain models to shopping list items entities models
                    var shoppingListItemEntity = new ShoppingListItemEntity
                    {
                        ShoppingListId = shoppingListEntity.Id,
                        ItemId = item.ItemId
                    };

                    // Add the ShoppingListItem to the database
                    await _shoppingListDbContext.ShoppingListItems.AddAsync(shoppingListItemEntity);
                }
            }

            await _shoppingListDbContext.SaveChangesAsync();    // saving shopping list items to database

            // Enqueue or trigger the job after saving the shopping list and items
            _backgroundJobClient.Enqueue<NewShoppingListAddedInfoJob>(job => job.Execute());    // Fire and forget job
        }

        public async Task<int> getCountOfItemInShoppingList(int itemId)   // count the number of item in shopping list items  (One item can be found in maximum of 3 shopping lists)
        {
            return await _shoppingListDbContext.ShoppingListItems
                .Where(s => s.ItemId == itemId)   // Filter by ItemId
                .CountAsync();    // Count the number of items with itemId as ItemId
        } 
    }
}
