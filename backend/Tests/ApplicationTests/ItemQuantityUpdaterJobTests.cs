using Application.Interfaces;
using Application.Jobs;
using Domain.DomainModels;
using Moq;
using Tests.Builders;
using Xunit;

namespace Tests.ApplicationTests
{
    public class ItemQuantityUpdaterJobTests
    {
        private readonly Mock<IItemRepository> _itemRepository;
        private readonly Mock<IShoppingListRepository> _shoppingListRepository;
        private readonly ItemQuantityUpdaterJob _itemQuantityUpdaterJob;

        public ItemQuantityUpdaterJobTests()
        {
            _itemRepository = new Mock<IItemRepository>();
            _shoppingListRepository = new Mock<IShoppingListRepository>();
            _itemQuantityUpdaterJob = new ItemQuantityUpdaterJob(_itemRepository.Object, _shoppingListRepository.Object);
        }

        [Fact]
        public async Task Given_Items_When_ExecuteIsCalled_Then_ItemQuantitiesAreUpdated()
        {
            // Given
            var items = new List<Item>
            {
                ItemBuilder.WithDefaults().WithId(1).WithName("Milk").WithQuantity(2).Build(),
                ItemBuilder.WithDefaults().WithId(2).WithName("Apple").WithQuantity(3).Build(),
            };

            _itemRepository.Setup(repo => repo.GetItems()).ReturnsAsync(items);
            _shoppingListRepository.Setup(repo => repo.getCountOfItemInShoppingList(1)).ReturnsAsync(2);
            _shoppingListRepository.Setup(repo => repo.getCountOfItemInShoppingList(2)).ReturnsAsync(3);

            // When
            await _itemQuantityUpdaterJob.Execute();

            // Then
            _itemRepository.Verify(repo => repo.EditItem(It.Is<Item>(item => item.Id == 1 && item.Quantity == 2)));
            _itemRepository.Verify(repo => repo.EditItem(It.Is<Item>(item => item.Id == 2 && item.Quantity == 3)));
        }
    }
}
