using Newtonsoft.Json;

namespace CacheServices.Models
{
    public class TransportData
    {
        [JsonProperty("Id")]
        public string Data { get; set; }
    }

    public class Transport
    {
        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("LastEvent")]
        public string LastEvent { get; set; }

        [JsonProperty("TypeCar")]
        public string TypeCar { get; set; }

        [JsonProperty("Mark")]
        public string Mark { get; set; }

        [JsonProperty("Number")]
        public string Number { get; set; }

        [JsonProperty("TemperatureMode")]
        public object TemperatureMode { get; set; }

        [JsonProperty("TransportViewItems")]
        public TransportViewItem[] TransportViewItems { get; set; }
    }

    public class TransportViewItem
    {
        [JsonProperty("Date")]
        public string Date { get; set; }

        [JsonProperty("Duration")]
        public object Duration { get; set; }

        [JsonProperty("Distance")]
        public double Distance { get; set; }
    }    
}