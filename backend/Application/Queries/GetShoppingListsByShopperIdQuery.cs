using Domain.DomainModels;
using MediatR;

namespace Application.Queries
{
    public class GetShoppingListsByShopperIdQuery : IRequest<IEnumerable<ShoppingList>>  // request expects a response of type IEnumerable<ShoppingList> (The query class is used to send a request for data)
    {
        public int ShopperId { get; set; }    // query takes a single property (ShopperId)
    }
}
