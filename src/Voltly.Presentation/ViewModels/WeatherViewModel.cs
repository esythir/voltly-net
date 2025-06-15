using Voltly.Presentation.Services;
using Voltly.Presentation.ViewModels;

namespace Voltly.Presentation.ViewModels
{
    public class WeatherViewModel
    {
        private readonly IWeatherApiService _svc;
        public WeatherForecast[]? Forecasts { get; private set; }

        public WeatherViewModel(IWeatherApiService svc)
            => _svc = svc;

        public async Task LoadAsync()
            => Forecasts = await _svc.GetForecastsAsync();
    }
    
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }
        public int TemperatureC { get; set; }
        public string? Summary { get; set; }
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}