using System.Collections.Generic;
using System.Text;

namespace Xunit.Injection.Tests
{
    [ServiceProviderXunitInjection("xunit.injection.tests", "Xunit.Injection.Tests.ServiceProviderFactory", "CreateServiceProvider")]
    public class ServiceProviderInjectionTest
    {
        private readonly ILoggingService _logging;

        public ServiceProviderInjectionTest(ILoggingService logging)
        {
            _logging = logging;
        }

        [Fact]
        public void TypeIsInjected()
        {
            Assert.NotNull(_logging);
            Assert.IsType<LoggingService>(_logging);
        }
    }
}
