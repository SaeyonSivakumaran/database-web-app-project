using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseWebApp.Models
{
    public class WeatherModel
    {
        public string Icon { get; set; }
        public string Description { get; set; }
        public string Temp { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

    }
}