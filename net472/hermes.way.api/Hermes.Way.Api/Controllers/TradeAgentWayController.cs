using Hermes.Way.Api.Services;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Unity;

namespace Hermes.Way.Api.Controllers
{
    [Route(template: "api/ta")]
    public class TradeAgentWayController : ApiController
    {
        private readonly IUnityContainer unity;
        public TradeAgentWayController(IUnityContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));
        }

        private TradeAgentWayController() { }
        
        [HttpGet]
        public async Task<HttpResponseMessage> Get(double id, string date)
        {
            return await Task.Run(async () =>
            {
                var config = unity.Resolve<TaWayPointsServiceConfig>();
                config.ServiceType = 2;
                config.Date = DateTime.Parse(date, new CultureInfo("ru-Ru"));
                var service = unity.Resolve<TaWayPointsService>();
                var res =  await service.Get(config);
                var result = new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK
                };
                return result;
            }).ConfigureAwait(false);
            
        }
    }
}
