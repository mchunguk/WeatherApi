using System;
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

        public void CreateForecast(WeatherForecast forecast)
        {
            if (forecast == null)
            {
                throw new ArgumentNullException(nameof(forecast));
            }

            _context.WeatherForecasts.Add(forecast);
        }

        public void DeleteForecast(WeatherForecast forecast)
        {
            if (forecast==null)
            {
                throw new ArgumentNullException(nameof(forecast));
            }

            _context.WeatherForecasts.Remove(forecast);
        }

        public IEnumerable<WeatherForecast> GetAllForecasts()
        {
            return _context.WeatherForecasts.ToList();
        }

        public WeatherForecast GetForecastById(int id)
        {
            return _context.WeatherForecasts.FirstOrDefault( p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >=0);
        }

        public void UpdateForecast(WeatherForecast forecast)
        {
             // Nothing
        }
    }
}