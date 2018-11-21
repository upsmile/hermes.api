using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CacheServices.Services
{
    public interface ICacheService<T, in TP>
    {
        Task<ConcurrentBag<T>> GetValuesAsync();
        
        Task<ConcurrentBag<T>> FillCacheAsync();
        
        Task<T> GetValueAsync(TP identificator);
        
        Task<string> UpdateCacheAsync();
        
        void CreateJob();
    }
}