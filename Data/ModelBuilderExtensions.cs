using System;
using Microsoft.EntityFrameworkCore;
using WeatherApi.Models;

namespace WeatherApi.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var rng = new Random();
            var Summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            modelBuilder.Entity<WeatherForecast>().HasData(
                new WeatherForecast { Id = 1, Date = DateTime.Now.AddDays(1), TemperatureC = rng.Next(-20, 55), Summary = Summaries[rng.Next(Summaries.Length)]},
                new WeatherForecast { Id = 2, Date = DateTime.Now.AddDays(2), TemperatureC = rng.Next(-20, 55), Summary = Summaries[rng.Next(Summaries.Length)]},
                new WeatherForecast { Id = 3, Date = DateTime.Now.AddDays(3), TemperatureC = rng.Next(-20, 55), Summary = Summaries[rng.Next(Summaries.Length)]},
                new WeatherForecast { Id = 4, Date = DateTime.Now.AddDays(4), TemperatureC = rng.Next(-20, 55), Summary = Summaries[rng.Next(Summaries.Length)]},
                new WeatherForecast { Id = 5, Date = DateTime.Now.AddDays(5), TemperatureC = rng.Next(-20, 55), Summary = Summaries[rng.Next(Summaries.Length)]},
                new WeatherForecast { Id = 6, Date = DateTime.Now.AddDays(6), TemperatureC = rng.Next(-20, 55), Summary = Summaries[rng.Next(Summaries.Length)]},
                new WeatherForecast { Id = 7, Date = DateTime.Now.AddDays(7), TemperatureC = rng.Next(-20, 55), Summary = Summaries[rng.Next(Summaries.Length)]},
                new WeatherForecast { Id = 8, Date = DateTime.Now.AddDays(8), TemperatureC = rng.Next(-20, 55), Summary = Summaries[rng.Next(Summaries.Length)]},
                new WeatherForecast { Id = 9, Date = DateTime.Now.AddDays(9), TemperatureC = rng.Next(-20, 55), Summary = Summaries[rng.Next(Summaries.Length)]},
                new WeatherForecast { Id = 10, Date = DateTime.Now.AddDays(10), TemperatureC = rng.Next(-20, 55), Summary = Summaries[rng.Next(Summaries.Length)]}                                                                                                
            );
        }
    }

}