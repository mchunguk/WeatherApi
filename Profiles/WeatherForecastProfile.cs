using AutoMapper;
using WeatherApi.Dto;
using WeatherApi.Models;

namespace WeatherApi.Profiles
{
    public class WeatherForecastProfile : Profile
    {
        public WeatherForecastProfile()
        {
            CreateMap<WeatherForecast, WeatherForecastsReadDto>();
            CreateMap<WeatherForecast, WeatherForecastCreateDto>();
            CreateMap<WeatherForecastCreateDto,WeatherForecast>();
            CreateMap<WeatherForecastsUpdateDto,WeatherForecast>();
            CreateMap<WeatherForecast, WeatherForecastsUpdateDto>();

            CreateMap<WeatherForecast, WeatherForecastsReadDtoV2>();
            CreateMap<WeatherForecast, WeatherForecastsReadDtoV3>();
            CreateMap<WeatherForecast, WeatherForecastsReadDtoV4>();
        }
        
    }

}