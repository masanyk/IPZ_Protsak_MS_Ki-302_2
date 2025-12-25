using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using lab2_17.Entity;
using lab2_17.Requests;

namespace lab2_17
{
    public partial class UpdateWeatherPage : Window
    {
        private Weather _weather { get; set; }
        private string _cityName { get; set; }

        public UpdateWeatherPage(Weather weather, string cityName)
        {
            InitializeComponent();
            _weather = weather;
            _cityName = cityName;
            DataContext = weather;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedWeatherCondition = WeatherConditionComboBox.Text;
                if (!string.IsNullOrWhiteSpace(selectedWeatherCondition))
                {
                    _weather.Description = selectedWeatherCondition;

                    if (new UpdateWeather().Update(_weather, _cityName))
                    {
                        MessageBox.Show($"Погода оновлена: {selectedWeatherCondition}, {_weather.Temperature}");
                        DialogResult = true;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Не вдалося оновити погоду.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Оберіть умови погоди", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
