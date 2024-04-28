using Newtonsoft.Json;

namespace FlightDataWebScraper.DTOS
{
    public class TechnicalStop
    {
        [JsonProperty("numberStops")]
        public int NumberStops { get; set; }

        [JsonProperty("airportStops")]
        public List<object>? AirportStops { get; set; }
    }
}
