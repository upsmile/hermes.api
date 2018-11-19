using Hermes.Way.Api.Services;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Unity;

namespace Hermes.Way.Api.Controllers
{
    [Route(template:"api/tr")]
    public class TransportWayController : ApiController
    {
        private readonly IUnityContainer unity;
        public TransportWayController(IUnityContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));
        }

        private TransportWayController() { }

        [HttpGet]
        public async Task<HttpResponseMessage> Get(double id, string date)
        {
            return await Task.Run(async () =>
            {
                var config = unity.Resolve<TrWayPointsConfig>();
                config.ServiceType = 1;
                config.Date = DateTime.Parse(date, new CultureInfo("ru-Ru"));
                var service = unity.Resolve<TrWayPointsService>();
                var res = await service.Get(config);
                HttpResponseMessage result = null;
                if (res.Exception == null)
                {
                    result = Request.CreateResponse(System.Net.HttpStatusCode.OK, res.Result);
                }
                else
                {
                    Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, res.Exception);
                }
                return result;
            }).ConfigureAwait(false);

        }
    }
}
