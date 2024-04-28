using System;
using System.Net.Http;
using System.Net.Http.Json;
using FlightDataWebScraper.DTOS;

public class WebScraperHttp : IWebScraperHttp
{
    private readonly HttpClient _httpClient;

    public WebScraperHttp(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<RootObject> FetchDataAsync(string url)
    {
        try
        {
            RootObject? response = await _httpClient.GetFromJsonAsync<RootObject>(url);
            if (response == null)
            {
                throw new Exception("Could not parse the respomse");
            }

            return response;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error fetching data: {ex.Message}");
            return null;
        }
    }
}
