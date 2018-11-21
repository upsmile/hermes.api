using System.Threading.Tasks;

namespace CacheServices.Services
{
    public interface IService<T>
    {
        Task<T> GetData();
    }
}