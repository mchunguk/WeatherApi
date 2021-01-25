using System.Collections.Generic;
using System.Linq;
using WeatherApi.Models;

namespace WeatherApi.Data
{
    public class SqlWeatherRepo : IWeatherRepo
    {
        private readonly WeatherContext _context;

        public SqlWeatherRepo(WeatherContext context)
        {
            _context = context;
        }

        public IEnumerable<WeatherForecast> GetAllForecasts()
        {
            return _context.WeatherForecasts.ToList();
        }        
    }
}