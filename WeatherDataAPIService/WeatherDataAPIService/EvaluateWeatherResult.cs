using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Demo2
{
    public class EvaluateWeatherResult
    {
        public WeatherData EvaluateWeatherData(String url)
        {
            string json = new WebClient().DownloadString(url);
             JObject data = (JObject)JsonConvert.DeserializeObject(json);

            Root items = JsonConvert.DeserializeObject<Root>(json);

            WeatherData weatherData = new WeatherData();
            weatherData.narrative = items.daypart[0].narrative[1].ToString();
            weatherData.preceipechance = items.daypart[0].precipChance[1].ToString();
            GetPrecipitationData();
            return weatherData;


        }

        public WeatherData weatherData { get; set; }
        public class WeatherData
        {
            public string narrative { get; set; }
            public string preceipechance { get; set; }
        }
        private string GetPrecipitationData()
        {

            string data = string.Empty;

            try
            {
                string json = new WebClient().DownloadString("https://weatherapidata.eu-gb.mybluemix.net/api/values");
                WeatherData items = JsonConvert.DeserializeObject<WeatherData>(json);
                data = items.preceipechance;
            }
            catch (Exception ex)
            {
                Random random = new Random();
                data = random.Next(0, 100).ToString();
            }
            return data;
        }
        public class Daypart
        {

            public List<string> narrative { get; set; }
            public List<int> precipChance { get; set; }
        }
        public class Root
        {
           public List<Daypart> daypart { get; set; }
        }
    }
}
