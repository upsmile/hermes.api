using System.Collections.Concurrent;
using System.Threading.Tasks;
using CacheServices.Models;
using CacheServices.Services.Helpers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CacheServices.Services.Points
{
    public class PointsService : IPointsService
    {
        private readonly string _connectionString;
        private readonly CacheHelper _helper;
        
        public PointsService(IConfiguration configuration, CacheHelper helper)
        {
            _connectionString = configuration["CacheServices:PointsConnectionstring"];
            _helper = helper;
        }
        
        public async Task<ConcurrentBag<Point>> GetData()
        {
            // get data from service
            var result = await _helper.GetDataFromService(_connectionString);
            
            // get string value from result
            var data = JsonConvert.DeserializeObject<PointsData>(result);
            var list = data.GetDeliveryPointsResult;

            //get list of points
            var listPoints = JsonConvert.DeserializeObject<ConcurrentBag<Point>>(list);

            return listPoints;
        }
    }
}