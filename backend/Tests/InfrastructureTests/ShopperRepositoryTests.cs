using Domain.DomainModels;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.EntityModels;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Tests.Builders;
using Xunit;

namespace Tests.InfrastructureTests
{
    public class ShopperRepositoryTests
    {
        private readonly ShoppingListDbContext _shoppingListDbContext;
        private readonly ShopperRepository _shopperRepository;  // testing the real repository implementation, so there's no need to mock ShopperRepository

        public ShopperRepositoryTests()
        { 
            _shoppingListDbContext = CreateDbContext();
            _shopperRepository = new ShopperRepository(_shoppingListDbContext);
        }

        private ShoppingListDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ShoppingListDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new ShoppingListDbContext(options);

            // Add fake data to all tables
            dbContext.Shoppers.AddRange(GetFakeShoppers());
            dbContext.Items.AddRange(GetFakeItems());
            dbContext.ShoppingLists.AddRange(GetFakeShoppingLists());
            dbContext.ShoppingListItems.AddRange(GetFakeShoppingListItems());

            dbContext.SaveChanges();

            return dbContext;
        }

        private List<ShopperEntity> GetFakeShoppers()
        {
            return new List<ShopperEntity>();
        }

        private List<ItemEntity> GetFakeItems()
        {
            return new List<ItemEntity>();
        }

        private List<ShoppingListEntity> GetFakeShoppingLists()
        {
            return new List<ShoppingListEntity>();
        }

        private List<ShoppingListItemEntity> GetFakeShoppingListItems()
        {
            return new List<ShoppingListItemEntity>();
        }



        // Testing GetShoppers:
        [Fact]
        public async Task Given_Shoppers_When_GetShoppersIsCalled_Then_ReturnAllShoppers()
        {
            // Given
            _shoppingListDbContext.Shoppers.RemoveRange(_shoppingListDbContext.Shoppers);  // Cleanup before adding new data
            await _shoppingListDbContext.SaveChangesAsync();

            var shopper1 = new ShopperEntityBuilder().WithId(1).WithName("John Doe").Build();
            var shopper2 = new ShopperEntityBuilder().WithId(2).WithName("Jane Smith").Build();
            _shoppingListDbContext.Shoppers.Add(shopper1);
            _shoppingListDbContext.Shoppers.Add(shopper2);
            await _shoppingListDbContext.SaveChangesAsync();

            // When
            var shoppers = await _shopperRepository.GetShoppers();

            // Then
            shoppers.Should().NotBeNull();
            shoppers.Should().HaveCount(2);
            shoppers.Should().ContainSingle(s => s.Id == 1 && s.Name == "John Doe");
            shoppers.Should().ContainSingle(s => s.Id == 2 && s.Name == "Jane Smith");
        }

        [Fact]
        public async Task Given_NoShoppers_When_GetShoppersIsCalled_Then_ReturnEmptyList()
        {
            // Given
            // no shoppers in the database

            // When
            var shoppers = await _shopperRepository.GetShoppers();

            // Then
            shoppers.Should().NotBeNull();  
            shoppers.Should().BeEmpty();  
        }



        // Testing GetShopperById:
        [Fact]
        public async Task Given_ExistingShopper_When_GetShopperByIdIsCalled_Then_ReturnShopper()
        {
            // Given
            _shoppingListDbContext.Shoppers.RemoveRange(_shoppingListDbContext.Shoppers);  // Cleanup before adding new data
            await _shoppingListDbContext.SaveChangesAsync();

            var shopper = new ShopperEntityBuilder().WithId(1).WithName("John Doe").Build();
            _shoppingListDbContext.Shoppers.Add(shopper);
            await _shoppingListDbContext.SaveChangesAsync();

            // When
            var shopperById = await _shopperRepository.GetShopperById(1);

            // Then
            shopperById.Should().NotBeNull();
            shopperById.Id.Should().Be(1);
            shopperById.Name.Should().Be("John Doe");
        }

        [Fact]
        public async Task Given_NonExistingShopper_When_GetShopperByIdIsCalled_Then_ReturnNull()
        {
            // Given
            // No shoppers in the database, or shopper with id 999 that doesn't exist

            // When
            var shopperById = await _shopperRepository.GetShopperById(999);

            // Then
            shopperById.Should().BeNull();  
        }



        // Testing GetShopper (by name):
        [Fact]
        public async Task Given_ExistingShopper_When_GetShopperIsCalled_Then_ReturnShopper()
        {
            // Given
            _shoppingListDbContext.Shoppers.RemoveRange(_shoppingListDbContext.Shoppers);  // Cleanup before adding new data
            await _shoppingListDbContext.SaveChangesAsync();

            var shopper = new ShopperEntityBuilder().WithId(1).WithName("John Doe").Build();
            _shoppingListDbContext.Shoppers.Add(shopper);
            await _shoppingListDbContext.SaveChangesAsync();

            // When
            var shopperById = await _shopperRepository.GetShopper("John Doe");

            // Then
            shopperById.Should().NotBeNull();
            shopperById.Id.Should().Be(1);
            shopperById.Name.Should().Be("John Doe");
        }

        [Fact]
        public async Task Given_NonExistingShopper_When_GetShopperIsCalled_Then_ReturnNull()
        {
            // Given
            // No shoppers in the database, or shopper with name "x" that doesn't exist

            // When
            var shopperById = await _shopperRepository.GetShopper("x");

            // Then
            shopperById.Should().BeNull();
        }



        // Testing AddShopper:
        [Fact]
        public async Task Given_NewShopper_When_AddShopperIsCalled_Then_ShopperShouldBeAdded()
        {
            // Given
            _shoppingListDbContext.Shoppers.RemoveRange(_shoppingListDbContext.Shoppers);  // Cleanup before adding new data
            await _shoppingListDbContext.SaveChangesAsync();
            var newShopper = new Shopper { Name = "John Doe" };

            // When
            await _shopperRepository.AddShopper(newShopper);

            // Then
            var addedShopper = await _shoppingListDbContext.Shoppers
                .FirstOrDefaultAsync(s => s.Name == newShopper.Name);

            addedShopper.Should().NotBeNull();
            addedShopper.Name.Should().Be(newShopper.Name);
        }



        // Testing DeleteShopper:
        [Fact]
        public async Task Given_ExistingShopper_When_DeleteShopperIsCalled_Then_ShopperShouldBeDeleted()
        {
            // Given
            _shoppingListDbContext.Shoppers.RemoveRange(_shoppingListDbContext.Shoppers);  // Cleanup before adding new data
            await _shoppingListDbContext.SaveChangesAsync();

            var shopper1 = new ShopperEntityBuilder().WithId(1).WithName("John Doe").Build();
            var shopper2 = new ShopperEntityBuilder().WithId(2).WithName("Jane Smith").Build();
            _shoppingListDbContext.Shoppers.Add(shopper1);
            _shoppingListDbContext.Shoppers.Add(shopper2);
            await _shoppingListDbContext.SaveChangesAsync();

            // When
            await _shopperRepository.DeleteShopper(1);  // Deleting the shopper with id 1

            // Then
            var deletedShopper = await _shoppingListDbContext.Shoppers.FirstOrDefaultAsync(s => s.Id == 1);
            deletedShopper.Should().BeNull();  // shopper1 should no longer exist in database

            var nonDeletedShopper = await _shoppingListDbContext.Shoppers.FirstOrDefaultAsync(s => s.Id == 2);
            nonDeletedShopper.Should().NotBeNull();   // shopper2 should still exist in database
        }

        [Fact]
        public async Task Given_NonExistingShopper_When_DeleteShopperIsCalled_Then_NoActionShouldBeTaken()
        {
            // Given
            _shoppingListDbContext.Shoppers.RemoveRange(_shoppingListDbContext.Shoppers);  // Cleanup before adding new data
            await _shoppingListDbContext.SaveChangesAsync();

            var shopper = new ShopperEntityBuilder().WithId(1).WithName("John Doe").Build();
            _shoppingListDbContext.Shoppers.Add(shopper);
            await _shoppingListDbContext.SaveChangesAsync();

            // When
            // Attempting to delete a shopper that does not exist (with id 999)
            await _shopperRepository.DeleteShopper(999);

            // Then
            var shoppersCount = await _shoppingListDbContext.Shoppers.CountAsync();
            shoppersCount.Should().Be(1);  // No shoppers should be in the database
        }



        // Testing EditShopper:
        
    }
}
