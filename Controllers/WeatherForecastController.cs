using CityWeather.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CityWeather.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public int N = 100;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<WeatherForecast[]> Get()
        {

            var client = new RestClient($"https://countriesnow.space/api/v0.1/countries/population/cities");
            var request = new RestRequest(Method.GET);
            var response = await client.ExecuteAsync(request);

            JObject obs = JObject.Parse(response.Content);

            if (response.IsSuccessful)
            {
                CityModel[] cities = obs["data"].ToObject<CityModel[]>();
                WeatherForecast[] weatherForecasts = new WeatherForecast[N];

                for( int i = 0; i<N && cities.Length>0; i++)
                {
                    CityModel city = cities[i];

                    string apiKey = Environment.GetEnvironmentVariable("OPENWEATHER_API_KEY");
                    string queryString = "https://api.openweathermap.org/data/2.5/weather?q=" + city.City + "&appid=" + apiKey;
                    System.Diagnostics.Debug.WriteLine(queryString);
                    client = new RestClient(queryString);
                    request = new RestRequest(Method.GET);
                    response = await client.ExecuteAsync(request);
                    string weatherMain = "not defined";


                    if (response.IsSuccessful)
                    {
                        obs = JObject.Parse(response.Content);
                        string rssTitle = (string)obs["weather"][0]["main"];
                        if(rssTitle.Length<30)
                            weatherMain = rssTitle;
                        System.Diagnostics.Debug.WriteLine(rssTitle);
                    } 

                    weatherForecasts[i] = new WeatherForecast
                    {
                        City = city.City,
                        Country = city.Country,
                        Weather = weatherMain,
                    };

                }

                return weatherForecasts;
            }
            else
            {
                return null;
            }

        }
    }
}
