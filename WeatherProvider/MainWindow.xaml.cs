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
            var settings = new OpenWeatherMapApiClientSettings() { //инициализация настроек через конфигурационный файл appsettings.json
                Url = new Uri(App.Config["Url"]),
                ApiKey = App.Config["ApiKey"]
            };
            weatherInfoProvider = new WeatherInfoProvider(new OpenWeatherMapApiClient(settings)); //прокидывание в конструктора реализацию IWeatherClient
        }

        private async void CityInputTextBox_EnterKeyDown(object sender, KeyEventArgs e)
        {
            if (exceptionLabel.Content != "") exceptionLabel.Content = ""; //Очистка исключения, если пользователь начал новый ввод
            if (e.Key == Key.Enter)
            {
                try
                {
                    var city = await weatherInfoProvider.GetCityAsync(cityInputTextBox.Text); //нахождеине города по пользовательскому вводу
                    if (city != null)
                    {
                        var weather = await weatherInfoProvider.GetWeatherInfoAsync(city); //нахождение погоды на основании найденного города
                        cityNameLabel.Content = "Название города: " + weather.CityName;
                        temperatureLabel.Content = "Температура: " + weather.Temperature + "°C";
                        descriptionLabel.Content = "Описание: " + weather.Description;
                        windLabel.Content = "Скорость ветра: " + weather.WindSpeed + " м/с";
                    }
                }
                catch (CityNotFoundException exception)
                {
                    exceptionLabel.Content = exception.Message;
                    cityNameLabel.Content = temperatureLabel.Content  //очистка поля вывода
                        = descriptionLabel.Content = windLabel.Content = "";
                }
                catch (HttpRequestException exception)
                {
                    exceptionLabel.Content = "Ошибка: некорректный ответ с сервера";
                    cityNameLabel.Content = temperatureLabel.Content //очистка поля вывода
                        = descriptionLabel.Content = windLabel.Content = "";
                }
                finally
                {
                    cityInputTextBox.Clear(); //очистка TextBox после нажатия Enter и выполнения (вне зависимости от выброшенного исключения) операций
                }
            }
        }
    }
}
