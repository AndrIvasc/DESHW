using Newtonsoft.Json;

namespace FlightDataWebScraper.DTOS
{
    public class Header
    {
        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("error")]
        public bool Error { get; set; }

        [JsonProperty("bodyType")]
        public string? BodyType { get; set; }
    }
}
