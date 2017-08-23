using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Injection.AspNet
{
    // Only supports Assembly target, for now.
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
    public class AspNetXunitInjectionAttribute : Attribute, IXunitInjectionControllerAttribute
    {
        private readonly string _assemblyName;
        private readonly string _typeName;

        public AspNetXunitInjectionAttribute() { }

        public AspNetXunitInjectionAttribute(string typeName) : this(assemblyName: null, typeName: typeName)
        {
        }

        public AspNetXunitInjectionAttribute(string assemblyName, string typeName)
        {
            _assemblyName = assemblyName;
            _typeName = typeName;
        }

        public bool TryCreateInjectionControllerForAssembly(ExceptionAggregator aggregator, IReflectionAssemblyInfo assembly, out IXunitInjectionController controller)
        {
            if (TryFindStartupType(assembly, out var type))
            {
                // TODO: Use generic host?

                // Check for a ConfigureServices method
                var configureServicesMethod = type.GetMethod("ConfigureServices", BindingFlags.Public | BindingFlags.Instance, Type.DefaultBinder, new[] { typeof(IServiceCollection) }, new ParameterModifier[0]);
                if (configureServicesMethod == null)
                {
                    aggregator.Add(new InvalidOperationException($"Could not find appropriate ConfigureServices method on TestStartup type {type.FullName}"));
                    controller = null;
                    return false;
                }

                // Create the service collection
                var services = new ServiceCollection();

                try
                {
                    // Activate the type
                    var startup = Activator.CreateInstance(type);

                    // Configure the services
                    configureServicesMethod.Invoke(startup, new object[] { services });
                }
                catch (TargetInvocationException tex)
                {
                    aggregator.Add(tex.InnerException);
                    controller = null;
                    return false;
                }
                catch (Exception ex)
                {
                    aggregator.Add(ex);
                    controller = null;
                    return false;
                }

                // Create a ServiceProvider-based controller from the service provider
                controller = new ServiceProviderXunitInjectionController(services.BuildServiceProvider());
                return true;
            }

            aggregator.Add(new InvalidOperationException("Could not find TestStartup class in target assembly"));
            controller = null;
            return false;
        }

        public bool TryCreateInjectionControllerForType(ExceptionAggregator aggregator, IReflectionTypeInfo type, out IXunitInjectionController controller)
        {
            controller = null;
            return false;
        }

        private bool TryFindStartupType(IReflectionAssemblyInfo assembly, out Type type)
        {
            if (!string.IsNullOrEmpty(_typeName))
            {
                var asm = string.IsNullOrEmpty(_assemblyName) ? assembly.Assembly : Assembly.Load(_assemblyName);
                if (asm == null)
                {
                    throw new InvalidOperationException($"Could not load Assembly {_assemblyName}");
                }

                type = asm.GetType(_typeName);
                return type != null;
            }

            // Find the TestStartup class
            foreach (var typ in assembly.GetTypes(includePrivateTypes: false))
            {
                if (typ.ToRuntimeType().Name.Equals("TestStartup"))
                {
                    // Found a match, use it.
                    type = typ.ToRuntimeType();
                    return true;
                }
            }

            type = null;
            return false;
        }
    }
}
