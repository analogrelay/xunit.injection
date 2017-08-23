using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Injection
{
    public class XunitInjectingTestCollectionRunner : XunitTestCollectionRunner
    {
        private readonly IMessageSink _diagnosticMessageSink;

        public XunitInjectingTestCollectionRunner(ITestCollection testCollection, IEnumerable<IXunitTestCase> testCases, IMessageSink diagnosticMessageSink, IMessageBus messageBus, ITestCaseOrderer testCaseOrderer, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource) : base(testCollection, testCases, diagnosticMessageSink, messageBus, testCaseOrderer, aggregator, cancellationTokenSource)
        {
            // Base class doesn't expose this, so capture it here.
            _diagnosticMessageSink = diagnosticMessageSink;
        }

        protected override Task<RunSummary> RunTestClassAsync(ITestClass testClass, IReflectionTypeInfo @class, IEnumerable<IXunitTestCase> testCases)
            => new XunitInjectingTestClassRunner(testClass, @class, testCases, _diagnosticMessageSink, MessageBus, TestCaseOrderer, new ExceptionAggregator(Aggregator), CancellationTokenSource, CollectionFixtureMappings).RunAsync();
    }
}
