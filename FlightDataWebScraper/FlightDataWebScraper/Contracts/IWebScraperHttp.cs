using FlightDataWebScraper.DTOS;

public interface IWebScraperHttp
{
    Task<RootObject> FetchDataAsync(string url);
}