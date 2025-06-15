using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using Voltly.Presentation;
using Voltly.Presentation.Services;
using Voltly.Presentation.ViewModels;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<HomeViewModel>();
builder.Services.AddScoped<CounterViewModel>();
builder.Services.AddScoped<WeatherViewModel>();
builder.Services.AddScoped<IWeatherApiService, WeatherApiService>();

await builder.Build().RunAsync();