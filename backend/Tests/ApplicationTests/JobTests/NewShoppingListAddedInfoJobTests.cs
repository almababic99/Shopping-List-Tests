using Application.Jobs;
using FluentAssertions;
using Xunit;

namespace Tests.ApplicationTests.JobTests
{
    public class NewShoppingListAddedInfoJobTests
    {
        [Fact]
        public void Given_NewShoppingListAddedInfoJob_When_ExecuteIsCalled_Then_PrintNewShoppingListAddedMessageInConsole()
        {
            // Given
            var job = new NewShoppingListAddedInfoJob();
            var stringWriter = new StringWriter();       // StringWriter is an object used to capture anything that is written to the console
            Console.SetOut(stringWriter);                // This redirects the console output to the stringWriter instead of the default console window

            // When
            job.Execute();

            // Then
            stringWriter.ToString().Trim().Should().Be("New shopping list added!");
            // This retrieves the console output captured by stringWriter (ToString), removes any leading or trailing whitespace (Trim), and resulting string should match the message "New shopping list added!".
        }
    }
}
