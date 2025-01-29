using Domain.DomainModels;
using MediatR;

namespace Application.Queries
{
    public class GetShoppersQuery : IRequest<IEnumerable<Shopper>>  // request expects a response of type IEnumerable<Shopper> (The query class is used to send a request for data)
    {
        
    }
}
