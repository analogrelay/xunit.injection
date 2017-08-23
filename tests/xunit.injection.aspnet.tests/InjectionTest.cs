using Microsoft.Extensions.Logging;

namespace Xunit.Injection.AspNet.Tests
{
    public class InjectionTest
    {
        private readonly ILogger<InjectionTest> _logger;

        public InjectionTest(ILogger<InjectionTest> logger)
        {
            _logger = logger;
        }

        [Fact]
        public void LoggingWorks()
        {
            _logger.LogInformation("Log Message");

            Assert.Equal("[Information] Log Message", TestStartup.GlobalLogWriter.GetStringBuilder().ToString());
        }
    }
}
