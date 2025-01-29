using MediatR;

namespace Application.Commands
{
    public class DeleteItemCommand : IRequest
    {
        public int Id { get; set; }
    }
}
