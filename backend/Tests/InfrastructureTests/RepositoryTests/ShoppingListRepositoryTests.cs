using Hangfire;
using Infrastructure.Data;
using Infrastructure.EntityModels;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using Tests.Builders;
using Domain.DomainModels;
using Moq;

namespace Tests.InfrastructureTests.RepositoryTests
{
    public class ShoppingListRepositoryTests
    {
        private readonly ShoppingListDbContext _shoppingListDbContext;
        private readonly Mock<IBackgroundJobClient> _backgroundJobClient;
        private readonly ShoppingListRepository _shoppingListRepository;

        public ShoppingListRepositoryTests()
        {
            _shoppingListDbContext = CreateDbContext();
            _backgroundJobClient = new Mock<IBackgroundJobClient>();
            _shoppingListRepository = new ShoppingListRepository(_shoppingListDbContext, _backgroundJobClient.Object);

            // Cleanup the database before every test
            _shoppingListDbContext.ShoppingLists.RemoveRange(_shoppingListDbContext.ShoppingLists);
            _shoppingListDbContext.SaveChanges();
        }

        private ShoppingListDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ShoppingListDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new ShoppingListDbContext(options);

            dbContext.ShoppingLists.AddRange(GetFakeShoppingLists());

            dbContext.SaveChanges();

            return dbContext;
        }

        private List<ShoppingListEntity> GetFakeShoppingLists()
        {
            return new List<ShoppingListEntity>();
        }


        // Testing GetShoppingLists:
        [Fact]
        public async Task Given_ShoppingLists_When_GetShoppingListsIsCalled_Then_ReturnAllShoppingLists()
        {
            // Given
            var shoppingListEntity = new ShoppingListEntityBuilder().WithId(1).WithShopperId(1).WithItems(new List<ShoppingListItemEntity> {
                        new ShoppingListItemEntity { Id = 1, ShoppingListId = 1, ItemId = 1 },
                        new ShoppingListItemEntity { Id = 2, ShoppingListId = 1, ItemId = 2 }
                    }).Build();
            _shoppingListDbContext.ShoppingLists.Add(shoppingListEntity);
            await _shoppingListDbContext.SaveChangesAsync();

            // When
            var shoppingLists = await _shoppingListRepository.GetShoppingLists();

            // Then
            shoppingLists.Should().NotBeNull();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Should().ContainSingle(s => s.Id == 1 && s.ShopperId == 1);

            var shoppingList = shoppingLists.Single(s => s.Id == 1);
            shoppingList.Items.Should().HaveCount(2);
            shoppingList.Items.Should().Contain(i => i.ItemId == 1);
            shoppingList.Items.Should().Contain(i => i.ItemId == 2);
        }

        [Fact]
        public async Task Given_NoShoppingLists_When_GetShoppingListsIsCalled_Then_ReturnEmptyList()
        {
            // Given
            // No shopping lists in the database

            // When
            var shoppingLists = await _shoppingListRepository.GetShoppingLists();

            // Then
            shoppingLists.Should().NotBeNull();
            shoppingLists.Should().BeEmpty();
        }



        // Testing GetShoppingListsByShopperId:
        [Fact]
        public async Task Given_ExistingShopperId_When_GetShoppingListsByShopperIdIsCalled_Then_ReturnShoppingListsByShopperId()
        {
            // Given
            var shopperId = 1;
            var shoppingListEntity = new ShoppingListEntityBuilder().WithId(1).WithShopperId(shopperId).WithItems(new List<ShoppingListItemEntity> {
                        new ShoppingListItemEntity { Id = 1, ShoppingListId = 1, ItemId = 1 },
                        new ShoppingListItemEntity { Id = 2, ShoppingListId = 1, ItemId = 2 }
                    }).Build();
            _shoppingListDbContext.ShoppingLists.Add(shoppingListEntity);
            await _shoppingListDbContext.SaveChangesAsync();

            // When
            var shoppingLists = await _shoppingListRepository.GetShoppingListsByShopperId(shopperId);

            // Then
            shoppingLists.Should().NotBeNull();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Should().Contain(s => s.ShopperId == 1);
            shoppingLists.Should().Contain(s => s.Id == 1);
        }

        [Fact]
        public async Task Given_NonExistingShopperId_When_GetShoppingListsByShopperIdIsCalled_Then_ReturnEmptyList()
        {
            // Given
            // No shopping lists in the database, or shopping list with shopper with id 999 that doesn't exist

            // When
            var shoppingListsById = await _shoppingListRepository.GetShoppingListsByShopperId(999);

            // Then
            shoppingListsById.Should().BeEmpty();
        }



        // Testing DeleteShoppingList:
        [Fact]
        public async Task Given_ExistingShoppingList_When_DeleteShoppingListIsCalled_Then_DeleteShoppingList()
        {
            // Given
            var shoppingListEntity = new ShoppingListEntityBuilder().WithId(1).WithShopperId(1).WithItems(new List<ShoppingListItemEntity> {
                        new ShoppingListItemEntity { Id = 1, ShoppingListId = 1, ItemId = 1 },
                        new ShoppingListItemEntity { Id = 2, ShoppingListId = 1, ItemId = 2 }
                    }).Build();
            _shoppingListDbContext.ShoppingLists.Add(shoppingListEntity);
            await _shoppingListDbContext.SaveChangesAsync();

            // When
            await _shoppingListRepository.DeleteShoppingList(1);

            // Then
            var shoppingList = await _shoppingListDbContext.ShoppingLists.FirstOrDefaultAsync(s => s.Id == 1);
            shoppingList.Should().BeNull();
        }

        [Fact]
        public async Task Given_NonExistingShoppingList_When_DeleteShoppingListIsCalled_Then_NoActionShouldBeTaken()
        {
            // Given
            var shoppingListEntity = new ShoppingListEntityBuilder().WithId(1).WithShopperId(1).WithItems(new List<ShoppingListItemEntity> {
                        new ShoppingListItemEntity { Id = 1, ShoppingListId = 1, ItemId = 1 },
                        new ShoppingListItemEntity { Id = 2, ShoppingListId = 1, ItemId = 2 }
                    }).Build();
            _shoppingListDbContext.ShoppingLists.Add(shoppingListEntity);
            await _shoppingListDbContext.SaveChangesAsync();

            // When
            // Attempting to delete a shopping list that does not exist (with id 999)
            await _shoppingListRepository.DeleteShoppingList(999);

            // Then
            var shoppingListsCount = await _shoppingListDbContext.ShoppingLists.CountAsync();
            shoppingListsCount.Should().Be(1);
        }



        // Testing AddShoppingList:
        [Fact]
        public async Task Given_NewShoppingList_When_AddShoppingListIsCalled_Then_ShoppingListShouldBeAdded()
        {
            // Given
            var newShoppingList = new ShoppingListBuilder().WithId(1).WithShopperId(1).WithItems(new List<ShoppingListItem>
            {
                new ShoppingListItem { Id = 1, ItemId = 1 },
                new ShoppingListItem { Id = 2, ItemId = 2 }
            }).Build();

            // When
            await _shoppingListRepository.AddShoppingList(newShoppingList);

            // Then
            var addedShoppingList = await _shoppingListDbContext.ShoppingLists.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id == 1 && s.ShopperId == 1);
            addedShoppingList.Should().NotBeNull();
            addedShoppingList.Items.Count.Should().Be(newShoppingList.Items.Count);

            addedShoppingList.Items.Should().Contain(i => i.ItemId == 1);
            addedShoppingList.Items.Should().Contain(i => i.ItemId == 2);
        }



        // Testing getCountOfItemInShoppingList:
        [Fact]
        public async Task Given_ShoppingListItems_When_getCountOfItemInShoppingListIsCalled_Then_ReturnItemCount()
        {
            // Given
            var shoppingListEntity = new ShoppingListEntityBuilder().WithId(1).WithShopperId(1).WithItems(new List<ShoppingListItemEntity> {
                        new ShoppingListItemEntity { Id = 1, ShoppingListId = 1, ItemId = 1 },
                        new ShoppingListItemEntity { Id = 2, ShoppingListId = 1, ItemId = 2 }
                    }).Build();
            _shoppingListDbContext.ShoppingLists.Add(shoppingListEntity);
            await _shoppingListDbContext.SaveChangesAsync();

            // When
            var count1 = await _shoppingListRepository.getCountOfItemInShoppingList(1);
            var count2 = await _shoppingListRepository.getCountOfItemInShoppingList(2);

            // Then
            count1.Should().Be(1);  // The item with ItemId = 1 is in 1 shopping list
            count2.Should().Be(1);  // The item with ItemId = 2 is in 1 shopping list
        }

        [Fact]
        public async Task Given_NoItems_When_getCountOfItemInShoppingListIsCalled_Then_ReturnItemCountZero()
        {
            // Given
            // No items or item with ItemId = 999 that doesn't exist

            // When
            var count = await _shoppingListRepository.getCountOfItemInShoppingList(999);

            // Then
            count.Should().Be(0);  
        }
    }
}
