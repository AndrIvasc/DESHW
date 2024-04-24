using System;
using System.Net.Http;

public class WebScraperHttp
{
    private readonly HttpClient _httpClient;

    public WebScraperHttp()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> FetchDataAsync(string url)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error fetching data: {ex.Message}");
            return null;
        }
    }
}
