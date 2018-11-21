using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using CacheServices.Models;

namespace CacheServices.Services.Transports
{
    /// <inheritdoc />
    public sealed class TrWayPointsCacheService:ICacheService<List<double>,IPointCacheKey>
    {
        public Task<ConcurrentBag<List<double>>> GetValuesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ConcurrentBag<List<double>>> FillCacheAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<double>> GetValueAsync(IPointCacheKey identificator)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateCacheAsync()
        {
            throw new NotImplementedException();
        }

        public void CreateJob()
        {
            throw new NotImplementedException();
        }
    }
}