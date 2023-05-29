using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public class OpenWeatherMapApiClient : IWeatherClient
{
    private readonly HttpClient client;
    private readonly string apiKey;
    private static JsonSerializerOptions jsonSerializerOptions = 
        new() { PropertyNameCaseInsensitive = true }; //поскольку имена свойств должны отвечать принятым правилам написания 
                                                      //(с большой буквы для public свойств) необходимо задать настройку игнорирования регистра
    public OpenWeatherMapApiClient(OpenWeatherMapApiClientSettings settings)
    {
        client = new HttpClient() { BaseAddress = settings.Url };
        apiKey = settings.ApiKey;
    }

    public async Task<ApiCity[]> GetInfoByCityNameAsync(string cityInput, CancellationToken cancellationToken = default)
    {
        var url = @$"/geo/1.0/direct?q={cityInput}&limit=5&appid={apiKey}";
        var response = await client.GetAsync(url);
        var cityApi = await JsonSerializer.DeserializeAsync<ApiCity[]>(
            await response.Content.ReadAsStreamAsync(),
            jsonSerializerOptions);

        return cityApi;
    }

    public async Task<ApiWeatherInfo> GetWeatherInfoAsync(string city, CancellationToken cancellationToken = default)
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

