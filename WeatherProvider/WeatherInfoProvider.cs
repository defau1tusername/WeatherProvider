using System.Threading.Tasks;

/// <summary>
/// Класс <c>WeatherInfoProvider</c>, назначением которого является конвертация полученных API моделей в удобную форму
/// </summary>
public class WeatherInfoProvider
{
    private readonly IWeatherClient client;

    public WeatherInfoProvider(IWeatherClient client)
    {
        this.client = client;
    }

    /// <summary>
    /// Получение города с наибольшим лексическим совпадением
    /// </summary>
    public async Task<string> GetCityAsync(string value)
    {
        var foundCities = await client.GetInfoByCityNameAsync(value);

        if (foundCities.Length == 0) throw new CityNotFoundException();
        return foundCities[0].Name;
    }

    /// <summary>
    /// Получение информации о погоде
    /// </summary>
    public async Task<WeatherInfo> GetWeatherInfoAsync(string city)
    {
        var weatherInfoApi = await client.GetWeatherInfoAsync(city);
        var weatherInfo = new WeatherInfo(
            weatherInfoApi.Name,
            weatherInfoApi.Main.Temp,
            weatherInfoApi.Weather[0].Description,
            weatherInfoApi.Wind.Speed);

        return weatherInfo;
    }
}