using Infrastructure.Models;

namespace Infrastructure.EntityModels
{
    public class ShoppingListItemEntity
    {
        public int Id { get; set; }
        public int ShoppingListId { get; set; }  // Foreign key to ShoppingList
        public ShoppingListEntity? ShoppingList { get; set; }  // Navigation property
        public int ItemId { get; set; }  // Foreign key to Item
        public ItemEntity? Item { get; set; }  // Navigation property
    }
}

// ShoppingListItem represents the relationship between a shopping list and an item
