using Application.Exceptions;
using Application.Interfaces;
using Domain.DomainModels;
using MediatR;

namespace Application.Queries
{
    public class GetShoppingListsQueryHandler : IRequestHandler<GetShoppingListsQuery, IEnumerable<ShoppingList>>  // handler will handle GetShoppingListsQuery and return IEnumerable<ShoppingList> as the result
    {
        private readonly IShoppingListRepository _shoppingListRepository;

        public GetShoppingListsQueryHandler(IShoppingListRepository shoppingListRepository) 
        {
            _shoppingListRepository = shoppingListRepository;
        }

        public async Task<IEnumerable<ShoppingList>> Handle(GetShoppingListsQuery request, CancellationToken cancellationToken)
        {
            var shoppingLists = await _shoppingListRepository.GetShoppingLists();

            if (shoppingLists == null || !shoppingLists.Any())
            {
                throw new ShoppingListsNotFoundException($"Shopping lists not found.");
            }

            return shoppingLists;
        }
    }
}
