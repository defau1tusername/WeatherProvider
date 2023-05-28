using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


public class OpenWeatherMapApi : IWeatherProvider
{
    private readonly string apiKey;
    private readonly HttpClient client;
    
    public OpenWeatherMapApi()
    {
        apiKey = "696565622336d9538c60b83c1618abc3";
        client = new HttpClient();
    }

    public async Task<string> FindCityNameAsync(string cityInput)
    {
        var url = @$"http://api.openweathermap.org/geo/1.0/direct?q={cityInput}&limit=5&appid={apiKey}";
        var response = await client.GetAsync(url);
        var cityApi = await JsonSerializer.DeserializeAsync<CityApi[]>(
            await response.Content.ReadAsStreamAsync(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return cityApi.Length > 0 
            ? cityApi[0].Name 
            : null;
    }

    public async Task<WeatherInfo> GetWeatherInfoAsync(string city)
    {
        var url = @$"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&lang=ru";
        var response = await client.GetAsync(url);
        var weatherInfoApi = await JsonSerializer.DeserializeAsync<WeatherInfoApi>(
            await response.Content.ReadAsStreamAsync(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var weatherInfo = new WeatherInfo(
            weatherInfoApi.Name,
            weatherInfoApi.Main.Temp,
            weatherInfoApi.Weather[0].Description);

        return weatherInfo;
    }

}

