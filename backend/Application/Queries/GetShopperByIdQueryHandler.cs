using Application.Exceptions;
using Application.Interfaces;
using Domain.DomainModels;
using MediatR;

namespace Application.Queries
{
    public class GetShopperByIdQueryHandler : IRequestHandler<GetShopperByIdQuery, Shopper>   // handler will handle GetShopperByIdQuery and return Shopper as the result
    {

        private readonly IShopperRepository _shopperRepository;

        public GetShopperByIdQueryHandler(IShopperRepository shopperRepository)
        {
            _shopperRepository = shopperRepository;
        }

        public async Task<Shopper> Handle(GetShopperByIdQuery request, CancellationToken cancellationToken)
        {
            var shopper = await _shopperRepository.GetShopperById(request.Id);

            if (shopper == null)
            {
                throw new ShopperNotFoundException($"Shopper with ID {request.Id} not found.");
            }

            return shopper;
        }
    }
}
