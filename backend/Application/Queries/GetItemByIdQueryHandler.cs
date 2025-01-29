using Application.Exceptions;
using Application.Interfaces;
using Domain.DomainModels;
using MediatR;

namespace Application.Queries
{
    public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, Item>   // handler will handle GetItemByIdQuery and return Item as the result
    {
        private readonly IItemRepository _itemRepository;

        public GetItemByIdQueryHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<Item> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetItemById(request.Id);

            if (item == null)
            {
                throw new ItemNotFoundException($"Item with ID {request.Id} not found.");
            }

            return item;
        }
    }
}
