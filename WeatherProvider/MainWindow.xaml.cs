using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static IWeatherProvider weatherProvider = new OpenWeatherMapApi();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void cityInputTextBox_EnterKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    var city = await weatherProvider.FindCityNameAsync(cityInputTextBox.Text);
                    if (city != null)
                    {
                        var weather = await weatherProvider.GetWeatherInfoAsync(city);
                        cityNameLabel.Content = "Название города: " + weather.CityName;
                        temperatureLabel.Content = "Температура: " + weather.Temperature + "°C";
                        descriptionLabel.Content = "Описание: " + weather.Description;
                    }
                }
                catch (CityNotFoundException exception)
                {
                    MessageBox.Show(exception.Message);
                }
                

                cityInputTextBox.Clear();
            }
        }
    }
}
