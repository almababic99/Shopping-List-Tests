using MediatR;

namespace Application.Commands
{
    public class UpdateShopperCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
