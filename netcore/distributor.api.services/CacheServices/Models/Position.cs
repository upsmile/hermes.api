using Newtonsoft.Json;

namespace CacheServices.Models
{
    public class Position
    {
        [JsonProperty("Lat")]
        public double Lat { get; set; }

        [JsonProperty("Lon")]
        public double Lon { get; set; }

        [JsonProperty("Ele")]
        public long Ele { get; set; }
    }
}