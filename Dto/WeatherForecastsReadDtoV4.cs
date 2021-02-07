using System;

namespace WeatherApi.Dto
{
    public class WeatherForecastsReadDtoV4
    {
        public int Id { get; set; }
         
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }

        public string Version => "This is v4";
    }
}
