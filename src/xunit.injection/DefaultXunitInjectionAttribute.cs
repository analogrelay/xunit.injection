using System;

namespace Xunit.Injection
{
    /// <summary>
    /// Apply this attribute to use the default Xunit constructor injection behavior. Useful for when an assembly-level attribute is changing the injection behavior.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DefaultXunitInjectionAttribute : Attribute, IXunitInjectionControllerAttribute
    {
        public IXunitInjectionController CreateInjectionController() => DefaultXunitInjectionController.Instance;
    }
}
