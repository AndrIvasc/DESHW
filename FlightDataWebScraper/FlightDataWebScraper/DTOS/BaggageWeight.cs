using Newtonsoft.Json;

namespace FlightDataWebScraper.DTOS
{
    public class BaggageWeight
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("measurementType")]
        public MeasurementType? MeasurementType { get; set; }
    }
}
