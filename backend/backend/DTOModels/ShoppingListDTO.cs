namespace API.DTOModels
{
    public class ShoppingListDTO
    {
        public int Id { get; set; }
        public ShopperDTO? Shopper { get; set; }   // Shopper details
        public List<ShoppingListItemDTO> Items { get; set; } = new List<ShoppingListItemDTO>();  
    }
}
