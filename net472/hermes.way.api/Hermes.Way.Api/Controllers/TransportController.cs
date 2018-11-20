using Hermes.Way.Api.Services;
using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Hermes.Way.Api.Models;
using Newtonsoft.Json;
using Unity;

namespace Hermes.Way.Api.Controllers
{
    /// <inheritdoc />
    [Route("api/tr")]
    public class TransportController : WayControllerBase
    {
        [HttpGet]
        public async Task<HttpResponseMessage> Get(string id, string date)
        {
            return await Task.Run(async () =>
            {
                var config = Unity.Resolve<TrWayPointsConfig>();
                config.ServiceType = 1;
                config.Date = DateTime.Parse(date, new CultureInfo("ru-Ru"));
                config.Id = double.Parse(id);
                var service = Unity.Resolve<TrWayPointsService>();
                var res = await service.Get(config);
                HttpResponseMessage result = null;
                if (res.Exception == null)
                {                    
                    result = Request.CreateResponse(HttpStatusCode.OK, res.Result);
                    Logger.Information("request {request} return status {status}",Request.RequestUri, HttpStatusCode.OK);
                }
                else
                {
                    Request.CreateResponse(HttpStatusCode.InternalServerError, res.Exception);
                    Logger.Fatal(res.Exception, JsonConvert.SerializeObject(res.Exception, Formatting.Indented));
                }
                return result;
            }).ConfigureAwait(false);

        }
    }
}
