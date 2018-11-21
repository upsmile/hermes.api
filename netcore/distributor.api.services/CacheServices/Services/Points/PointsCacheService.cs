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

namespace CacheServices.Services.Points
{
    public class PointsCacheService : ICacheService<Point, long>
    {
        private readonly ICacheManager<ConcurrentBag<Point>> _cache;
        private const string PointsCache = "PointsCache";
        private readonly IPointsService _service;
        private readonly object _lock;
        private readonly IConfiguration _config;
        
        public PointsCacheService(IPointsService service, IConfiguration configuration)
        {
            _cache = CacheFactory.Build<ConcurrentBag<Point>>("pointCacheService", settings =>
            {
                settings.WithSystemRuntimeCacheHandle("pointsHandle");
            });
            
            _cache.OnAdd += (sender, args) =>
            {
                Log.Logger.Information($"Data added to [{PointsCache}]");
            };
            _service = service;
            _lock = new object();
            _config = configuration;
        }

        public async Task<ConcurrentBag<Point>> GetValuesAsync()
        {
            var result = _cache.Get(PointsCache);

            if (result != null)
            {
                Log.Logger.Information($"Result from [{PointsCache}]");
                return result;
            }

            result = await FillCacheAsync();

            return result;
        }

        public async Task<ConcurrentBag<Point>> FillCacheAsync()
        {
            Log.Logger.Information($"Getting data from points service");
            var result = await _service.GetData();
            
            lock (_lock)
            {
                _cache.Clear();
                _cache.Add(PointsCache, result);
            }
            
            return result;
        }

        public async Task<Point> GetValueAsync(long code)
        {
            var allPoints = _cache.Get(PointsCache);
            
            if (allPoints == null)
                allPoints = await FillCacheAsync();
            else
                Log.Logger.Information($"Result from [{PointsCache}]");

            var result = allPoints.FirstOrDefault(p => p.Header?.Code == code);
            
            if (result == null)
                throw new Exception($"Can't find point with code {code}");

            return result;
        }

        public async Task<string> UpdateCacheAsync()
        {
            var sw = new Stopwatch();
            sw.Start();
            Log.Logger.Information($"[Update] Start of [{PointsCache}] update");
               
            await FillCacheAsync();
                
            sw.Stop();
            Log.Logger.Information($"[Update] End of [{PointsCache}] update in {sw.Elapsed}");

            return "Update cache for points successfully completed";
        }
        
        public void CreateJob()
        {
            RecurringJob.AddOrUpdate("Update points cache", () => UpdateCacheAsync(), 
                Cron.MinuteInterval(Convert.ToInt32(_config["Hangfire:UpdatePointsCacheTimeInMinutes"])));
        }
    }
}