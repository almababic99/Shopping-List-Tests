using MediatR;

namespace Application.Commands
{
    public class DeleteShoppingListCommand : IRequest
    {
        public int Id { get; set; }
    }
}
