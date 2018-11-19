using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Hermes.Way.Api.Controllers
{
    [Route(template:"api/tr")]
    public class TransportWayController : ApiController
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
