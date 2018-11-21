using System.Collections.Generic;
using System.Threading.Tasks;
using CacheServices.Models;
using CacheServices.Services.Helpers;
using CacheServices.Services.Points;
using Microsoft.AspNetCore.Mvc;

namespace CacheServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointsController : CacheControllerBase
    {
        private readonly PointsCacheService _service;
        private const string LogRequestMessage = "[REQUEST for points]";
        private const string LogResponseMessage = "[RESPONSE for points]";
        
        public PointsController(PointsCacheService service)
        {
            _service = service;
        }
        
        // Get all points RTT (TD)
        [HttpGet]
        public async Task<ActionResult> GetAllPoints()
        {
            const string kindLog = "all";
            var result =  await GetResponse<IEnumerable<Point>>(async () => await _service.GetValuesAsync(),
                LogRequestMessage, LogResponseMessage, kindLog);
            return result;
        }

        // Get concrete point by "Code"
        [HttpGet("{code}")]
        public async Task<ActionResult> GetPoint(long code)
        {
            var kindLog = $"with code {code}";
            var result =  await GetResponse(async () => await _service.GetValueAsync(code), 
                LogRequestMessage, LogResponseMessage, kindLog);
            return result;
        }

        // Update cache for points 
        [HttpGet]
        [Route("update")]
        public async Task<ActionResult> UpdatePointsCache()
        {          
            const string kindLog = "update";
            var result =  await GetResponse(async () => await _service.UpdateCacheAsync(),
                LogRequestMessage, LogResponseMessage, kindLog);
            return result;
        }        
    }
}
