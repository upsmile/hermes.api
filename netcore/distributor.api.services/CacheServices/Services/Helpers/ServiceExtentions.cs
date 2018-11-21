using CacheServices.Services.Points;
using CacheServices.Services.TA;
using CacheServices.Services.Transports;
using Microsoft.Extensions.DependencyInjection;

namespace CacheServices.Services.Helpers
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterCacheServices(
            this IServiceCollection services)
        {
            services.AddSingleton<IPointsService, PointsService>();
            services.AddSingleton<ITransportsService, TransportsService>();
            services.AddSingleton<ITaService, TAService>();
            services.AddSingleton<PointsCacheService>();
            services.AddSingleton<TransportsCacheService>();
            services.AddSingleton<TaCacheService>();
            services.AddSingleton<CacheHelper>();
                       
            return services;
        }
    }
}