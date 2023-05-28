using System.Threading.Tasks;

public interface IWeatherProvider
{
    Task<string> FindCityNameAsync(string cityInput);
    Task<WeatherInfo> GetWeatherInfoAsync(string city);
}
