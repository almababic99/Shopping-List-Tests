namespace Application.Jobs
{
    public class NewShoppingListAddedInfoJob     // Fire and forget job (triggered from ShoppingListRepository.cs when shopping list is added) 
    {
        public void Execute()
        {
            string message = "New shopping list added!";
            Console.WriteLine(message);
        }
    }
}
