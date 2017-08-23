using System;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Injection
{
    /// <summary>
    /// Apply this attribute to use the default Xunit constructor injection behavior. Useful for when an assembly-level attribute is changing the injection behavior.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DefaultXunitInjectionAttribute : Attribute, IXunitInjectionControllerAttribute
    {
        public bool TryCreateInjectionControllerForAssembly(ExceptionAggregator aggregator, IReflectionAssemblyInfo assembly, out IXunitInjectionController controller)
        {
            controller = DefaultXunitInjectionController.Instance;
            return true;
        }

        public bool TryCreateInjectionControllerForType(ExceptionAggregator aggregator, IReflectionTypeInfo type, out IXunitInjectionController controller)
        {
            controller = DefaultXunitInjectionController.Instance;
            return true;
        }
    }
}
