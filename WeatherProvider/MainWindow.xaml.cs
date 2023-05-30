using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace WeatherProvider
{
    public partial class MainWindow : Window
    {
        private static WeatherInfoProvider weatherInfoProvider;

        public MainWindow()
        {
            InitializeComponent();
            // Инициализация настроек через конфигурационный файл appsettings.json
            var settings = new OpenWeatherMapApiClientSettings() { 
                Url = new Uri(App.Config["Url"]),
                ApiKey = App.Config["ApiKey"]
            };
            weatherInfoProvider = new WeatherInfoProvider(new OpenWeatherMapApiClient(settings));
        }

        private async void CityInputTextBox_EnterKeyDown(object sender, KeyEventArgs e)
        {
            if (exceptionLabel.Content != "") exceptionLabel.Content = ""; // Очистка исключения, если пользователь начал новый ввод
            if (e.Key == Key.Enter)
            {
                try
                {
                    var city = await weatherInfoProvider.GetCityAsync(cityInputTextBox.Text); // Нахождеине города по пользовательскому вводу
                    if (city != null)
                    {
                        var weather = await weatherInfoProvider.GetWeatherInfoAsync(city); // Нахождение погоды на основании найденного города
                        cityNameLabel.Content = "Название города: " + weather.CityName;
                        temperatureLabel.Content = "Температура: " + weather.Temperature + "°C";
                        descriptionLabel.Content = "Описание: " + weather.Description;
                        windLabel.Content = "Скорость ветра: " + weather.WindSpeed + " м/с";
                    }
                }
                catch (Exception exception)
                    when (exception is CityNotFoundException || exception is HttpRequestException)
                {
                    exceptionLabel.Content = exception.Message;
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
