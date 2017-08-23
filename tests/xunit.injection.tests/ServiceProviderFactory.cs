using System;

namespace Xunit.Injection.Tests
{
    public static class ServiceProviderFactory
    {
        public static IServiceProvider CreateServiceProvider()
        {
            return new TestServiceProvider();
        }

        private class TestServiceProvider : IServiceProvider
        {
            public object GetService(Type serviceType)
            {
                if(serviceType == typeof(ILoggingService))
                {
                    return new LoggingService();
                }
                return null;
            }
        }
    }
}
