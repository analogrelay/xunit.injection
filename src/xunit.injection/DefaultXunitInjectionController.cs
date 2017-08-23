using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Injection
{
    public class DefaultXunitInjectionController : IXunitInjectionController
    {
        public static readonly DefaultXunitInjectionController Instance = new DefaultXunitInjectionController();

        private DefaultXunitInjectionController() { }

        public virtual bool TrySelectTestClassConstructor(ExceptionAggregator exceptionAggregator, IReflectionTypeInfo testClass, out ConstructorInfo constructor)
        {
            constructor = null;
            return false;
        }

        public virtual bool TryGetConstructorArgument(ExceptionAggregator exceptionAggregator, ConstructorInfo constructor, int index, ParameterInfo parameter, out object argumentValue)
        {
            argumentValue = null;
            return false;
        }
    }
}
