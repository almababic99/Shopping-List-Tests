using MediatR;

namespace Application.Commands
{
    public class CreateShopperCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
