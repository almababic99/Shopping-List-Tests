using Application.Exceptions;
using Application.Interfaces;
using MediatR;

namespace Application.Commands
{
    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand>
    {
        private readonly IItemRepository _itemRepository;

        public DeleteItemCommandHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetItemById(request.Id);

            if (item == null)
                throw new ItemNotFoundException($"Item with ID {request.Id} not found.");

            await _itemRepository.DeleteItem(request.Id);
        }
    }
}
