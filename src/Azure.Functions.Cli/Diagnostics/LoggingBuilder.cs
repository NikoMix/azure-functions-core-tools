using Azure.Functions.Cli.Common;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.Azure.WebJobs.Script;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Azure.Functions.Cli.Diagnostics
{
    internal class LoggingBuilder : IConfigureBuilder<ILoggingBuilder>
    {
        private bool _hostJsonDefaultLogLevelNone;

        public LoggingBuilder(bool hostJsonDefaultLogLevelNone)
        {
            _hostJsonDefaultLogLevelNone = hostJsonDefaultLogLevelNone;
        }

        public void Configure(ILoggingBuilder builder)
        {
            if (_hostJsonDefaultLogLevelNone)
            {
                builder.AddProvider(new ColoredConsoleLoggerProvider(LogLevel.None)).AddFilter((cat, level) => true);
            }
            else
            {
                builder.AddProvider(new ColoredConsoleLoggerProvider());
            }

            builder.Services.AddSingleton<TelemetryClient>(provider =>
            {
                TelemetryConfiguration configuration = provider.GetService<TelemetryConfiguration>();
                TelemetryClient client = new TelemetryClient(configuration);

                client.Context.GetInternalContext().SdkVersion = $"azurefunctionscoretools: {Constants.CliVersion}";

                return client;
            });
        }
    }
}
