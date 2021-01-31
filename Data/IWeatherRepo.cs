using System.Collections.Generic;
using WeatherApi.Models;

namespace WeatherApi.Data
{
    public interface IWeatherRepo
    {
        bool SaveChanges();
        
        IEnumerable<WeatherForecast> GetAllForecasts();
        WeatherForecast GetForecastById(int id);
        void CreateForecast(WeatherForecast forecast);
        void UpdateForecast(WeatherForecast forecast);
        void DeleteForecast(WeatherForecast forecast);        
    }
}