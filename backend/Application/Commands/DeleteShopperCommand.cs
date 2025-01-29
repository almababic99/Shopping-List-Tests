using MediatR;

namespace Application.Commands
{
    public class DeleteShopperCommand : IRequest
    {
        public int Id { get; set; }
    }
}
