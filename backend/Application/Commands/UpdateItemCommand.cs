using MediatR;

namespace Application.Commands
{
    public class UpdateItemCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Quantity { get; set; }  // In how many shopping lists is item stored
    }
}
