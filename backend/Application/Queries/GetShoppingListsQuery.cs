using Domain.DomainModels;
using MediatR;

namespace Application.Queries
{
    public class GetShoppingListsQuery : IRequest<IEnumerable<ShoppingList>>   // request expects a response of type IEnumerable<ShoppingList> (The query class is used to send a request for data)
    {
    }
}
