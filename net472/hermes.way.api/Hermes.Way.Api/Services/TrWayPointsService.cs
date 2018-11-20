using System;
using System.Threading.Tasks;
using Hermes.Way.Api.Models;

namespace Hermes.Way.Api.Services
{
    public sealed class TrWayPointsService : IWayPointsService<TrWayPointsConfig, TrWayPointsResult>
    {
        public async Task<TrWayPointsResult> Get(TrWayPointsConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            var task = Task<TrWayPointsResult>.Factory.StartNew(() => {
                var result = new TrWayPointsResult();
                var helper = new OracleDataHelper();
                try
                {
                    result.Result = OracleDataHelper.GetDeliveryVehiclesPoint(config.Date, config.Id);
                }
                catch (Exception exception)
                {
                    result.Exception = exception;
                }
                return result;
            }).ConfigureAwait(false);
            return await task;            
        }
    }
}
