using System.Formats.Asn1;
using System.Globalization;
using System.Net.Http;
using System.Security.Authentication.ExtendedProtection;
using CsvHelper;
using CsvHelper.Configuration;
using FlightDataWebScraper.Services;
using Microsoft.Extensions.DependencyInjection;
using FlightDataWebScraper.DTOS;
using System.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        var url = "http://homeworktask.infare.lt/search.php?from=MAD&to=FUE&depart=2024-05-09&return=2024-05-16";
        var filePath = @"C:\Users\andre\Desktop\FlightData.csv";

        var serviceProvider = new ServiceCollection()
            .AddSingleton<HttpClient>()
            .AddSingleton<IWebScraperHttp, WebScraperHttp>()
            .AddSingleton<CSVService>()
            .BuildServiceProvider();

        

        using HttpClient client = new HttpClient();

        IWebScraperHttp scraper = serviceProvider.GetService<IWebScraperHttp>()!;
        CSVService csvService = serviceProvider.GetService<CSVService>()!;

        RootObject jsonData = await scraper.FetchDataAsync(url);

        try
        {
            var outboundFlights = FilterFlights(jsonData, "MAD", "AUH");
            var inboundFlights = FilterFlights(jsonData, "AUH", "MAD");

            var totalAvailabilities = jsonData.Body.Data.TotalAvailabilities;
            var journeys = jsonData.Body.Data.Journeys;

            // Group outbound and inbound flights by price category
            var groupedOutboundFlights = GroupFlightsByPriceCategory(outboundFlights, journeys, totalAvailabilities);
            var groupedInboundFlights = GroupFlightsByPriceCategory(inboundFlights, journeys, totalAvailabilities);

            var roundtripFlightCombinations = MakeRoundtripCombinations(groupedOutboundFlights, groupedInboundFlights);

            var cheapestOptions = FindCheapestOptions(roundtripFlightCombinations, journeys, totalAvailabilities);

            csvService.SaveToCsv(cheapestOptions, filePath);

            Console.WriteLine("Data saved to flight_prices_with_taxes.csv successfully.");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static List<Flight> FilterFlights(RootObject jsonData, string departureAirport, string arrivalAirport)
    {
        var journeys = jsonData.Body.Data.Journeys;
        var filteredFlights = new List<Flight>();

        foreach (var journey in journeys)
        {
            foreach (var flight in journey.Flights)
            {
                var departureCode = flight.AirportDeparture.Code;
                var arrivalCode = flight.AirportArrival.Code;

                if (departureCode == departureAirport || arrivalCode == arrivalAirport)
                {
                    filteredFlights.Add(flight);
                }
            }
        }

        return filteredFlights;
    }

    public static List<Flight> FilterFlights(RootObject jsonData, string departureAirport, string arrivalAirport, string selectedDateString)
    {
        var journeys = jsonData.Body.Data.Journeys;
        var filteredFlights = new List<Flight>();

        foreach (var journey in journeys)
        {
            foreach (var flight in journey.Flights)
            {
                var departureCode = flight.AirportDeparture.Code;
                var arrivalCode = flight.AirportArrival.Code;
                var flightDepartureDate = flight.DateDeparture;

                if (departureCode == departureAirport || arrivalCode == arrivalAirport || flightDepartureDate == flightDepartureDate)
                {
                    flight.RecommendationId = journey.RecommendationId;
                    filteredFlights.Add(flight);
                }
            }
        }

        return filteredFlights;
    }

    public static List<List<Flight>> GroupFlightsByPriceCategory(List<Flight> flights, List<Journey> journeys, List<TotalAvailability> totalAvailabilities)
    {
        var recommendationIdToTotalMap = totalAvailabilities.ToDictionary(avail => avail.RecommendationId, avail => avail.Total);

        var journeyIdToRecommendationIdMap = journeys.ToDictionary(journey => journey.RecommendationId, journey => journey.RecommendationId);

        var recommendationIdToFlightsMap = new Dictionary<int, List<Flight>>();

        foreach (var flight in flights)
        {
            var recommendationId = GetRecommendationIdForFlight(flight, journeyIdToRecommendationIdMap);

            if (recommendationIdToTotalMap.ContainsKey(recommendationId))
            {
                if (!recommendationIdToFlightsMap.ContainsKey(recommendationId))
                {
                    recommendationIdToFlightsMap[recommendationId] = new List<Flight>();
                }
                recommendationIdToFlightsMap[recommendationId].Add(flight);
            }
        }

        var groupedFlights = recommendationIdToFlightsMap.Values.ToList();

        return groupedFlights;
    }

    private static int GetRecommendationIdForFlight(Flight flight, Dictionary<int, int> RecommendationIdToJourneyIdMap)
    {
        var RecommendationId = flight.RecommendationId;

        if (RecommendationIdToJourneyIdMap.ContainsKey(RecommendationId))
        {
            return RecommendationIdToJourneyIdMap[RecommendationId];
        }
        else
        {
            return -1;
        }
    }

    public static List<List<Flight>> MakeRoundtripCombinations(List<List<Flight>> outboundFlights, List<List<Flight>> inboundFlights)
    {
        var roundtripFlightCombinations = new List<List<Flight>>();

        foreach (var outboundGroup in outboundFlights)
        {
            foreach (var inboundGroup in inboundFlights)
            {
                var roundtripCombination = new List<Flight>();
                roundtripCombination.AddRange(outboundGroup);
                roundtripCombination.AddRange(inboundGroup);
                roundtripFlightCombinations.Add(roundtripCombination);
            }
        }

        return roundtripFlightCombinations;
    }

    public static List<PriceWithTaxes> FindCheapestOptions(List<List<Flight>> roundtripFlightCombinations, List<Journey> journeys, List<TotalAvailability> totalAvailabilities)
    {
        var cheapestOptions = new List<PriceWithTaxes>();

        foreach (var combination in roundtripFlightCombinations)
        {
            decimal minPrice = decimal.MaxValue;
            Flight cheapestFlight = null;
            Journey cheapestJourney = null;

            // Find the cheapest flight in the combination
            foreach (var flight in combination)
            {
                // Find the journey associated with the flight
                var journey = journeys.FirstOrDefault(j => j.RecommendationId == flight.RecommendationId);
                if (journey == null) continue;

                // Find the total availability associated with the journey
                var totalAvailability = totalAvailabilities.FirstOrDefault(ta => ta.RecommendationId == journey.RecommendationId);
                if (totalAvailability == null) continue;

                decimal totalPrice = totalAvailability.Total;
                if (totalPrice < minPrice)
                {
                    minPrice = totalPrice;
                    cheapestFlight = flight;
                    cheapestJourney = journey;
                }
            }

            if (cheapestFlight != null && cheapestJourney != null)
            {
                // Calculate taxes
                decimal tax = CalculateTax(cheapestJourney.ImportTaxAdl, minPrice);

                // Create a PriceWithTaxes object and add it to the list
                var priceWithTax = new PriceWithTaxes
                {
                    FlightNumber = cheapestFlight.Number,
                    DepartureAirport = cheapestFlight.AirportDeparture.Code,
                    ArrivalAirport = cheapestFlight.AirportArrival.Code,
                    DepartureDate = cheapestFlight.DateDeparture,
                    ArrivalDate = cheapestFlight.DateArrival,
                    Price = minPrice,
                    Tax = tax
                };
                cheapestOptions.Add(priceWithTax);
            }
        }

        return cheapestOptions;
    }

    public static decimal CalculateTax(dynamic flight, decimal totalPrice)
    {
        decimal totalTax = 0.0m;

        foreach (var journey in flight.journeys)
        {
            decimal journeyTaxPercent = journey.importTaxAdl / 100m;
            decimal journeyTax = totalPrice * journeyTaxPercent;
            totalTax += journeyTax;
        }

        return totalTax;
    }

}