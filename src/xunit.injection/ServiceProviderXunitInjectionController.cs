using System;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Injection
{
    public class ServiceProviderXunitInjectionController : IXunitInjectionController
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderXunitInjectionController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public bool TryGetConstructorArgument(ExceptionAggregator exceptionAggregator, ConstructorInfo constructor, int index, ParameterInfo parameter, out object argumentValue)
        {
            try
            {
                // Try to resolve it out of the service provider
                var service = _serviceProvider.GetService(parameter.ParameterType);
                if (service != null)
                {
                    argumentValue = service;
                    return true;
                }
            }
            catch (Exception ex)
            {
                exceptionAggregator.Add(ex);
            }

            argumentValue = null;
            return false;
        }

        public bool TrySelectTestClassConstructor(ExceptionAggregator exceptionAggregator, IReflectionTypeInfo testClass, out ConstructorInfo constructor)
        {
            // Use the base logic, which expects only one constructor
            constructor = null;
            return false;
        }
    }
}
