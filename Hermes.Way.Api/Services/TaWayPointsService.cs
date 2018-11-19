using System;
using System.Threading.Tasks;

namespace Hermes.Way.Api.Services
{
    public sealed class TaWayPointsService : IWayPointsService<TaWayPointsServiceConfig, TaWayPointsResult>
    {
        public async Task<TaWayPointsResult> Get(TaWayPointsServiceConfig config)
        {
           return await Task<TaWayPointsResult>.Factory.StartNew(() =>
            {
                var result = new TaWayPointsResult();
                try
                {
                    result.Result = new OracleDataHelper().GetDeliveryVehiclesPoint(config.Date, config.Id);
                }
                catch (Exception exception)
                {
                    result.Exception = exception;
                }
                return result;                
            }).ConfigureAwait(false);
        }
    }
}
