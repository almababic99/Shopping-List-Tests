using Application.Interfaces;
using Domain.DomainModels;
using MediatR;

namespace Application.Commands
{
    public class UpdateShopperCommandHandler : IRequestHandler<UpdateShopperCommand>
    {
        private readonly IShopperRepository _shopperRepository;

        public UpdateShopperCommandHandler(IShopperRepository shopperRepository)
        {
            _shopperRepository = shopperRepository;
        }

        public async Task Handle(UpdateShopperCommand request, CancellationToken cancellationToken)
        {
            var shopper = new Shopper { Id = request.Id, Name = request.Name };  // mapping dto to domain

            await _shopperRepository.EditShopper(shopper);
        }
    }
}
