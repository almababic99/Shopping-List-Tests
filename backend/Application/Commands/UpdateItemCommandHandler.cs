using Application.Interfaces;
using Domain.DomainModels;
using MediatR;

namespace Application.Commands
{
    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand>
    {
        private readonly IItemRepository _itemRepository;

        public UpdateItemCommandHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var item = new Item { Id = request.Id, Name = request.Name, Quantity = request.Quantity };  // mapping dto to domain

            await _itemRepository.EditItem(item);
        }
    }
}
