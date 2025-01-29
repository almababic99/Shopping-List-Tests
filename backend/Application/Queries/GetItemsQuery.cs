using Domain.DomainModels;
using MediatR;

namespace Application.Queries
{
    public class GetItemsQuery : IRequest<IEnumerable<Item>>   // request expects a response of type IEnumerable<Item> (The query class is used to send a request for data)
    {
    }
}
