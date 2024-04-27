using System.Formats.Asn1;
using System.Globalization;
using System.Net.Http;
using System.Security.Authentication.ExtendedProtection;
using CsvHelper;
using CsvHelper.Configuration;
using FlightDataWebScraper.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using static FlightDataWebScraper.DTOS.Json;

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

            // Group outbound and inbound flights by price category
            //var groupedOutboundFlights = GroupFlightsByPriceCategory(outboundFlights);
            //var groupedInboundFlights = GroupFlightsByPriceCategory(inboundFlights);

            //var roundtripFlightCombinations = MakeRoundtripCombinations(groupedOutboundFlights, groupedInboundFlights);

            //var cheapestOptions = FindCheapestOptions(roundtripFlightCombinations);

            //csvService.SaveToCsv(cheapestOptions, filePath);

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

    public static List<List<Flight>> GroupFlightsByPriceCategory(List<Flight> flights)
    {
        var groupedFlights = flights.GroupBy(flight => flight.Total)
                                    .Select(group => group.ToList())
                                    .ToList();
        return groupedFlights;
    }

    public static List<List<dynamic>> MakeRoundtripCombinations(List<List<dynamic>> outboundFlights, List<List<dynamic>> inboundFlights)
    {
        var roundtripFlightCombinations = new List<List<dynamic>>();

        foreach (var outboundGroup in outboundFlights)
        {
            foreach (var inboundGroup in inboundFlights)
            {
                var roundtripCombination = new List<dynamic>();
                roundtripCombination.AddRange(outboundGroup);
                roundtripCombination.AddRange(inboundGroup);
                roundtripFlightCombinations.Add(roundtripCombination);
            }
        }

        return roundtripFlightCombinations;
    }

    public static List<dynamic> FindCheapestOptions(List<List<dynamic>> roundtripFlightCombinations)
    {
        var cheapestOptions = new List<dynamic>();

        foreach (var combination in roundtripFlightCombinations)
        {
            decimal minPrice = decimal.MaxValue;
            dynamic cheapestFlight = null;

            // Find the cheapest flight in the combination
            foreach (var flight in combination)
            {
                decimal totalPrice = flight.total;
                if (totalPrice < minPrice)
                {
                    minPrice = totalPrice;
                    cheapestFlight = flight;
                }
            }

            if (cheapestFlight != null)
            {
                // Calculate taxes
                decimal tax = CalculateTax(cheapestFlight, minPrice);

                // Create a PriceWithTaxes object and add it to the list
                var priceWithTax = new PriceWithTaxes
                {
                    FlightNumber = cheapestFlight.number,
                    DepartureAirport = cheapestFlight.airportDeparture.code,
                    ArrivalAirport = cheapestFlight.airportArrival.code,
                    DepartureDate = cheapestFlight.dateDeparture,
                    ArrivalDate = cheapestFlight.dateArrival,
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