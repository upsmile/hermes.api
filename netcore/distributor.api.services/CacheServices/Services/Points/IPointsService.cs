using System.Collections.Concurrent;
using CacheServices.Models;

namespace CacheServices.Services.Points
{
    public interface IPointsService : IService<ConcurrentBag<Point>>
    {
    }
}