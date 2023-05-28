using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


public class WeatherInfoApi
{
    public string Name { get; set; }
    public WeatherApi[] Weather { get; set; }
    public MainApi Main { get; set; }
    public WindApi Wind { get; set; }
}

