using Domain.DomainModels;
using MediatR;

namespace Application.Queries
{
    public class GetShopperByIdQuery : IRequest<Shopper>  // request expects a response of type Shopper (The query class is used to send a request for data)
    {
        public int Id { get; set; }   // query takes a single property (Id)
    }
}