using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CacheManager.Core;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace CacheServices.Services.TA
{
    public class TaCacheService : ICacheService<Models.TA, long>
    {
        private readonly ICacheManager<ConcurrentBag<Models.TA>> _cache;
        private const string TaCache = "TACache";
        private readonly ITaService _service;
        private readonly object _lock;
        private readonly IConfiguration _config;
        
        public TaCacheService(ITaService service, IConfiguration configuration)
        {
            _cache = CacheFactory.Build<ConcurrentBag<Models.TA>>("TACacheService", settings =>
            {
                settings.WithSystemRuntimeCacheHandle("TAHandle");
            });
            
            _cache.OnAdd += (sender, args) =>
            {
                Log.Logger.Information($"Data added to [{TaCache}]");
            };
            _service = service;
            _lock = new object();
            _config = configuration;
        }


        public async Task<ConcurrentBag<Models.TA>> GetValuesAsync()
        {
            var result = _cache.Get(TaCache);

            if (result != null)
            {
                Log.Logger.Information($"Result from [{TaCache}]");
                return result;
            }

            result = await FillCacheAsync();
            return result;
        }

        public async Task<ConcurrentBag<Models.TA>> FillCacheAsync()
        {
            Log.Logger.Information($"Getting data from TA service");
            var result = await _service.GetData();

            lock (_lock)
            {
                _cache.Clear();
                _cache.Add(TaCache, result);
            }

            return result;
        }

        public async Task<Models.TA> GetValueAsync(long id)
        {
            var allPoints = _cache.Get(TaCache);
            
            if (allPoints == null)
                allPoints = await FillCacheAsync();
            else
                Log.Logger.Information($"Result from [{TaCache}]");

            var result = allPoints.FirstOrDefault(p => p.Id == id);
            
            if (result== null)
                throw new Exception($"Can't find point with code {id}");

            return result;
        }

        public async Task<string> UpdateCacheAsync()
        {
            var sw = new Stopwatch();
            sw.Start();
                Log.Logger.Information($"[Update] Start of [{TaCache}] update");
               
                await FillCacheAsync();
                
            sw.Stop();
                Log.Logger.Information($"[Update] End of [{TaCache}] update in {sw.Elapsed}");

            return "Update cache for TA successfully completed";
        }
        
        public void CreateJob()
        {
            RecurringJob.AddOrUpdate("Update TA cache", () => UpdateCacheAsync(), 
                Cron.MinuteInterval(Convert.ToInt32(_config["Hangfire:UpdateTACacheTimeInMinutes"])));
        }
    }
}