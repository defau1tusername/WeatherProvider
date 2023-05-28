using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WeatherInfo
{
    public string CityName { get; set; }
    public double Temperature { get; set; }
    public string Description { get; set; }

    public WeatherInfo(string cityName, double temperature, string description)
    {
        CityName = cityName;
        Temperature = Math.Floor(temperature - 273);
        Description = description;
    }
}

