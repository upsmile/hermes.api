using System;
using Newtonsoft.Json;

namespace CacheServices.Models
{
    public class Branch
    {
        [JsonProperty("Name")]
        public object Name { get; set; }

        [JsonProperty("Id")]
        public Guid Id { get; set; }
    }
}