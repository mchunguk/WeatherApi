﻿using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherApi.Data;
using WeatherApi.Dto;
using WeatherApi.Models;

namespace WeatherApi.Controllers
{
    [ApiController]
    // [Route("api/forecasts")]
    [Route("api/v{version:apiVersion}/forecasts")]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
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

        //https://tools.ietf.org/html/rfc2616#section-9.3
        //GET api/forecasts
        [HttpGet(Name="GetAllForecasts")]
        [MapToApiVersion("1.0")]
        public ActionResult <IEnumerable<WeatherForecastsReadDto>> GetAllForecasts()
        {
            var forecastItems = _repository.GetAllForecasts();

            return Ok(_mapper.Map<IEnumerable<WeatherForecastsReadDto>>(forecastItems));
        }

        //https://tools.ietf.org/html/rfc2616#section-9.3
        //GET api/forecasts/{id}
        [HttpGet("{id}", Name="GetForecastById")]
        [MapToApiVersion("1.0")]
        public ActionResult <WeatherForecastsReadDto> GetForecastById(int id)
        {
            var forecastItems = _repository.GetForecastById(id);

            if (forecastItems != null)
            {
                return Ok(_mapper.Map<WeatherForecastsReadDto>(forecastItems));
            }

            return NotFound();
            
        }

        //https://tools.ietf.org/html/rfc2616#section-9.5
        //POST api/forecasts
        [HttpPost]
        [MapToApiVersion("1.0")]
        public ActionResult<WeatherForecastsReadDto> CreateForecast(WeatherForecastCreateDto forecastCreateDto)
        {
            var forecastModel = _mapper.Map<WeatherForecast>(forecastCreateDto);

            _repository.CreateForecast(forecastModel);
            _repository.SaveChanges();

            var forecastReadtDto = _mapper.Map<WeatherForecastsReadDto>(forecastModel);

            return CreatedAtRoute(nameof(GetForecastById), new {Id = forecastReadtDto.Id}, forecastReadtDto);
        }

        //https://tools.ietf.org/html/rfc2616#section-9.6
        //PUT api/forecasts/{id}
        [HttpPut("{id}")]
        [MapToApiVersion("1.0")]
        public ActionResult UpdateForecast(int id, WeatherForecastsUpdateDto forecastsUpdateDto)
        {
            var forecastModelFromRepo = _repository.GetForecastById(id);

            if (forecastModelFromRepo == null)
            {
                return NotFound();
            }

            // mapper will update model from passed in object.
            // will be tracked via db context
            _mapper.Map(forecastsUpdateDto, forecastModelFromRepo);

            // keep this in in case you want to change the implementation in future i.e ORM or underlying database
            _repository.UpdateForecast (forecastModelFromRepo);

            _repository.SaveChanges();

            return NoContent();

        }

        //https://tools.ietf.org/html/rfc2068#section-19.6.1.1
        //PATCH api/forecasts/{id}
        [HttpPatch("{id}")]
        [MapToApiVersion("1.0")]
        public ActionResult PartialForecastUpdate(int id, JsonPatchDocument<WeatherForecastsUpdateDto> patchDoc)
        {
            var forecastModelFromRepo = _repository.GetForecastById(id);

            if (forecastModelFromRepo == null)
            {
                return NotFound();
            }

            // check that change will be accepted
            var forecastToPatch = _mapper.Map<WeatherForecastsUpdateDto>(forecastModelFromRepo);
            patchDoc.ApplyTo(forecastToPatch, ModelState);

            if (!TryValidateModel(forecastToPatch))
            {
                return ValidationProblem(ModelState);
            }

            // Apply the change
            _mapper.Map(forecastToPatch, forecastModelFromRepo);

            // keep this in in case you want to change the implementation in future i.e ORM or underlying database
            _repository.UpdateForecast (forecastModelFromRepo);

            _repository.SaveChanges();

            return NoContent();
        }

        //https://tools.ietf.org/html/rfc7231#section-4.3.5
        //DELETE api/forecasts/{id}
        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]        
        public ActionResult DeleteCommand(int id)
        {
            var forecastModelFromRepo = _repository.GetForecastById(id);

            if (forecastModelFromRepo == null)
            {
                return NotFound();
            }

            _repository.DeleteForecast(forecastModelFromRepo);
            _repository.SaveChanges();
            
            return NoContent();

        }


        //https://tools.ietf.org/html/rfc2616#section-9.3
        //GET api/forecasts
        [HttpGet(Name = nameof(GetAllForecastsV2))]
        [MapToApiVersion("2.0")]
        public ActionResult <IEnumerable<WeatherForecastsReadDtoV2>> GetAllForecastsV2()
        {
            var forecastItems = _repository.GetAllForecasts();

            return Ok(_mapper.Map<IEnumerable<WeatherForecastsReadDtoV2>>(forecastItems));
        }

        //https://tools.ietf.org/html/rfc2616#section-9.3
        //GET api/forecasts/{id}
        [HttpGet("{id}", Name = nameof(GetForecastByIdV2))]
        [MapToApiVersion("2.0")]
        public ActionResult <WeatherForecastsReadDtoV2> GetForecastByIdV2(int id)
        {
            var forecastItems = _repository.GetForecastById(id);

            if (forecastItems != null)
            {
                return Ok(_mapper.Map<WeatherForecastsReadDtoV2>(forecastItems));
            }

            return NotFound();
            
        }


    }
}
