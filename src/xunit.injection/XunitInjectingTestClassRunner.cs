using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Injection
{
    public class XunitInjectingTestClassRunner : XunitTestClassRunner
    {
        private IXunitInjectionController _injectionController;
        protected IXunitInjectionController InjectionController
        {
            get
            {
                if (_injectionController == null)
                {
                    _injectionController = CreateInjectionController();
                }
                return _injectionController;
            }
        }

        public XunitInjectingTestClassRunner(ITestClass testClass, IReflectionTypeInfo @class, IEnumerable<IXunitTestCase> testCases, IMessageSink diagnosticMessageSink, IMessageBus messageBus, ITestCaseOrderer testCaseOrderer, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource, IDictionary<Type, object> collectionFixtureMappings) : base(testClass, @class, testCases, diagnosticMessageSink, messageBus, testCaseOrderer, aggregator, cancellationTokenSource, collectionFixtureMappings)
        {
        }

        protected override ConstructorInfo SelectTestClassConstructor()
        {
            if (InjectionController.TrySelectTestClassConstructor(Aggregator, Class, out var ctor))
            {
                return ctor;
            }
            return base.SelectTestClassConstructor();
        }

        protected override bool TryGetConstructorArgument(ConstructorInfo constructor, int index, ParameterInfo parameter, out object argumentValue)
        {
            return InjectionController.TryGetConstructorArgument(Aggregator, constructor, index, parameter, out argumentValue) ||
                base.TryGetConstructorArgument(constructor, index, parameter, out argumentValue);
        }

        protected virtual IXunitInjectionController CreateInjectionController()
        {
            // Check the class and assembly for an IXunitInjectionControllerAttribute
            if (TryCreateController(Class.Type.GetCustomAttributes(), out var controller))
            {
                return controller;
            }

            if (Class.Assembly is IReflectionAssemblyInfo reflectionAssembly)
            {
                if (TryCreateController(reflectionAssembly.Assembly.GetCustomAttributes(), out controller))
                {
                    return controller;
                }
            }

            return DefaultXunitInjectionController.Instance;
        }

        private bool TryCreateController(IEnumerable<Attribute> attributes, out IXunitInjectionController controller)
        {
            foreach (var attribute in attributes)
            {
                if (attribute is IXunitInjectionControllerAttribute controllerAttribute)
                {
                    controller = controllerAttribute.CreateInjectionController();
                    return true;
                }
            }
            controller = null;
            return false;
        }
    }
}
