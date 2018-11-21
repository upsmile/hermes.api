using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CacheServices.Services.Helpers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CacheServices.Services.TA
{
    public class TAService : ITaService
    {
        private readonly string _connectionString;
        private readonly CacheHelper _helper;
        
        public TAService(IConfiguration configuration, CacheHelper helper)
        {
            _connectionString = 
                $"{configuration["CacheServices:TAConectionString"]}/{DateTime.Today.AddDays(-5).ToOADate()}";
            _helper = helper;
        }
        
        public async Task<ConcurrentBag<Models.TA>> GetData()
        {
            // get data from service
            var result = await _helper.GetDataFromService(_connectionString, true);
            
            // get TA from result
            var data = JsonConvert.DeserializeObject<ConcurrentBag<Models.TA>>(result);

            return data;
        }
    }
}