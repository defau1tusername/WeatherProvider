using System.Threading;
using System.Threading.Tasks;

public interface IWeatherClient
{
    Task<ApiCity[]> GetInfoByCityNameAsync(string cityInput, CancellationToken cancellationToken = default);
    Task<ApiWeatherInfo> GetWeatherInfoAsync(string city, CancellationToken cancellationToken = default);
}
