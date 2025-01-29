using Infrastructure.EntityModels;

namespace Infrastructure.Models
{
    public class ShopperEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ShoppingListEntity> ShoppingLists { get; set; } = new List<ShoppingListEntity>();  // Shopper can have multiple shopping lists
    }
}
