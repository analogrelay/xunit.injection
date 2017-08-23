using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Injection
{
    public interface IXunitInjectionController
    {
        bool TryGetConstructorArgument(ExceptionAggregator exceptionAggregator, ConstructorInfo constructor, int index, ParameterInfo parameter, out object argumentValue);
        bool TrySelectTestClassConstructor(ExceptionAggregator exceptionAggregator, IReflectionTypeInfo testClass, out ConstructorInfo constructor);
    }
}
