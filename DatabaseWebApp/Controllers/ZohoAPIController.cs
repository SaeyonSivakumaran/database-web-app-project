using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseWebApp.Models;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace DatabaseWebApp.Controllers
{
    public class ZohoAPIController : Controller
    {
        public async Task<ActionResult> Index(string code)
        {
            ViewData["displayWeather"] = true;
            ViewData["invalidscope"] = false;
            ApiHelper.InitializeClient();

            // Try to load the weather and if not possible, do not show the weather widget
            try
            {
                await this.LoadWeather();
            }
            catch (Exception e)
            {
                ViewData["displayWeather"] = false;
            }

            // Check if a code was passed in as a parameter in the url
            if (!string.IsNullOrEmpty(code))
            {
                ViewData["displayCode"] = true;
                ViewData["code"] = code;

                // Get code and use OAuth with Zoho API to get JSON data
                string authInfo = await this.OAuthPost(code);
                JObject authJson = JObject.Parse(authInfo);
                string accessToken = authJson["access_token"].ToString();
                string refreshToken = authJson["refresh_token"].ToString();
                return await GetZohoData(accessToken);
            } else if (code == "invalid")
            {
                ViewData["invalidscope"] = true;
            } else
            {
                ViewData["displayCode"] = false;
            }
            return View();
        }

        private async Task LoadWeather()
        {
            // Load weather using the OpenWeatherMap API
            WeatherModel weather = await WeatherProcessor.LoadWeather();
            ViewData["temperature"] = weather.Temp;
            ViewData["icon"] = $"http://openweathermap.org/img/w/{ weather.Icon }.png";
            ViewData["description"] = weather.Description;
            ViewData["city"] = weather.City;
            ViewData["country"] = weather.Country;
        }

        public ActionResult OAuthRedirect(FormCollection formInputs)
        {
            // Redirecting to authorization request url
            string clientId = "1000.6HOB8P0UQQ093190608HV63TWPQ6AH";
            Session["scope"] = formInputs["scope"];
            string redirect = "http://testwebappassetsoft.azurewebsites.net/ZohoAPI/Index";
            string redirectUrl = $"https://accounts.zoho.com/oauth/v2/auth?scope={ Session["scope"] }&client_id={ clientId }&response_type=code&access_type=offline&redirect_uri={ redirect }&prompt=consent";
            return Redirect(redirectUrl);
        }

        public async Task<String> OAuthPost(string token)
        {
            // Sending a POST request and generating access and refresh tokens
            string clientId = "1000.6HOB8P0UQQ093190608HV63TWPQ6AH";
            string clientSecret = "0ecfecbf51808d735e7d4c571da64b3370fe03f50d";
            string redirect = "http://testwebappassetsoft.azurewebsites.net/ZohoAPI/Index";
            string url = $"https://accounts.zoho.com/oauth/v2/token?code={ token }&redirect_uri={ redirect }&client_id={ clientId }&client_secret={ clientSecret }&grant_type=authorization_code";
            var post = await ApiHelper.ApiClient.PostAsync(url, null);
            string resultContent = await post.Content.ReadAsStringAsync();
            return resultContent;
        }

        public async Task<ActionResult> GetZohoData(string access)
        {
            // Send a GET request to Zoho with an authorization header
            using (var client = new HttpClient())
            {
                // Using the scope to assign a valid matching url if possible
                string url = "";
                if (Session["scope"].ToString() == "ZohoProjects.portals.READ")
                {
                    url = "https://projectsapi.zoho.com/restapi/portals/";
                }

                // Redirecting to the appropriate action if there is a valid url
                if (!String.IsNullOrEmpty(url))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access);
                    var response = await client.GetStringAsync(url);
                    return ZohoContent(response);
                } else
                {
                    return RedirectToAction("Index", new { code = "invalid" });
                }
            }
        }

        public ActionResult ZohoContent(string jsonData)
        {
            return Content(jsonData);
        }
    }
}