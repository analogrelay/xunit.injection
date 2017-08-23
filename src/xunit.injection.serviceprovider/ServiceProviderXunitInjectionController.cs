using System;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Injection.ServiceProvider
{
    public sealed class ServiceProviderInjectionAttribute : Attribute, IXunitInjectionControllerAttribute
    {
        private readonly string _assemblyName;
        private readonly string _typeName;
        private readonly string _methodName;

        public ServiceProviderInjectionAttribute(string assemblyName, string typeName, string methodName)
        {
            _assemblyName = assemblyName;
            _typeName = typeName;
            _methodName = methodName;
        }

        public IXunitInjectionController CreateInjectionController()
        {
            var asm = Assembly.Load(_assemblyName);
            if(asm == null)
            {
                throw new InvalidOperationException($"Cannot load assembly: {_assemblyName}");
            }

            var typ = asm.GetType(_typeName);
            if(typ == null)
            {
                throw new InvalidOperationException($"Cannot find type: {_typeName}");
            }

            var method = typ.GetMethod(_methodName, BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, Type.EmptyTypes, new ParameterModifier[0]);
            if(method == null)
            {
                throw new InvalidOperationException($"Cannot find public static parameter-less method: {_methodName}");
            }

            if(method.ReturnType != typeof(IServiceProvider))
            {
                throw new InvalidOperationException($"{_methodName} does not return an {nameof(IServiceProvider)}");
            }

            // Invoke the method
            var result = method.Invoke(null, new object[0]);
            if(result == null)
            {
                throw new InvalidOperationException($"{_methodName} returned null");
            }
            return new ServiceProviderXunitInjectionController((IServiceProvider)result);
        }
    }

    public class ServiceProviderXunitInjectionController : IXunitInjectionController
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderXunitInjectionController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public bool TryGetConstructorArgument(ExceptionAggregator exceptionAggregator, ConstructorInfo constructor, int index, ParameterInfo parameter, out object argumentValue)
        {
            throw new NotImplementedException();
        }

        public bool TrySelectTestClassConstructor(ExceptionAggregator exceptionAggregator, IReflectionTypeInfo testClass, out ConstructorInfo constructor)
        {
            throw new NotImplementedException();
        }
    }
}
