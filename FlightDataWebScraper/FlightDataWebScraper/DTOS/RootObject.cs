using Newtonsoft.Json;

namespace FlightDataWebScraper.DTOS
{
    public class RootObject
    {
        [JsonProperty("header")]
        public Header? Header { get; set; }

        [JsonProperty("body")]
        public Body? Body { get; set; }
    }
}
