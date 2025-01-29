using Application.Exceptions;
using Application.Interfaces;
using MediatR;

namespace Application.Commands
{
    public class DeleteShopperCommandHandler : IRequestHandler<DeleteShopperCommand>
    {
        private readonly IShopperRepository _shopperRepository;

        public DeleteShopperCommandHandler(IShopperRepository shopperRepository)
        {
            _shopperRepository = shopperRepository;
        }

        public async Task Handle(DeleteShopperCommand request, CancellationToken cancellationToken)
        {
            var shopper = await _shopperRepository.GetShopperById(request.Id);

            if (shopper == null)
                throw new ShopperNotFoundException($"Shopper with ID {request.Id} not found.");

            await _shopperRepository.DeleteShopper(request.Id);
        }
    }
}
