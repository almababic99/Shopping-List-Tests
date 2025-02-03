using Domain.DomainModels;
using FluentAssertions;
using Infrastructure.Data;
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
        private readonly ShopperRepository _shopperRepository;  

        public ShopperRepositoryTests()
        { 
            _shoppingListDbContext = CreateDbContext();
            _shopperRepository = new ShopperRepository(_shoppingListDbContext);

            // Cleanup the database before every test
            _shoppingListDbContext.Shoppers.RemoveRange(_shoppingListDbContext.Shoppers);
            _shoppingListDbContext.SaveChanges();
        }

        private ShoppingListDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ShoppingListDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new ShoppingListDbContext(options);

            dbContext.Shoppers.AddRange(GetFakeShoppers());

            dbContext.SaveChanges();

            return dbContext;
        }

        private List<ShopperEntity> GetFakeShoppers()
        {
            return new List<ShopperEntity>();
        }



        // Testing GetShoppers:
        [Fact]
        public async Task Given_Shoppers_When_GetShoppersIsCalled_Then_ReturnAllShoppers()
        {
            // Given
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
            var shopper1 = new ShopperEntityBuilder().WithId(1).WithName("John Doe").Build();
            var shopper2 = new ShopperEntityBuilder().WithId(2).WithName("Jane Smith").Build();
            _shoppingListDbContext.Shoppers.Add(shopper1);
            _shoppingListDbContext.Shoppers.Add(shopper2);
            await _shoppingListDbContext.SaveChangesAsync();

            // When
            await _shopperRepository.DeleteShopper(1);  

            // Then
            var deletedShopper = await _shoppingListDbContext.Shoppers.FirstOrDefaultAsync(s => s.Id == 1);
            deletedShopper.Should().BeNull();  

            var nonDeletedShopper = await _shoppingListDbContext.Shoppers.FirstOrDefaultAsync(s => s.Id == 2);
            nonDeletedShopper.Should().NotBeNull();   
        }

        [Fact]
        public async Task Given_NonExistingShopper_When_DeleteShopperIsCalled_Then_NoActionShouldBeTaken()
        {
            // Given
            var shopper = new ShopperEntityBuilder().WithId(1).WithName("John Doe").Build();
            _shoppingListDbContext.Shoppers.Add(shopper);
            await _shoppingListDbContext.SaveChangesAsync();

            // When
            // Attempting to delete a shopper that does not exist (with id 999)
            await _shopperRepository.DeleteShopper(999);

            // Then
            var shoppersCount = await _shoppingListDbContext.Shoppers.CountAsync();
            shoppersCount.Should().Be(1);  
        }



        // Testing EditShopper:
        [Fact]
        public async Task Given_ExistingShopper_When_EditShopperIsCalled_Then_ShopperShouldBeUpdated()
        {
            // Given
            var shopper = new ShopperEntityBuilder().WithId(1).WithName("John Doe").Build();
            _shoppingListDbContext.Shoppers.Add(shopper);
            await _shoppingListDbContext.SaveChangesAsync();

            var shopperToEdit = new Shopper { Id = 1, Name = "Updated John Doe" };

            // When
            await _shopperRepository.EditShopper(shopperToEdit);

            // Then
            var updatedShopper = await _shoppingListDbContext.Shoppers.FirstOrDefaultAsync(s => s.Id == shopperToEdit.Id);

            updatedShopper.Should().NotBeNull();   
            updatedShopper.Name.Should().Be("Updated John Doe");  
        }

        [Fact]
        public async Task Given_NonExistingShopper_When_EditShopperIsCalled_Then_NoChangesShouldBeMade()
        {
            // Given
            var shopperToEdit = new Shopper { Id = 999, Name = "Non Existing Shopper" };

            // When
            await _shopperRepository.EditShopper(shopperToEdit);

            // Then
            var nonExistingShopper = await _shoppingListDbContext.Shoppers.FirstOrDefaultAsync(s => s.Id == shopperToEdit.Id);

            nonExistingShopper.Should().BeNull();  
        }
    }
}
