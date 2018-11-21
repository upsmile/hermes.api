using System.Threading.Tasks;
using CacheServices.Services.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CacheServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : CacheControllerBase
    {       
        // App version
        [HttpGet]
        [Route("version")]
        public async Task<ActionResult> GetVersion()
        {
            const string requestLog = "[REQUEST] version";
            var meth = Task.Run(() => typeof(Startup).Assembly.GetName().Version.ToString());
            var result =  await GetResponse(async () => await meth, requestLog);
            return result;
        }        
    }
}
