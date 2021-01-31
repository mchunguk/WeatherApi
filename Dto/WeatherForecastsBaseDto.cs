using System;

namespace WeatherApi.Dto
{
    public class WeatherForecastsBaseDto
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public string Summary { get; set; }
    }
}
