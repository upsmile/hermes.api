using System.Diagnostics.CodeAnalysis;
using Serilog;
using Serilog.Events;
using Unity;
using Unity.Extension;
using Unity.Injection;
using Unity.Lifetime;

namespace Hermes.Way.Api.Services
{
    /// <inheritdoc />
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class LoggerContainerExtention: UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType<ILogger>(new ContainerControlledLifetimeManager(), new InjectionFactory(
                (ctr, type, name) =>
                {
                    ILogger log = new LoggerConfiguration()
                        .WriteTo.Async(x=>x.Sentry(System.Configuration.ConfigurationManager.AppSettings["sentrydsn"]))
                        .WriteTo.Async(x=>x.RollingFile("log-{Date}.log",LogEventLevel.Debug))                        
                        .CreateLogger();
                    return log;
                }));
        }
    }
}