using Voltly.Presentation.ViewModels;

namespace Voltly.Presentation.Services;

public interface IWeatherApiService
{
    Task<WeatherForecast[]> GetForecastsAsync();
}