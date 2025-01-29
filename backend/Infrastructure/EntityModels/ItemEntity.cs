using Infrastructure.EntityModels;

namespace Infrastructure.Models
{
    public class ItemEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ShoppingListItemEntity> ShoppingLists { get; set; } = new List<ShoppingListItemEntity>();  // Item can be in multiple shopping lists

        public int Quantity { get; set; }  // In how many shopping lists is item stored
    }
}
