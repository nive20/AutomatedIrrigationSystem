using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Demo2.EvaluateWeatherResult;

namespace Demo2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<WeatherData> Get()
        {
            string baseUrl = "https://api.weather.com/v3/wx/forecast/daily/7day?geocode=48.1545703,11.2616557&format=json&units=m&language=en-US&apiKey=41b54a6111754445b54a611175b44574";
            EvaluateWeatherResult evaluateWeatherResult = new EvaluateWeatherResult();
            WeatherData weatherData = new WeatherData();
            WeatherData result = evaluateWeatherResult.EvaluateWeatherData(baseUrl);
            // return new string[] { "60", "Partly cloudy skies. Hot. Low 13C. Winds ESE at 10 to 15 km/h." };
            return result;
        }



        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
