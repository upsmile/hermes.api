using System;
using System.Threading.Tasks;
using Hermes.Way.Api.Models;

namespace Hermes.Way.Api.Services
{
    public sealed class TaWayPointsService : IWayPointsService<TaWayPointsServiceConfig, TaWayPointsResult>
    {
        public TaWayPointsService()
        {
        }

        public async Task<TaWayPointsResult> Get(TaWayPointsServiceConfig config)
        {
           return await Task<TaWayPointsResult>.Factory.StartNew(() =>
            {
                var result = new TaWayPointsResult();
                try
                {
                    var helper = new OracleDataHelper();
                    helper.EndLoadData += argument =>{
                        if(argument.Result != null)
                            result.Result = argument.Result;
                        if(argument.Exception != null)
                            result.Exception = argument.Exception;
                    };
                    helper.GetTaPoints(config.Id, config.Date, config.Date);
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
