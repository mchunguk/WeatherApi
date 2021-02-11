using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WeatherApi.Models;

namespace WeatherApi.Data
{
    public class SqlWeatherRepoAsync : IWeatherRepoAsync
    {
        private readonly WeatherContext _context;

        public SqlWeatherRepoAsync(WeatherContext context)
        {
            _context = context;
        }

        public async Task CreateForecastAsync(WeatherForecast forecast)
        {
            if (forecast == null)
            {
                throw new ArgumentNullException(nameof(forecast));
            }

            await _context.WeatherForecasts.AddAsync(forecast);
        }

        public void DeleteForecast(WeatherForecast forecast)
        {
            if (forecast==null)
            {
                throw new ArgumentNullException(nameof(forecast));
            }

            _context.WeatherForecasts.Remove(forecast);
        }

        public async Task <IEnumerable<WeatherForecast>> GetAllForecastsAsync()
        {
            return await _context.WeatherForecasts.ToListAsync();
        }
        public async Task <WeatherForecast> GetForecastByIdAsync(int id)
        {
            return await _context.WeatherForecasts.FirstOrDefaultAsync( p => p.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >=0);
        }

        public void UpdateForecast(WeatherForecast forecast)
        {
             // Nothing
        }
    }
}