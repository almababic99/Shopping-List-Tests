using Application.Exceptions;
using Application.Interfaces;
using Domain.DomainModels;
using MediatR;

namespace Application.Queries
{
    public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, IEnumerable<Item>>  // handler will handle GetItemsQuery and return IEnumerable<Item> as the result
    {
        public IItemRepository _itemRepository;

        public GetItemsQueryHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<IEnumerable<Item>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await _itemRepository.GetItems();

            if (items == null || !items.Any())
            {
                throw new ItemsNotFoundException($"Items not found.");
            }

            return items;
        }
    }
}
