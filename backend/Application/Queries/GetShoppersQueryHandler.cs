using Application.Exceptions;
using Application.Interfaces;
using Domain.DomainModels;
using MediatR;

namespace Application.Queries
{
    public class GetShoppersQueryHandler : IRequestHandler<GetShoppersQuery, IEnumerable<Shopper>>  // handler will handle GetShoppersQuery and return IEnumerable<Shopper> as the result
    {
        private readonly IShopperRepository _shopperRepository;

        public GetShoppersQueryHandler(IShopperRepository shopperRepository)
        {
            _shopperRepository = shopperRepository;
        }

        public async Task<IEnumerable<Shopper>> Handle(GetShoppersQuery request, CancellationToken cancellationToken)
        {
            var shoppers = await _shopperRepository.GetShoppers();

            if (shoppers == null || !shoppers.Any())
            {
                throw new ShoppersNotFoundException($"Shoppers not found.");
            }

            return shoppers;
        }
    }
}
