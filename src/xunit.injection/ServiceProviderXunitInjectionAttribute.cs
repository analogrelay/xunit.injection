using System;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Injection
{
    public sealed class ServiceProviderXunitInjectionAttribute : Attribute, IXunitInjectionControllerAttribute
    {
        private readonly string _assemblyName;
        private readonly string _typeName;
        private readonly string _methodName;

        public ServiceProviderXunitInjectionAttribute(string assemblyName, string typeName, string methodName)
        {
            _assemblyName = assemblyName;
            _typeName = typeName;
            _methodName = methodName;
        }

        public bool TryCreateInjectionControllerForAssembly(ExceptionAggregator aggregator, IReflectionAssemblyInfo assembly, out IXunitInjectionController controller) => TryCreateInjectionControllerCore(aggregator, out controller);
        public bool TryCreateInjectionControllerForType(ExceptionAggregator aggregator, IReflectionTypeInfo type, out IXunitInjectionController controller) => TryCreateInjectionControllerCore(aggregator, out controller);

        private bool TryCreateInjectionControllerCore(ExceptionAggregator aggregator, out IXunitInjectionController controller)
        {
            var asm = Assembly.Load(_assemblyName);
            if (asm == null)
            {
                aggregator.Add(new InvalidOperationException($"Cannot load assembly: {_assemblyName}"));
                controller = null;
                return false;
            }

            var typ = asm.GetType(_typeName);
            if (typ == null)
            {
                aggregator.Add(new InvalidOperationException($"Cannot find type: {_typeName}"));
                controller = null;
                return false;
            }

            var method = typ.GetMethod(_methodName, BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, Type.EmptyTypes, new ParameterModifier[0]);
            if (method == null)
            {
                aggregator.Add(new InvalidOperationException($"Cannot find public static parameter-less method: {_methodName}"));
                controller = null;
                return false;
            }

            if (method.ReturnType != typeof(IServiceProvider))
            {
                aggregator.Add(new InvalidOperationException($"{_methodName} does not return an {nameof(IServiceProvider)}"));
                controller = null;
                return false;
            }

            // Invoke the method
            var result = method.Invoke(null, new object[0]);
            if (result == null)
            {
                aggregator.Add(new InvalidOperationException($"{_methodName} returned null"));
                controller = null;
                return false;
            }

            controller = new ServiceProviderXunitInjectionController((IServiceProvider)result);
            return true;
        }
    }
}
