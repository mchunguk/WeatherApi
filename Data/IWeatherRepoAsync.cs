using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherApi.Models;

namespace WeatherApi.Data
{
    public interface IWeatherRepoAsync
    {
        Task <bool> SaveChangesAsync();
        Task<IEnumerable<WeatherForecast>> GetAllForecastsAsync();
        Task<WeatherForecast> GetForecastByIdAsync(int id);
        Task CreateForecastAsync(WeatherForecast forecast);
        void UpdateForecast(WeatherForecast forecast);
        void DeleteForecast(WeatherForecast forecast);        
    }
}