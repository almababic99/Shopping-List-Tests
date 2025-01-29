using Infrastructure.Models;

namespace Infrastructure.EntityModels
{
    public class ShoppingListEntity
    {
        public int Id { get; set; }
        public int? ShopperId { get; set; }
        public ShopperEntity? Shopper { get; set; }  // Navigation property
        public List<ShoppingListItemEntity> Items { get; set; } = new List<ShoppingListItemEntity>();  // List of ShoppingListItem that connects ShoppingList to Item
    }
}

// Adding the navigation property made EF understand the relationship between ShoppingListEntity and ShopperEntity better, ensuring it could:
// Recognize and use the correct foreign key (ShopperId), Automatically map and manage the relationship, Resolve the issue where EF was mistakenly looking for a non-existent ShopperEntityId column.
