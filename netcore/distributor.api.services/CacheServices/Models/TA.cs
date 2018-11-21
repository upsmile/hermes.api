using Newtonsoft.Json;

namespace CacheServices.Models
{
    public class TA
    {
        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("LastEvent")]
        public string LastEvent { get; set; }

        [JsonProperty("Contract")]
        public string Contract { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Department")]
        public string Department { get; set; }

        [JsonProperty("PositionName")]
        public string PositionName { get; set; }

        [JsonProperty("TransportViewItems")]
        public TransportViewItem[] TransportViewItems { get; set; }
    }
}