namespace Xunit.Injection.Tests
{
    [DefaultConstructorInjectionController]
    public class DefaultConstructorInjectionTest
    {
        private readonly LoggingService _logging;

        public DefaultConstructorInjectionTest(LoggingService logging)
        {
            _logging = logging;
        }

        [Fact]
        public void TypeIsInjected()
        {
            Assert.NotNull(_logging);
        }
    }
}
