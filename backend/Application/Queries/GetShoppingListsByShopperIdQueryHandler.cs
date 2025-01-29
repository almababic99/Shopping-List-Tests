using Application.Exceptions;
using Application.Interfaces;
using Domain.DomainModels;
using MediatR;

namespace Application.Queries
{
    public class GetShoppingListsByShopperIdQueryHandler : IRequestHandler<GetShoppingListsByShopperIdQuery, IEnumerable<ShoppingList>>   // handler will handle GetShoppingListsByShopperIdQuery and return IEnumerable<ShoppingList> as the result
    {
        private readonly IShoppingListRepository _shoppingListRepository;
        public GetShoppingListsByShopperIdQueryHandler(IShoppingListRepository shoppingListRepository)
        {
            _shoppingListRepository = shoppingListRepository;
        }

        public async Task<IEnumerable<ShoppingList>> Handle(GetShoppingListsByShopperIdQuery request, CancellationToken cancellationToken)
        {
            var shoppingLists = await _shoppingListRepository.GetShoppingListsByShopperId(request.ShopperId);

            if (shoppingLists == null || !shoppingLists.Any())
            {
                throw new ShoppingListsByShopperIdNotFoundException($"Shopping lists for shopper with ID {request.ShopperId} not found.");
            }

            return shoppingLists;
        }
    }
}
