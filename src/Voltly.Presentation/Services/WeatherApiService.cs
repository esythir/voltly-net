using System.Net.Http;
using System.Net.Http.Json;
using Voltly.Presentation.ViewModels;

namespace Voltly.Presentation.Services;

public class WeatherApiService : IWeatherApiService
{
    private readonly HttpClient _http;
    public WeatherApiService(HttpClient http) => _http = http;
    public Task<WeatherForecast[]> GetForecastsAsync()
        => _http.GetFromJsonAsync<WeatherForecast[]>("sample-data/weather.json")!;
}