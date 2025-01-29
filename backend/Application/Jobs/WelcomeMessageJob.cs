namespace Application.Jobs
{
    public class WelcomeMessageJob   // Delayed job (triggered from Program.cs delayed by 10 seconds)
    {
        public void Execute()
        {
            string message = "Welcome to the Shopping List!";
            Console.WriteLine(message); 
        }
    }
}
