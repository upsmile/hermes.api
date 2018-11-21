using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CacheServices.Models;
using CacheServices.Services.Helpers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CacheServices.Services.Transports
{
    public class TransportsService : ITransportsService
    {
        private readonly string _connectionString;
        private readonly CacheHelper _helper;
        
        public TransportsService(IConfiguration configuration, CacheHelper helper)
        {
             _connectionString = 
                 $"{configuration["CacheServices:TransportConnectionString"]}/{DateTime.Today.AddDays(-5).ToOADate()}";
            _helper = helper;
        }
        
        public async Task<ConcurrentBag<Transport>> GetData()
        {
            // get data from service transports
            var result = await _helper.GetDataFromService(_connectionString, true);
                      
            // get list of transports
            var data = JsonConvert.DeserializeObject<ConcurrentBag<Transport>>(result);

            return data;
        }
    }
}