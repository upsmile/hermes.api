using System.Collections.Generic;
using System.Threading.Tasks;
using CacheServices.Models;
using CacheServices.Services.Helpers;
using CacheServices.Services.Transports;
using Microsoft.AspNetCore.Mvc;

namespace CacheServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransportsController : CacheControllerBase
    {
        private readonly TransportsCacheService _service;
        private const string LogRequestMessage = "[REQUEST for transport]";
        private const string LogResponseMessage = "[RESPONSE for transport]";
        
        public TransportsController(TransportsCacheService service)
        {          
            _service = service;
        }
        
        // Get all transports
        [HttpGet]
        public async Task<ActionResult> GetAllTransports()
        {
            const string kindLog = "all";
            var result =  await GetResponse<IEnumerable<Transport>>(async () => await _service.GetValuesAsync(), 
                LogRequestMessage, LogResponseMessage, kindLog);
            return result;
        }

        // Get concrete transport by "Id"
        [HttpGet("{id}")]
        public async Task<ActionResult> GetTransport(long id)
        {
            var kindLog = $"with id {id}";
            var result =  await GetResponse(async () => await _service.GetValueAsync(id), 
                LogRequestMessage, LogResponseMessage, kindLog);
            return result;
        }

        // Update cache for transports 
        [HttpGet]
        [Route("update")]
        public async Task<ActionResult> UpdateTransportsCache()
        {
            const string kindLog = "update";
            var result =  await GetResponse(async () => await _service.UpdateCacheAsync(), 
                LogRequestMessage, LogResponseMessage, kindLog);
            return result;
        }
    }
}