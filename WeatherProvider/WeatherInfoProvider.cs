﻿using System.Threading.Tasks;

public class WeatherInfoProvider
{
    private IWeatherClient client;

    public WeatherInfoProvider(IWeatherClient client)
    {
        this.client = client;
    }

    public async Task<string> GetCityAsync(string cityInput)
    {
        var citiesApi = await client.GetCityInfoByInputAsync(cityInput);

        if (citiesApi.Length > 0) return citiesApi[0].Name;
        else throw new CityNotFoundException();
    }

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
