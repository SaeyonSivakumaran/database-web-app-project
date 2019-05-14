using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace DatabaseWebApp.Models
{
    public class WeatherProcessor
    {
        public static string key = "7e8467fc371dea33bf7ec5df6f73697d";
        public static string cityId = "6066513";

        public static async Task<WeatherModel> LoadWeather()
        {
            string url = $"http://api.openweathermap.org/data/2.5/weather?id={ cityId }&APPID={ key }&units=metric";
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    WeatherModel weather = new WeatherModel();
                    string data = await response.Content.ReadAsStringAsync();
                    dynamic jsonData = JsonConvert.DeserializeObject(data);
                    weather.Temp = jsonData.main.temp.ToString();
                    weather.Icon = jsonData.weather[0].icon.ToString();
                    weather.Description = jsonData.weather[0].description.ToString();
                    weather.City = jsonData.name;
                    weather.Country = jsonData.sys.country;
                    return weather;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}