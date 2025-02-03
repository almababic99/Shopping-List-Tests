using Application.Jobs;
using FluentAssertions;
using Xunit;

namespace Tests.ApplicationTests.JobTests
{
    public class WelcomeMessageJobTests
    {
        [Fact]
        public void Given_WelcomeMessageJob_When_ExecuteIsCalled_Then_PrintWelcomeMessageInConsole()
        {
            // Given
            var job = new WelcomeMessageJob();
            var stringWriter = new StringWriter();   // StringWriter is an object used to capture anything that is written to the console
            Console.SetOut(stringWriter);            // This redirects the console output to the stringWriter instead of the default console window

            // When
            job.Execute();

            // Then
            stringWriter.ToString().Trim().Should().Be("Welcome to the Shopping List!");  
            // This retrieves the console output captured by stringWriter (ToString), removes any leading or trailing whitespace (Trim), and resulting string should match the message "Welcome to the Shopping List!".
        }
    }
}
