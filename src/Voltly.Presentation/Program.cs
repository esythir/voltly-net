using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

// para o componente App
using Voltly.Presentation;            

// para registrar o layout (se precisar)
// using Voltly.Presentation.Components.Shared;

using Voltly.Presentation.ViewModels;
using Voltly.Presentation.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient
builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// DI MVVM
builder.Services.AddScoped<HomeViewModel>();
builder.Services.AddScoped<CounterViewModel>();
builder.Services.AddScoped<WeatherViewModel>();

// DI Service
builder.Services.AddScoped<IWeatherApiService, WeatherApiService>();

await builder.Build().RunAsync();