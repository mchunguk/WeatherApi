namespace WeatherApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [ApiVersionNeutral]
    [Route( "api/forecasts" )]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [Route( "health" )]
        public ActionResult Ping() => Ok();        
    }
}