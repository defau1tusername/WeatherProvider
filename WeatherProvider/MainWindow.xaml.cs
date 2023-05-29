using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeatherProvider
{
    public partial class MainWindow : Window
    {
        private static WeatherInfoProvider weatherInfoProvider;

        public MainWindow()
        {
            InitializeComponent();
            var settings = new OpenWeatherMapApiClientSettings() {
                Url = new Uri(App.Config["Url"]),
                ApiKey = App.Config["ApiKey"]
            };
            weatherInfoProvider = new WeatherInfoProvider(new OpenWeatherMapApiClient(settings));
        }

        private async void CityInputTextBox_EnterKeyDown(object sender, KeyEventArgs e)
        {
            if (exceptionLabel.Content != "") exceptionLabel.Content = "";
            if (e.Key == Key.Enter)
            {
                try
                {
                    var city = await weatherInfoProvider.GetCityAsync(cityInputTextBox.Text);
                    if (city != null)
                    {
                        var weather = await weatherInfoProvider.GetWeatherInfoAsync(city);
                        cityNameLabel.Content = "Название города: " + weather.CityName;
                        temperatureLabel.Content = "Температура: " + weather.Temperature + "°C";
                        descriptionLabel.Content = "Описание: " + weather.Description;
                        windLabel.Content = "Скорость ветра: " + weather.WindSpeed + " м/с";
                    }
                }
                catch (CityNotFoundException exception)
                {
                    exceptionLabel.Content = exception.Message;
                    cityNameLabel.Content = temperatureLabel.Content 
                        = descriptionLabel.Content = windLabel.Content = "";
                }
                catch (HttpRequestException exception)
                {
                    exceptionLabel.Content = "Ошибка: некорректный ответ с сервера";
                    cityNameLabel.Content = temperatureLabel.Content
                        = descriptionLabel.Content = windLabel.Content = "";
                }
                finally
                {
                    cityInputTextBox.Clear();
                }
            }
        }
    }
}
