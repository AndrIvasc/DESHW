using Newtonsoft.Json;

namespace FlightDataWebScraper.DTOS
{
    public class Flight
    {
        [JsonProperty("number")]
        public string? Number { get; set; }

        [JsonProperty("airportDeparture")]
        public AirportDeparture? AirportDeparture { get; set; }

        [JsonProperty("airportArrival")]
        public AirportArrival? AirportArrival { get; set; }

        [JsonProperty("dateDeparture")]
        public string? DateDeparture { get; set; }

        [JsonProperty("dateArrival")]
        public string? DateArrival { get; set; }

        [JsonProperty("gmtDateDeparture")]
        public string? GmtDateDeparture { get; set; }

        [JsonProperty("gmtDateArrival")]
        public string? GmtDateArrival { get; set; }

        [JsonProperty("companyCode")]
        public string? CompanyCode { get; set; }

        [JsonProperty("operator")]
        public string? Operator { get; set; }

        [JsonProperty("flote")]
        public Flote? Flote { get; set; }

        [JsonProperty("technicalStop")]
        public TechnicalStop? TechnicalStop { get; set; }

        [JsonProperty("terminalDeparture")]
        public string? TerminalDeparture { get; set; }

        [JsonProperty("terminalArrival")]
        public string? TerminalArrival { get; set; }

        [JsonProperty("cabinClass")]
        public CabinClass? CabinClass { get; set; }
    }
}
