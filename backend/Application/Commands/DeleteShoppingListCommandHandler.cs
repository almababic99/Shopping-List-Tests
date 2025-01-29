using Application.Exceptions;
using Application.Interfaces;
using MediatR;

namespace Application.Commands
{
    public class DeleteShoppingListCommandHandler : IRequestHandler<DeleteShoppingListCommand>
    {
        private readonly IShoppingListRepository _shoppingListRepository;

        public DeleteShoppingListCommandHandler(IShoppingListRepository shoppingListRepository)
        {
            _shoppingListRepository = shoppingListRepository;
        }

        public async Task Handle(DeleteShoppingListCommand request, CancellationToken cancellationToken)
        {
            var shoppingLists = await _shoppingListRepository.GetShoppingLists();

            var shoppingListToDelete = shoppingLists.FirstOrDefault(s => s.Id == request.Id);

            if (shoppingListToDelete == null)
                throw new ShoppingListsNotFoundException($"Shopping list with ID {request.Id} not found.");

            await _shoppingListRepository.DeleteShoppingList(request.Id);
        }
    }
}
