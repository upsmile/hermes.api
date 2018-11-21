using System.Collections.Concurrent;
using CacheServices.Models;

namespace CacheServices.Services.Transports
{
    public interface ITransportsService : IService<ConcurrentBag<Transport>>
    {
    }
}