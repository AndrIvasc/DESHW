using System.Net.Http;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        var url = "http://homeworktask.infare.lt/search.php?from=MAD&to=FUE&depart=2024-05-09&return=2024-05-16";

        using HttpClient client = new HttpClient();

        var scraper = new WebScraperHttp();
        string jsonData = await scraper.FetchDataAsync(url);

        try
        {

            //FlightDataWebScraper.Json.RootObject rootObject = JsonConvert.DeserializeObject<FlightDataWebScraper.Json.RootObject>(jsonData);

            //if (rootObject?.Body?.Data?.Journeys != null)
            //{
            //    foreach (var journey in rootObject.Body.Data.Journeys)
            //    {
            //        foreach (var flight in journey.Flights)
          //          {
           //             Console.WriteLine($"Flight Number: {flight.Number}");
          //          }
          //      }

          //  }
                dynamic desirializeObject = JsonConvert.DeserializeObject(jsonData); //var arba dynamic
                //Console.WriteLine(desirializeObject);
                //Console.ReadLine();

                var flights = desirializeObject.body.data.journeys[1].flights; // <---- VEIKIA!!!!!!!!

            Console.WriteLine(flights);
            Console.ReadLine();

            //var outboundFlights = flights.body.data.journeys.FindAll(f => f.DepartureAirport == "MAD" && f.ArrivalAirport == "AUH");

            //List<FlightInformation> data = JsonConvert.DeserializeObject<List<FlightInformation>>(jsonData);



            // Extracting all the flights
            // var flights = desirializeObject.body.data.journeys.flights;

            //Filter all the flights from MAD - AUH
            //var madToAuhFlights = flights.Where(f1 => f1.airportDeparture.code == "MAD" && f1.airportArrivals.code == "AUH");

            // Extract all the prices for each recomentadion

            //var prices = desirializeObject.body.totalAvailabilities.ToDictionary(p => (int)p.recommendationId, p => (double)p.total);



            //Console.WriteLine("");
            //Console.ReadKey();

            //List<FlightData> data = JsonConvert.DeserializeObject<List<FlightData>>(json);

            //WriteToCsv(data, @"C:\Users\andre\Desktop\FlightData.csv");

            Console.WriteLine("Data scraped and saved to flights.csv successfully.");
                Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        // todo : create object with properties that need to be filled in the the csv...
    }
}