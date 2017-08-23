using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Injection
{
    public interface IXunitInjectionControllerAttribute
    {
        bool TryCreateInjectionControllerForAssembly(ExceptionAggregator aggregator, IReflectionAssemblyInfo assembly, out IXunitInjectionController controller);
        bool TryCreateInjectionControllerForType(ExceptionAggregator aggregator, IReflectionTypeInfo type, out IXunitInjectionController controller);
    }
}
