using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

[assembly: Xunit.Injection.AspNet.AspNetXunitInjection]

namespace Xunit.Injection.AspNet.Tests
{
    public class TestStartup
    {
        // Not the best...
        public static readonly StringWriter GlobalLogWriter = new StringWriter();

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                var logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.TextWriter(GlobalLogWriter, outputTemplate: "[{Level}] {Message}")
                    .CreateLogger();
                builder.AddSerilog(logger);
            });
        }
    }
}
