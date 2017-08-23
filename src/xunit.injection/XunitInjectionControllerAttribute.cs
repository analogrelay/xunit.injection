using System;
using System.Reflection;

namespace Xunit.Injection
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class XunitInjectionControllerAttribute : Attribute, IXunitInjectionControllerAttribute
    {
        private readonly string _assemblyName;
        private readonly string _typeName;

        public XunitInjectionControllerAttribute(string assemblyName, string typeName)
        {
            _assemblyName = assemblyName;
            _typeName = typeName;
        }

        public IXunitInjectionController CreateInjectionController()
        {
            var asm = Assembly.Load(_assemblyName);
            var typ = asm.GetType(_typeName);
            return (IXunitInjectionController)Activator.CreateInstance(typ);
        }
    }
}
