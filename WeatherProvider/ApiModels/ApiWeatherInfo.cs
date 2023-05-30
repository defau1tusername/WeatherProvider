public class ApiWeatherInfo
{
    public string Name { get; set; }
    public ApiWeather[] Weather { get; set; }
    public ApiMain Main { get; set; }
    public ApiWind Wind { get; set; }
}