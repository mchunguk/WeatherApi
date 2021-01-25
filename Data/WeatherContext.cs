using WeatherApi.Models;
using Microsoft.EntityFrameworkCore;

namespace WeatherApi.Data
{
    public class WeatherContext : DbContext
    {
        public WeatherContext(DbContextOptions<WeatherContext> opt) : base(opt)
        {
            
        }

        public DbSet<WeatherForecast> Weather { get; set; }


    }
}