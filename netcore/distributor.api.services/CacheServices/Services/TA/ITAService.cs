using System.Collections.Concurrent;

namespace CacheServices.Services.TA
{
    public interface ITaService : IService<ConcurrentBag<Models.TA>>
    {
    }
}