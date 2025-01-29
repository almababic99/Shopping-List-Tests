using Application.Interfaces;
using Domain.DomainModels;
using MediatR;

namespace Application.Commands
{
    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand>
    {
        private readonly IItemRepository _itemRepository;

        public CreateItemCommandHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var item = new Item { Id = request.Id, Name = request.Name, Quantity = request.Quantity };  // mapping dto to domain

            var existingItem = await _itemRepository.GetItem(item.Name);

            if (existingItem != null)
            {
                throw new InvalidOperationException("An item with the same name already exists");  // if the name already exists it throws an error
            }

            await _itemRepository.AddItem(item).ConfigureAwait(false);
        }
    }
}
