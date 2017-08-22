using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Injection
{
    public class XunitInjectingTestClassRunner : XunitTestClassRunner
    {
        public XunitInjectingTestClassRunner(ITestClass testClass, IReflectionTypeInfo @class, IEnumerable<IXunitTestCase> testCases, IMessageSink diagnosticMessageSink, IMessageBus messageBus, ITestCaseOrderer testCaseOrderer, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource, IDictionary<Type, object> collectionFixtureMappings) : base(testClass, @class, testCases, diagnosticMessageSink, messageBus, testCaseOrderer, aggregator, cancellationTokenSource, collectionFixtureMappings)
        {
        }

        protected override ConstructorInfo SelectTestClassConstructor()
        {
            return base.SelectTestClassConstructor();
        }

        protected override bool TryGetConstructorArgument(ConstructorInfo constructor, int index, ParameterInfo parameter, out object argumentValue)
        {
            return base.TryGetConstructorArgument(constructor, index, parameter, out argumentValue);
        }
    }
}
