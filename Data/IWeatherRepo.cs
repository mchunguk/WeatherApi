using System.Collections.Generic;
using WeatherApi.Models;

namespace WeatherApi.Data
{
    public interface IWeatherRepo
    {
        public IEnumerable<WeatherForecast> GetAllForecasts();
    }
}