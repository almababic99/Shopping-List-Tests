namespace Domain.DomainModels
{
    public class ShoppingListItem
    {
        public int Id { get; set; }
        public int ShoppingListId { get; set; }  // Foreign key to ShoppingList
        public int ItemId { get; set; }  // Foreign key to Item
    }
}
