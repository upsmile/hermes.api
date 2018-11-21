using Newtonsoft.Json;

namespace CacheServices.Models
{
    public class PointsData
    {
        [JsonProperty("GetDeliveryPointsResult")]
        public string GetDeliveryPointsResult { get; set; }
    }
}