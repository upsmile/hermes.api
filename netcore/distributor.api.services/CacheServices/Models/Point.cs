using Newtonsoft.Json;

namespace CacheServices.Models
{
    public class Point
    {
        [JsonProperty("Position")]
        public Position Position { get; set; }

        [JsonProperty("Header")]
        public Header Header { get; set; }
    }
}