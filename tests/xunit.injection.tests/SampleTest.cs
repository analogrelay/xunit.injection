using System;
using Xunit;

[assembly: TestFramework("Xunit.Injection.XunitInjectingTestFramework", "xunit.injection")]

namespace xunit.injections.tests
{
    public class SampleTest
    {
        private readonly ILoggingService _logging;

        public SampleTest(ILoggingService logging)
        {
            _logging = logging;
        }

        [Fact]
        public void SimpleEqualsTest()
        {
            _logging.Log("Testing equality");
            Assert.Equal(1, 1);
            _logging.Log("Tested equality");
        }
    }
}
