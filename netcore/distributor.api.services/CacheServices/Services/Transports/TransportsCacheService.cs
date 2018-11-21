using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CacheManager.Core;
using CacheServices.Models;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace CacheServices.Services.Transports
{
    public class TransportsCacheService : ICacheService<Transport, long>
    {
        private readonly ICacheManager<ConcurrentBag<Transport>> _cache;
        private const string TransportsCache = "TransportsCache";
        private readonly ITransportsService _service;
        private readonly object _lock;
        private readonly IConfiguration _config;
        
        public TransportsCacheService(ITransportsService service, IConfiguration configuration)
        {
            _cache = CacheFactory.Build<ConcurrentBag<Transport>>("transportsCacheService", settings =>
            {
                settings.WithSystemRuntimeCacheHandle("transportsHandle");
            });
            
            _cache.OnAdd += (sender, args) =>
            {
                Log.Logger.Information($"Data added to [{TransportsCache}]");
            };
            
            _service = service;
            _lock = new object();
            _config = configuration;           
        }


        public async Task<ConcurrentBag<Transport>> GetValuesAsync()
        {
            var result = _cache.Get(TransportsCache);

            if (result != null)
            {
                Log.Logger.Information($"Result from [{TransportsCache}]");
                return result;
            }

            result = await FillCacheAsync();
            return result;
        }

        public async Task<ConcurrentBag<Transport>> FillCacheAsync()
        {
            Log.Logger.Information($"Getting data from transports service");
            var result = await _service.GetData();
            
            lock (_lock)
            {
                _cache.Clear();
                _cache.Add(TransportsCache, result);
            }

            return result;
        }

        public async Task<Transport> GetValueAsync(long id)
        {
            var allPoints = _cache.Get(TransportsCache);
            
            if (allPoints == null)
                allPoints = await FillCacheAsync();
            else
                Log.Logger.Information($"Result from [{TransportsCache}]");

            var result = allPoints.FirstOrDefault(p => p.Id == id);
            
            if (result== null)
                throw new Exception($"Can't find transport with id {id}");

            return result;
        }

        public async Task<string> UpdateCacheAsync()
        {
            var sw = new Stopwatch();
            sw.Start();
            Log.Logger.Information($"[Update] Start of [{TransportsCache}] update");
               
            await FillCacheAsync();
                
            sw.Stop();
            Log.Logger.Information($"[Update] End of [{TransportsCache}] update in {sw.Elapsed}");

            return "Update cache for TA successfully completed";
        }

        public void CreateJob()
        {
            RecurringJob.AddOrUpdate("Update transports cache", () => UpdateCacheAsync(), 
                Cron.MinuteInterval(Convert.ToInt32(_config["Hangfire:UpdateTransportsCacheTimeInMinutes"])));
        }
    }
}