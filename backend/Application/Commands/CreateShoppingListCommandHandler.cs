using Application.Exceptions;
using Application.Interfaces;
using Domain.DomainModels;
using MediatR;

namespace Application.Commands
{
    public class CreateShoppingListCommandHandler : IRequestHandler<CreateShoppingListCommand>
    {
        private readonly IShoppingListRepository _shoppingListRepository;
        public CreateShoppingListCommandHandler(IShoppingListRepository shoppingListRepository)
        {
            _shoppingListRepository = shoppingListRepository;
        }

        public async Task Handle(CreateShoppingListCommand request, CancellationToken cancellationToken)
        {
            // One item can be found in maximum of 3 shopping lists:
            foreach (var shoppingListItem in request.Items)
            {
                var countOfItemInShoppingList = await _shoppingListRepository.getCountOfItemInShoppingList(shoppingListItem.ItemId);
                if (countOfItemInShoppingList >= 3)
                {
                    throw new ShoppingListItemException($"Item with ID {shoppingListItem.ItemId} is already in 3 shopping lists and one item can be found in maximum of 3 shopping lists");
                }
            }

            var shoppingList = new ShoppingList
            {
                Id = request.Id,
                ShopperId = request.ShopperId,
                Items = request.Items
            };

            await _shoppingListRepository.AddShoppingList(shoppingList);
        }
    }
}
