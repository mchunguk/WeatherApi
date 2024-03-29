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

        /// <summary>
        /// Gets a list of all forecasts (V1).
        /// GET api/forecasts
        ///
        /// https://tools.ietf.org/html/rfc2616#section-9.3
        /// </summary>
        /// <returns>All forecasts.</returns>
        /// <response code="200">Forecasts successfully retrieved.</response>
        [MapToApiVersion("1.0")]
        [HttpGet(Name = nameof(GetAllForecasts))]
        [Produces( "application/json" )]
        [ProducesResponseType( typeof( IEnumerable<WeatherForecastsReadDto> ), 200 )]
        public ActionResult <IEnumerable<WeatherForecastsReadDto>> GetAllForecasts()
        {
            var forecastItems = _repository.GetAllForecasts();

            return Ok(_mapper.Map<IEnumerable<WeatherForecastsReadDto>>(forecastItems));
        }

        /// <summary>
        /// Gets a single forecast (V1).
        /// GET api/forecasts/{id}
        ///
        /// https://tools.ietf.org/html/rfc2616#section-9.3
        /// </summary>
        /// <param name="id">The requested forecast identifier.</param>
        /// <returns>The requested forecast.</returns>
        /// <response code="200">The forecast was successfully retrieved.</response>
        /// <response code="400">The input value is invalid.</response>
        /// <response code="404">The forecast does not exist.</response>
        [MapToApiVersion("1.0")]
        [HttpGet("{id}", Name = nameof(GetForecastById))]
        [Produces( "application/json" )]
        [ProducesResponseType( typeof( WeatherForecastsReadDto ), 200 )]
        [ProducesResponseType( 400 )]
        [ProducesResponseType( 404 )]
        public ActionResult <WeatherForecastsReadDto> GetForecastById(int id)
        {
            var forecastItems = _repository.GetForecastById(id);

            if (forecastItems != null)
            {
                return Ok(_mapper.Map<WeatherForecastsReadDto>(forecastItems));
            }

            return NotFound();
            
        }

        /// <summary>
        /// Add a new forecast (V1 and V2).
        /// POST api/forecasts
        ///
        /// https://tools.ietf.org/html/rfc2616#section-9.5
        /// </summary>
        /// <param name="forecastCreateDto">The forecast to add.</param>
        /// <returns>The created order route in the header.</returns>
        /// <response code="201">The forecast was successfully placed.</response>
        /// <response code="400">The forecast is invalid.</response>
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
        [HttpPost]
        [ProducesResponseType( typeof( WeatherForecastCreateDto ), 201 )]
        [ProducesResponseType( 400 )]
        public ActionResult<WeatherForecastsReadDto> CreateForecast(WeatherForecastCreateDto forecastCreateDto)
        {
            var forecastModel = _mapper.Map<WeatherForecast>(forecastCreateDto);

            _repository.CreateForecast(forecastModel);
            _repository.SaveChanges();

            var forecastReadtDto = _mapper.Map<WeatherForecastsReadDto>(forecastModel);

            return CreatedAtRoute(nameof(GetForecastById), new {Id = forecastReadtDto.Id}, forecastReadtDto);
        }

        /// <summary>
        /// Fully replace an existing forecast (V1 and V2).
        /// PUT api/forecasts/{id}
        ///
        /// https://tools.ietf.org/html/rfc2616#section-9.6
        /// </summary>
        /// <param name="forecastsUpdateDto">The forecast to fully replace.</param>
        /// <returns>Nothing.</returns>
        /// <response code="204">The forecast was successfully replaced.</response>
        /// <response code="400">The forecast passed in the body is invalid.</response>
        /// <response code="404">The forecast to be replaced does not exist.</response>
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
        [HttpPut("{id}")]
        [ProducesResponseType( 204 )]
        [ProducesResponseType( 400 )]
        [ProducesResponseType( 404 )]
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

        /// <summary>
        /// Update fields of an existing forecast (V1 and V2).
        /// PATCH api/forecasts/{id}
        ///
        /// https://tools.ietf.org/html/rfc2068#section-19.6.1.1
        /// </summary>
        /// <param name="patchDoc">The json patch document containing the fields of the forecast to update.</param>
        /// <returns>Nothing.</returns>
        /// <response code="204">The forecast was successfully updated.</response>
        /// <response code="400">The forecast passed in the body is invalid.</response>
        /// <response code="404">The forecast to be updated does not exist.</response>
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
        [HttpPatch("{id}")]
        [ProducesResponseType( 204 )]
        [ProducesResponseType( 400 )]
        [ProducesResponseType( 404 )]
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

        /// <summary>
        /// Deletes a forecast (V1 and V2).
        /// DELETE api/forecasts/{id}
        ///
        /// https://tools.ietf.org/html/rfc7231#section-4.3.5
        /// </summary>
        /// <param name="id">The forecast to delete.</param>
        /// <returns>None</returns>
        /// <response code="204">The forecast was successfully deleted.</response>
        /// <response code="400">The input value is invalid.</response>
        /// <response code="404">The forecast does not exist.</response>
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
        [HttpDelete("{id}")]
        [ProducesResponseType( 204 )]
        [ProducesResponseType( 400 )]
        [ProducesResponseType( 404 )]
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

        /// <summary>
        /// Gets a list of all forecasts (V2).
        /// GET api/forecasts
        ///
        /// https://tools.ietf.org/html/rfc2616#section-9.3
        /// </summary>
        /// <returns>All forecasts.</returns>
        /// <response code="200">Forecasts successfully retrieved.</response>
        [MapToApiVersion("2.0")]
        [HttpGet(Name = nameof(GetAllForecastsV2))]
        [Produces( "application/json" )]
        [ProducesResponseType( typeof( IEnumerable<WeatherForecastsReadDtoV2> ), 200 )]
        public ActionResult <IEnumerable<WeatherForecastsReadDtoV2>> GetAllForecastsV2()
        {
            var forecastItems = _repository.GetAllForecasts();

            return Ok(_mapper.Map<IEnumerable<WeatherForecastsReadDtoV2>>(forecastItems));
        }

        /// <summary>
        /// Gets a single forecast (V2).
        /// GET api/forecasts/{id}
        ///
        /// https://tools.ietf.org/html/rfc2616#section-9.3
        /// </summary>
        /// <param name="id">The requested forecast identifier.</param>
        /// <returns>The requested forecast.</returns>
        /// <response code="200">The forecast was successfully retrieved.</response>
        /// <response code="400">The input value is invalid.</response>
        /// <response code="404">The forecast does not exist.</response>
        [MapToApiVersion("2.0")]
        [HttpGet("{id}", Name = nameof(GetForecastByIdV2))]
        [Produces( "application/json" )]
        [ProducesResponseType( typeof( WeatherForecastsReadDtoV2 ), 200 )]
        [ProducesResponseType( 400 )]
        [ProducesResponseType( 404 )]
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
