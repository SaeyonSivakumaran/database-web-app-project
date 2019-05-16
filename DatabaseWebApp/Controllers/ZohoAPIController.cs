using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseWebApp.Models;
using System.Threading.Tasks;
using System.Net.Http;

namespace DatabaseWebApp.Controllers
{
    public class ZohoAPIController : Controller
    {
        public async Task<ActionResult> Index(string code)
        {
            ViewData["displayWeather"] = true;
            ApiHelper.InitializeClient();
            try
            {
                await this.LoadWeather();
            } catch(Exception e)
            {
                ViewData["displayWeather"] = false;
            }          
            if (!string.IsNullOrEmpty(code))
            {
                ViewData["displayCode"] = true;
                ViewData["code"] = code;
                string authInfo = await this.OAuthPost(code);
                return Content(authInfo);
            }
            else
            {
                ViewData["displayCode"] = false;
            }
            return View();
        }

        private async Task LoadWeather()
        {
            WeatherModel weather = await WeatherProcessor.LoadWeather();
            ViewData["temperature"] = weather.Temp;
            ViewData["icon"] = $"http://openweathermap.org/img/w/{ weather.Icon }.png";
            ViewData["description"] = weather.Description;
            ViewData["city"] = weather.City;
            ViewData["country"] = weather.Country;
        }

        public ActionResult OAuthRedirect(FormCollection formInputs)
        {
            string clientId = "1000.6HOB8P0UQQ093190608HV63TWPQ6AH";
            string scope = formInputs["scope"];
            string redirect = "http://testwebappassetsoft.azurewebsites.net/ZohoAPI/Index";
            string redirectUrl = $"https://accounts.zoho.com/oauth/v2/auth?scope={ scope }&client_id={ clientId }&response_type=code&access_type=offline&redirect_uri={ redirect }&prompt=consent";
            return Redirect(redirectUrl);
        }

        public async Task<String> OAuthPost(string token)
        {
            using (var client = new HttpClient())
            {
                string clientId = "1000.6HOB8P0UQQ093190608HV63TWPQ6AH";
                string clientSecret = "0ecfecbf51808d735e7d4c571da64b3370fe03f50d";
                string redirect = "http://testwebappassetsoft.azurewebsites.net/ZohoAPI/Index";
                string url = $"https://accounts.zoho.com/oauth/v2/token?code={ token }&redirect_uri={ redirect }&client_id={ clientId }&client_secret={ clientSecret }&grant_type=authorization_code";
                var post = await client.PostAsync(url, null);
                string resultContent = await post.Content.ReadAsStringAsync();
                return resultContent;
            }
        }
    }
}