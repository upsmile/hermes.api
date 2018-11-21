using System;

namespace CacheServices.Models
{
    public interface IPointCacheKey
    {
        double Id { get; set; }
        
        DateTime Date { get; set; }
    }
}