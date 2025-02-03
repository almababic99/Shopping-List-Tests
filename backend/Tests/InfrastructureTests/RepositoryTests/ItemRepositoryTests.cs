using Infrastructure.Data;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Tests.Builders;
using Xunit;
using FluentAssertions;
using Domain.DomainModels;

namespace Tests.InfrastructureTests.RepositoryTests
{
    public class ItemRepositoryTests
    {
        private readonly ShoppingListDbContext _shoppingListDbContext;
        private readonly ItemRepository _itemRepository;

        public ItemRepositoryTests()
        {
            _shoppingListDbContext = CreateDbContext();
            _itemRepository = new ItemRepository(_shoppingListDbContext);

            // Cleanup the database before every test
            _shoppingListDbContext.Items.RemoveRange(_shoppingListDbContext.Items);
            _shoppingListDbContext.SaveChanges();
        }

        private ShoppingListDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ShoppingListDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new ShoppingListDbContext(options);

            dbContext.Items.AddRange(GetFakeItems());

            dbContext.SaveChanges();

            return dbContext;
        }

        private List<ItemEntity> GetFakeItems()
        {
            return new List<ItemEntity>();
        }



        // Testing GetItems:
        [Fact]
        public async Task Given_Items_When_GetItemsIsCalled_Then_ReturnAllItems()
        {
            // Given
            var item1 = new ItemEntityBuilder().WithId(1).WithName("Milk").Build();
            var item2 = new ItemEntityBuilder().WithId(2).WithName("Apple").Build();
            _shoppingListDbContext.Items.Add(item1);
            _shoppingListDbContext.Items.Add(item2);
            await _shoppingListDbContext.SaveChangesAsync();

            // When
            var items = await _itemRepository.GetItems();

            // Then
            items.Should().NotBeNull();
            items.Should().HaveCount(2);
            items.Should().Contain(s => s.Id == 1 && s.Name == "Milk");
            items.Should().Contain(s => s.Id == 2 && s.Name == "Apple");
        }

        [Fact]
        public async Task Given_NoItems_When_GetItemsIsCalled_Then_ReturnEmptyList()
        {
            // Given
            // no items in the database

            // When
            var items = await _itemRepository.GetItems();

            // Then
            items.Should().NotBeNull();
            items.Should().BeEmpty();
        }



        // Testing GetItemById:
        [Fact]
        public async Task Given_ExistingItem_When_GetItemByIdIsCalled_Then_ReturnItem()
        {
            // Given
            var item = new ItemEntityBuilder().WithId(1).WithName("Milk").WithQuantity(1).Build();
            _shoppingListDbContext.Items.Add(item);
            await _shoppingListDbContext.SaveChangesAsync();

            // When
            var itemById = await _itemRepository.GetItemById(1);

            // Then
            itemById.Should().NotBeNull();
            itemById.Id.Should().Be(1);
            itemById.Name.Should().Be("Milk");
            itemById.Quantity.Should().Be(1);
        }

        [Fact]
        public async Task Given_NonExistingItem_When_GetItemByIdIsCalled_Then_ReturnNull()
        {
            // Given
            // No items in the database, or item with id 999 that doesn't exist

            // When
            var itemById = await _itemRepository.GetItemById(999);

            // Then
            itemById.Should().BeNull();
        }



        // Testing GetItem (by name):
        [Fact]
        public async Task Given_ExistingItem_When_GetItemIsCalled_Then_ReturnItem()
        {
            // Given
            var item = new ItemEntityBuilder().WithId(1).WithName("Milk").WithQuantity(1).Build();
            _shoppingListDbContext.Items.Add(item);
            await _shoppingListDbContext.SaveChangesAsync();

            // When
            var itemById = await _itemRepository.GetItem("Milk");

            // Then
            itemById.Should().NotBeNull();
            itemById.Id.Should().Be(1);
            itemById.Name.Should().Be("Milk");
            itemById.Quantity.Should().Be(1);
        }

        [Fact]
        public async Task Given_NonExistingItem_When_GetItemIsCalled_Then_ReturnNull()
        {
            // Given
            // No items in the database, or item with name "x" that doesn't exist

            // When
            var itemById = await _itemRepository.GetItem("Milk");

            // Then
            itemById.Should().BeNull();
        }



        // Testing AddItem:
        [Fact]
        public async Task Given_NewItem_When_AddItemIsCalled_Then_ItemShouldBeAdded()
        {
            // Given
            var newItem = new Item { Name = "Milk" };

            // When
            await _itemRepository.AddItem(newItem);

            // Then
            var addedItem = await _shoppingListDbContext.Items
                .FirstOrDefaultAsync(s => s.Name == newItem.Name);

            addedItem.Should().NotBeNull();
            addedItem.Name.Should().Be(newItem.Name);
        }



        // Testing DeleteItem:
        [Fact]
        public async Task Given_ExistingItem_When_DeleteItemIsCalled_Then_ItemShouldBeDeleted()
        {
            // Given
            var item1 = new ItemEntityBuilder().WithId(1).WithName("Milk").WithQuantity(1).Build();
            var item2 = new ItemEntityBuilder().WithId(2).WithName("Apple").WithQuantity(2).Build();
            _shoppingListDbContext.Items.Add(item1);
            _shoppingListDbContext.Items.Add(item2);
            await _shoppingListDbContext.SaveChangesAsync();

            // When
            await _itemRepository.DeleteItem(1);

            // Then
            var deletedItem = await _shoppingListDbContext.Items.FirstOrDefaultAsync(s => s.Id == 1);
            deletedItem.Should().BeNull();

            var nonDeletedItem = await _shoppingListDbContext.Items.FirstOrDefaultAsync(s => s.Id == 2);
            nonDeletedItem.Should().NotBeNull();
        }

        [Fact]
        public async Task Given_NonExistingItem_When_DeleteItemIsCalled_Then_NoActionShouldBeTaken()
        {
            // Given
            var item = new ItemEntityBuilder().WithId(1).WithName("Milk").WithQuantity(1).Build();
            _shoppingListDbContext.Items.Add(item);
            await _shoppingListDbContext.SaveChangesAsync();

            // When
            // Attempting to delete an item that does not exist (with id 999)
            await _itemRepository.DeleteItem(999);

            // Then
            var itemsCount = await _shoppingListDbContext.Items.CountAsync();
            itemsCount.Should().Be(1);
        }



        // Testing EditItem:
        [Fact]
        public async Task Given_ExistingItem_When_EditItemIsCalled_Then_ItemShouldBeUpdated()
        {
            // Given
            var item = new ItemEntityBuilder().WithId(1).WithName("Milk").WithQuantity(1).Build();
            _shoppingListDbContext.Items.Add(item);
            await _shoppingListDbContext.SaveChangesAsync();

            var itemToEdit = new Item { Id = 1, Name = "Updated milk" };

            // When
            await _itemRepository.EditItem(itemToEdit);

            // Then
            var updatedITem = await _shoppingListDbContext.Items.FirstOrDefaultAsync(s => s.Id == itemToEdit.Id);

            updatedITem.Should().NotBeNull();
            updatedITem.Name.Should().Be("Updated milk");
        }

        [Fact]
        public async Task Given_NonExistingItem_When_EditItemIsCalled_Then_NoChangesShouldBeMade()
        {
            // Given
            var itemToEdit = new Item { Id = 999, Name = "Non Existing Item" };

            // When
            await _itemRepository.EditItem(itemToEdit);

            // Then
            var nonExistingItem = await _shoppingListDbContext.Items.FirstOrDefaultAsync(s => s.Id == itemToEdit.Id);

            nonExistingItem.Should().BeNull();
        }
    }
}
