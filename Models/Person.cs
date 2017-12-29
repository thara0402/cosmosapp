using Newtonsoft.Json;

namespace cosmosapp.Models
{
    public class Person : DocumentBase
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

}