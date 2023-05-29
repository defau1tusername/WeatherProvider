public class WeatherInfo
{
    public string CityName { get; set; }
    public double Temperature { get; set; }
    public string Description { get; set; }
    public double WindSpeed { get; set; }

    public WeatherInfo(string cityName, double temperature, string description, double windSpeed)
    {
        CityName = cityName;
        Temperature = temperature;
        Description = description;
        WindSpeed = windSpeed;
    }
}

