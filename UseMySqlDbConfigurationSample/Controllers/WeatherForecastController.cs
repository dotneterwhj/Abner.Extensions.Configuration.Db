using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace UseSqlDbConfigurationSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IOptions<DemoOption> _demoOption;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
            IOptions<DemoOption> demoOption,
            IConfiguration configuration)
        {
            _logger = logger;
            this._demoOption = demoOption;
            this._configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(new
            {
                json = _demoOption.Value,
                array = _configuration["MySqlArray:0:myKey"],
                text = _configuration["MysqlText"]
            });
        }

        //[HttpGet]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}
    }
}