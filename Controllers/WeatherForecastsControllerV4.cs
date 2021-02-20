using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WeatherApi.Data;
using WeatherApi.Dto;
using WeatherApi.Models;

namespace WeatherApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/forecasts")]
    [ApiVersion("4.0")]
    public class WeatherForecastsControllerV4 : ControllerBase
    {
        private readonly ILogger<WeatherForecastsController> _logger;
        private readonly IWeatherRepoAsync _repository;
        private IMapper _mapper;        

        public WeatherForecastsControllerV4(ILogger<WeatherForecastsController> logger, IWeatherRepoAsync repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;            
        }

        /// <summary>
        /// Gets a list of all forecasts (V4).
        /// GET api/forecasts
        ///
        /// https://tools.ietf.org/html/rfc2616#section-9.3
        /// </summary>
        /// <returns>All forecasts.</returns>
        /// <response code="200">Forecasts successfully retrieved.</response>
        [HttpGet(Name = nameof(GetAllForecasts))]
        [Produces( "application/json" )]
        [ProducesResponseType( typeof( IEnumerable<WeatherForecastsReadDtoV4> ), 200 )]
        public async Task<IActionResult> GetAllForecasts()
        {
            var forecastItems = await _repository.GetAllForecastsAsync();

            _logger.LogInformation("Within {method}.", "GetAllForecasts");

            return Ok(_mapper.Map<IEnumerable<WeatherForecastsReadDtoV4>>(forecastItems));
        }

        /// <summary>
        /// Gets a single forecast (V4).
        /// GET api/forecasts/{id}
        ///
        /// https://tools.ietf.org/html/rfc2616#section-9.3
        /// </summary>
        /// <param name="id">The requested forecast identifier.</param>
        /// <returns>The requested forecast.</returns>
        /// <response code="200">The forecast was successfully retrieved.</response>
        /// <response code="400">The input value is invalid.</response>
        /// <response code="404">The forecast does not exist.</response>
        [HttpGet("{id}", Name = nameof(GetForecastById))]
        [Produces( "application/json" )]
        [ProducesResponseType( typeof( WeatherForecastsReadDtoV4 ), 200 )]
        [ProducesResponseType( 400 )]
        [ProducesResponseType( 404 )]
        public async Task<IActionResult> GetForecastById(int id)
        {
            var forecastItems = await _repository.GetForecastByIdAsync(id);

            if (forecastItems != null)
            {
                return Ok(_mapper.Map<WeatherForecastsReadDtoV4>(forecastItems));
            }

            return NotFound();
            
        }

        /// <summary>
        /// Add a new forecast (V3 and V4).
        /// POST api/forecasts
        ///
        /// https://tools.ietf.org/html/rfc2616#section-9.5
        /// </summary>
        /// <param name="forecastCreateDto">The forecast to add.</param>
        /// <returns>The created order route in the header.</returns>
        /// <response code="201">The forecast was successfully placed.</response>
        /// <response code="400">The forecast is invalid.</response>
        [HttpPost]
        [ProducesResponseType( typeof( WeatherForecastCreateDto ), 201 )]
        [ProducesResponseType( 400 )]
        public async Task<IActionResult> CreateForecast(WeatherForecastCreateDto forecastCreateDto)
        {
            var forecastModel = _mapper.Map<WeatherForecast>(forecastCreateDto);

            await _repository.CreateForecastAsync(forecastModel);
            await _repository.SaveChangesAsync();

            var forecastReadtDto = _mapper.Map<WeatherForecastsReadDtoV4>(forecastModel);

            return CreatedAtRoute(nameof(GetForecastById), new {Id = forecastReadtDto.Id}, forecastReadtDto);
        }

        /// <summary>
        /// Fully replace an existing forecast (V4 - same as V3).
        /// PUT api/forecasts/{id}
        ///
        /// https://tools.ietf.org/html/rfc2616#section-9.6
        /// </summary>
        /// <param name="forecastsUpdateDto">The forecast to fully replace.</param>
        /// <returns>Nothing.</returns>
        /// <response code="204">The forecast was successfully replaced.</response>
        /// <response code="400">The forecast passed in the body is invalid.</response>
        /// <response code="404">The forecast to be replaced does not exist.</response>
        [HttpPut("{id}")]
        [ProducesResponseType( 204 )]
        [ProducesResponseType( 400 )]
        [ProducesResponseType( 404 )]
        public async Task<IActionResult> UpdateForecast(int id, WeatherForecastsUpdateDto forecastsUpdateDto)
        {
            var forecastModelFromRepo = await _repository.GetForecastByIdAsync(id);

            if (forecastModelFromRepo == null)
            {
                return NotFound();
            }

            // mapper will update model from passed in object.
            // will be tracked via db context
            _mapper.Map(forecastsUpdateDto, forecastModelFromRepo);

            // keep this in in case you want to change the implementation in future i.e ORM or underlying database
            _repository.UpdateForecast (forecastModelFromRepo);

            await _repository.SaveChangesAsync();

            return NoContent();

        }

        /// <summary>
        /// Update fields of an existing forecast (V4 - same as V3).
        /// PATCH api/forecasts/{id}
        ///
        /// https://tools.ietf.org/html/rfc2068#section-19.6.1.1
        /// </summary>
        /// <param name="patchDoc">The json patch document containing the fields of the forecast to update.</param>
        /// <returns>Nothing.</returns>
        /// <response code="204">The forecast was successfully updated.</response>
        /// <response code="400">The forecast passed in the body is invalid.</response>
        /// <response code="404">The forecast to be updated does not exist.</response>
        [HttpPatch("{id}")]
        [ProducesResponseType( 204 )]
        [ProducesResponseType( 400 )]
        [ProducesResponseType( 404 )]
        public async Task<IActionResult> PartialForecastUpdate(int id, JsonPatchDocument<WeatherForecastsUpdateDto> patchDoc)
        {
            var forecastModelFromRepo = await _repository.GetForecastByIdAsync(id);

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

            await _repository.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes a forecast (V4 - same as V3).
        /// DELETE api/forecasts/{id}
        ///
        /// https://tools.ietf.org/html/rfc7231#section-4.3.5
        /// </summary>
        /// <param name="id">The forecast to delete.</param>
        /// <returns>None</returns>
        /// <response code="204">The forecast was successfully deleted.</response>
        /// <response code="400">The input value is invalid.</response>
        /// <response code="404">The forecast does not exist.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType( 204 )]
        [ProducesResponseType( 400 )]
        [ProducesResponseType( 404 )]
        public async Task<IActionResult> DeleteCommand(int id)
        {
            var forecastModelFromRepo = await _repository.GetForecastByIdAsync(id);

            if (forecastModelFromRepo == null)
            {
                return NotFound();
            }

            _repository.DeleteForecast(forecastModelFromRepo);
            await _repository.SaveChangesAsync();
            
            return NoContent();

        }

    }
}
