using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CacheServices.Services.Helpers
{
    public class CacheControllerBase : ControllerBase
    {
        public async Task<ActionResult> GetResponse<T>(Func<Task<T>> loadDataMethod, string startLog = null, 
            string endLog = null, string dataKindLog = null)
        {            
            var sw = new Stopwatch();
            sw.Start();
            if (!string.IsNullOrEmpty(startLog))
                Log.Logger.Information($"{startLog} {dataKindLog}");
            T result;

            try
            {
                result = await Task.Run(loadDataMethod);
            }
            catch (Exception ex)
            {
                sw.Stop();
                if (!string.IsNullOrEmpty(endLog))
                    Log.Logger.Information($"{endLog} {dataKindLog} in {sw.Elapsed}");
                Log.Logger.Error(ex.Message);

                return StatusCode(500, ex.Message);
            }

            sw.Stop();
            if (!string.IsNullOrEmpty(endLog))
                Log.Logger.Information($"{endLog} {dataKindLog} in {sw.Elapsed}");
            return Ok(result);
        }
    }
}