using System.Collections.Generic;
using System.Threading.Tasks;
using CacheServices.Models;
using CacheServices.Services.Helpers;
using CacheServices.Services.TA;
using Microsoft.AspNetCore.Mvc;

namespace CacheServices.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class TaController : CacheControllerBase
    {
        private readonly TaCacheService _service;
        private const string LogRequestMessage = "[REQUEST for TA]";
        private const string LogResponseMessage = "[RESPONSE for TA]";
        
        public TaController(TaCacheService service)
        {
            _service = service;  
        }
        
        // Get all TA
        [HttpGet]
        public async Task<ActionResult> GetAllTa()
        {
            const string kindLog = "all";
            var result =  await GetResponse<IEnumerable<TA>>(async () => await _service.GetValuesAsync(),
                LogRequestMessage, LogResponseMessage, kindLog);
            return result;
        }

        // Get concrete TA by "Id"
        [HttpGet("{id}")]
        public async Task<ActionResult> GetTa(long id)
        {
            var kindLog = $"with id {id}";
            var result =  await GetResponse(async () => await _service.GetValueAsync(id),
                LogRequestMessage, LogResponseMessage, kindLog);
            return result;
        }

        // Update cache for TA 
        [HttpGet]
        [Route("update")]
        public async Task<ActionResult> UpdateTaCache()
        {
            const string kindLog = "update";
            var result =  await GetResponse(async () => await _service.UpdateCacheAsync(), 
                LogRequestMessage, LogResponseMessage, kindLog);
            return result;
        }
    }
}