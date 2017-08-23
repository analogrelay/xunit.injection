using System;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Injection
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class XunitInjectionAttribute : Attribute, IXunitInjectionControllerAttribute
    {
        private readonly string _assemblyName;
        private readonly string _typeName;

        public XunitInjectionAttribute(string assemblyName, string typeName)
        {
            _assemblyName = assemblyName;
            _typeName = typeName;
        }

        public bool TryCreateInjectionControllerForAssembly(ExceptionAggregator aggregator, IReflectionAssemblyInfo assembly, out IXunitInjectionController controller) => TryCreateInjectionControllerCore(aggregator, out controller);
        public bool TryCreateInjectionControllerForType(ExceptionAggregator aggregator, IReflectionTypeInfo type, out IXunitInjectionController controller) => TryCreateInjectionControllerCore(aggregator, out controller);

        private bool TryCreateInjectionControllerCore(ExceptionAggregator aggregator, out IXunitInjectionController controller)
        {
            var asm = Assembly.Load(_assemblyName);
            if(asm == null)
            {
                aggregator.Add(new InvalidOperationException($"Cannot load assembly: {_assemblyName}"));
                controller = null;
                return false;
            }

            var typ = asm.GetType(_typeName);
            if(typ == null)
            {
                aggregator.Add(new InvalidOperationException($"Cannot find type: {_typeName}"));
                controller = null;
                return false;
            }

            controller = (IXunitInjectionController)Activator.CreateInstance(typ);
            return true;
        }
    }
}
