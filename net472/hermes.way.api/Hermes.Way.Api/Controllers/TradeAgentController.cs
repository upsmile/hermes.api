using Hermes.Way.Api.Services;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Hermes.Way.Api.Models;
using Unity;

namespace Hermes.Way.Api.Controllers
{
    [Route("api/ta")]
    public class TradeAgentController : WayControllerBase
    {        
        
        [HttpGet]
        public async Task<HttpResponseMessage> Get(string id, string date)
        {
            return await Task.Run(async () =>
            {
                var config = Unity.Resolve<TaWayPointsServiceConfig>();
                config.ServiceType = 2;
                // todo: не выполняется валидация и контроль параметров 
                config.Date = DateTime.Parse(date, new CultureInfo("ru-Ru"));
                config.Id = double.Parse(id);
                var service = Unity.Resolve<TaWayPointsService>();
                Logger.Information("begin request {0}", Request.RequestUri.ToString());
                var res =  await service.Get(config);
                HttpResponseMessage result = null;
                if(res.Exception == null)
                {                    
                    result = Request.CreateResponse(System.Net.HttpStatusCode.OK, res.Result);
                    Logger.Information("result  staus request {request} is {status}", Request.RequestUri.ToString(), System.Net.HttpStatusCode.OK);
                }
                else
                {                    
                    result = Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, res.Exception);
                    Logger.Fatal(res.Exception, $"{Request.RequestUri}{Environment.NewLine}{res.Exception.Message}");
                }                
                return result;

            }).ConfigureAwait(false);            
        }
    }
}
