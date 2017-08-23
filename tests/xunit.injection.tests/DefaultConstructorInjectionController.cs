using System;
using System.Linq;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Injection.Tests
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class DefaultConstructorInjectionControllerAttribute : Attribute, IXunitInjectionControllerAttribute
    {
        public bool TryCreateInjectionControllerForAssembly(ExceptionAggregator aggregator, IReflectionAssemblyInfo assembly, out IXunitInjectionController controller)
        {
            controller = new DefaultConstructorInjectionController();
            return true;
        }

        public bool TryCreateInjectionControllerForType(ExceptionAggregator aggregator, IReflectionTypeInfo type, out IXunitInjectionController controller)
        {
            controller = new DefaultConstructorInjectionController();
            return true;
        }
    }

    public class DefaultConstructorInjectionController : IXunitInjectionController
    {
        public bool TrySelectTestClassConstructor(ExceptionAggregator exceptionAggregator, IReflectionTypeInfo testClass, out ConstructorInfo constructor)
        {
            if (TryFindConstructor(testClass.Type, out constructor))
            {
                return true;
            }

            return false;
        }

        public bool TryGetConstructorArgument(ExceptionAggregator exceptionAggregator, ConstructorInfo constructor, int index, ParameterInfo parameter, out object argumentValue)
        {
            // Try to construct an instance of the thing
            if (TryFindConstructor(parameter.ParameterType, out var ctor))
            {
                if (ctor.GetParameters().Length == 0)
                {
                    argumentValue = ctor.Invoke(new object[0]);
                    return true;
                }
            }

            argumentValue = null;
            return false;
        }

        private bool TryFindConstructor(Type type, out ConstructorInfo ctor)
        {
            var ctors = type.GetTypeInfo()
                                  .DeclaredConstructors
                                  .Where(ci => !ci.IsStatic && ci.IsPublic)
                                  .ToList();

            if (ctors.Count == 1)
            {
                ctor = ctors[0];
                return true;
            }
            else
            {
                ctor = null;
                return false;
            }
        }
    }
}
