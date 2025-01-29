using Domain.DomainModels;
using MediatR;

namespace Application.Commands
{
    public class CreateShoppingListCommand : IRequest
    {
        public int Id { get; set; }
        public int? ShopperId { get; set; }
        public List<ShoppingListItem> Items { get; set; } = new List<ShoppingListItem>();
    }
}
