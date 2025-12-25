using System;
using System.Windows;
using System.Windows.Controls;
using lab2_17.Entity;

namespace lab2_17
{
    public partial class WeatherDetailsWindow : Window
    {
        public WeatherDetailsViewModel ViewModel { get; set; }
        private string _cityName { get; set; }

        public WeatherDetailsWindow(string city)
        {
            InitializeComponent();
            _cityName = city;
            ViewModel = new WeatherDetailsViewModel(_cityName);
            DataContext = ViewModel;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ShowWeatherButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && button.CommandParameter is Weather weather)
                {
                    var upd = new UpdateWeatherPage(weather, _cityName);
                    upd.Show();
                }
                else
                {
                    MessageBox.Show("Не вдалося знайти дані про погоду.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}