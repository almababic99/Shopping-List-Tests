using Application.Interfaces;
using Domain.DomainModels;
using Infrastructure.Data;
using Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ShopperRepository : IShopperRepository  // ShopperRepository class implements IShopperRepository interface which means it is expected to define the operations declared in IShopperRepository
    {
        private readonly ShoppingListDbContext _shoppingListDbContext;
        public ShopperRepository(ShoppingListDbContext shoppingListDbContext)  // The constructor takes an instance of ShoppingListDbContext as a parameter and assigns it to a private field _shoppingListDbContext. This is a Dependency Injection
        {
            _shoppingListDbContext = shoppingListDbContext;
        }

        public async Task<IEnumerable<Shopper>> GetShoppers()   // get all shoppers                                                         
        {
            var shoppers = await _shoppingListDbContext.Shoppers.ToListAsync();  // Get all Shopper entities from the DB
            return shoppers.Select(shopper => ShopperMapperEntityToDomain.MapToDomain(shopper));  // Map each Shopper entity to ShopperDomain and return as an IEnumerable<Shopper>
        }

        public async Task<Shopper?> GetShopperById(int id)  // get shopper by id
        {
            var shopperEntity = await _shoppingListDbContext.Shoppers.FirstOrDefaultAsync(i => i.Id == id);
            return shopperEntity != null ? ShopperMapperEntityToDomain.MapToDomain(shopperEntity) : null;
            // if shopperEntity is not null method ShopperMapperEntityToDomain.MapToDomain(shopperEntity) is called to map entity model to domain model
            // if shopperEntity is null it returns null
        }

        public async Task<Shopper?> GetShopper(string name)  // get shopper by name
        {
            var shopperEntity = await _shoppingListDbContext.Shoppers.FirstOrDefaultAsync(i => i.Name.ToLower() == name.ToLower());
            return shopperEntity != null ? ShopperMapperEntityToDomain.MapToDomain(shopperEntity) : null;
        }

        public async Task AddShopper(Shopper shopper)  // add shopper to database
        {
            // if the name of shopper is not already in database we can add it:
            var shopperEntity = ShopperMapperDomainToEntity.MapToEntity(shopper);  // mapping domain to entity

            await _shoppingListDbContext.Shoppers.AddAsync(shopperEntity);  // adding entity to DbContext

            await _shoppingListDbContext.SaveChangesAsync();   // saving changes to database
        }

        public async Task DeleteShopper(int id)   // delete shopper from database
        {
            var shopperEntity = await _shoppingListDbContext.Shoppers.FirstOrDefaultAsync(i => i.Id == id);

            if (shopperEntity != null)
            {
                _shoppingListDbContext.Shoppers.Remove(shopperEntity);
                await _shoppingListDbContext.SaveChangesAsync();
            }
        }

        public async Task EditShopper(Shopper shopper)   // edit shopper from database
        {
            var shopperEntity = ShopperMapperDomainToEntity.MapToEntity(shopper);   // mapping domain to entity

            var shopperToEdit = await _shoppingListDbContext.Shoppers.FirstOrDefaultAsync(i => i.Id == shopperEntity.Id);

            if (shopperToEdit != null)
            {
                shopperToEdit.Name = shopperEntity.Name;   // Assign the edited name to the shopper to be saved
                _shoppingListDbContext.Entry(shopperToEdit).State = EntityState.Modified;   // mark entity as modified in Entity Framework
                await _shoppingListDbContext.SaveChangesAsync();  // Save the changes to the database
            }
        }
    }
}