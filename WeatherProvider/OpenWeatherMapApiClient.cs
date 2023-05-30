using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Класс <c>OpenWeatherMapApiClient</c>, назначением которого - отправка запросов
/// </summary>
public class OpenWeatherMapApiClient : IWeatherClient
{
    private readonly HttpClient client;
    private readonly string apiKey;
    private static JsonSerializerOptions jsonSerializerOptions = 
        new() { PropertyNameCaseInsensitive = true };
    public OpenWeatherMapApiClient(OpenWeatherMapApiClientSettings settings)
    {
        client = new HttpClient() { BaseAddress = settings.Url };
        apiKey = settings.ApiKey;
    }

    /// <summary>
    /// Отправка запроса на получение городов с наибольшим лексическим совпадением
    /// </summary>
    public async Task<ApiCity[]> GetInfoByCityNameAsync(string cityInput, CancellationToken cancellationToken)
    {
        var url = @$"/geo/1.0/direct?q={cityInput}&limit=5&appid={apiKey}";
        var response = await client.GetAsync(url);
        var cityApi = await JsonSerializer.DeserializeAsync<ApiCity[]>(
            await response.Content.ReadAsStreamAsync(),
            jsonSerializerOptions);

        return cityApi;
    }

    /// <summary>
    /// Отправка запроса на получение информации о погоде
    /// </summary>
    public async Task<ApiWeatherInfo> GetWeatherInfoAsync(string city, CancellationToken cancellationToken)
    {
        var url = @$"/data/2.5/weather?q={city}&appid={apiKey}&lang=ru&units=metric";
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var weatherInfoApi = await JsonSerializer.DeserializeAsync<ApiWeatherInfo>(
            await response.Content.ReadAsStreamAsync(),
            jsonSerializerOptions);

        if (weatherInfoApi.Name == null 
            || weatherInfoApi.Main == null 
            || weatherInfoApi.Weather == null) throw new CityNotFoundException();

        return weatherInfoApi;
    }
}

