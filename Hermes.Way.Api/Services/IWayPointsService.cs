using System.Threading.Tasks;

namespace Hermes.Way.Api.Services
{

    /// <summary>
    /// Сервис получения данных о точках маршрутов транспортного средства  
    /// </summary>
    /// <typeparam name="TConfig"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IWayPointsService<in TConfig, TResult>
        where TResult: IWayPointsResult
        where TConfig: IWayPointsServiceConfig
    {
        Task<TResult> Get(TConfig config);
    }
}
