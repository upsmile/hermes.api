using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Hermes.Way.Api.Controllers
{
    [Route(template: "api/ta")]
    public class TradeAgentWayController : ApiController
    {
        [HttpGet]
        public Task<HttpResponseMessage> Get()
        {
            throw new NotImplementedException();
        }
        [HttpGet]
        public Task<HttpResponseMessage> Get(long id)
        {
            throw new NotImplementedException();
        }
    }
}
