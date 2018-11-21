using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using CacheServices.Services.Points;
using CacheServices.Services.TA;
using CacheServices.Services.Transports;
using Serilog;

namespace CacheServices.Services.Helpers
{

    public class CacheHelper
    {
        private readonly IServiceProvider _provider;
        
        public CacheHelper(IServiceProvider serviceProvider)
        {
            _provider = serviceProvider;
        }

        public void FillCaches()
        {
            var pointsService = (PointsCacheService)_provider.GetService(typeof(PointsCacheService));
            pointsService.CreateJob();
            Log.Logger.Information(CreateLog("PointsCache"));
            
            var transportService = (TransportsCacheService)_provider.GetService(typeof(TransportsCacheService));
            transportService.CreateJob();
            Log.Logger.Information(CreateLog("TransportsCache"));
            
            var taService = (TaCacheService)_provider.GetService(typeof(TaCacheService));
            taService.CreateJob();
            Log.Logger.Information(CreateLog("TACache"));
        }

        private string CreateLog(string jobName)
        {
            var result = $"Job for {jobName} update created";
            return result;
        }

        public async Task<string> GetDataFromService(string connectionString, bool removeEscapeCharacters = false)
        {
            Log.Logger.Information($"Call service on {connectionString}");
            var client = new HttpClient();
            var response = await client.GetAsync(connectionString);
            string result;
            var response2 = await response.Content.ReadAsStreamAsync();
            using (var reader = new StreamReader(response2))
            {
                result = reader.ReadToEnd();
            }
            if (!removeEscapeCharacters) return result;
            result = result.RemoveEscapeCharactersAndQuotes();
            return result;
        }
        
    }
}