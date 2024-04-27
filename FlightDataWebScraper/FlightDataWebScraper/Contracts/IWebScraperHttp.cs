using FlightDataWebScraper.DTOS;

public interface IWebScraperHttp
{
    Task<Json.RootObject> FetchDataAsync(string url);
}