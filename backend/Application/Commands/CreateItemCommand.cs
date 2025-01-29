using MediatR;

namespace Application.Commands
{
    public class CreateItemCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Quantity { get; set; }  // In how many shopping lists is item stored
    }
}
