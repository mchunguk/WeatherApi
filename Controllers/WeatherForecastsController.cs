using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherApi.Data;
using WeatherApi.Dto;

namespace WeatherApi.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    [Route("api/forecasts")]
    public class WeatherForecastsController : ControllerBase
    {
        private readonly ILogger<WeatherForecastsController> _logger;
        private readonly IWeatherRepo _repository;
        private IMapper _mapper;        

        public WeatherForecastsController(ILogger<WeatherForecastsController> logger, IWeatherRepo repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;            
        }

        //GET api/forecasts
        [HttpGet]
        public ActionResult <IEnumerable<WeatherForecastsReadDto>> GetAllForecasts()
        {
            var forecastItems = _repository.GetAllForecasts();

            return Ok(_mapper.Map<IEnumerable<WeatherForecastsReadDto>>(forecastItems));
        }

    }
}
