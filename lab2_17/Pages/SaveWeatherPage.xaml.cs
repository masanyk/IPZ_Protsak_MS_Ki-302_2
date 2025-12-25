using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using lab2_17.Entity;
using lab2_17.Requests;

namespace lab2_17
{
    public partial class SaveWeatherPage : Window, INotifyPropertyChanged
    {
        private Weather _weather;
        private string _cityName;

        public Weather Weather
        {
            get => _weather;
            set
            {
                _weather = value;
                OnPropertyChanged();
            }
        }

        public string CityName
        {
            get => _cityName;
            set
            {
                _cityName = value;
                OnPropertyChanged();
            }
        }

        public SaveWeatherPage(string cityName)
        {
            InitializeComponent();
            CityName = cityName;
            Weather = new Weather();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DetailsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (new SaveWeather().Save(CityName, Weather.Temperature, Weather.Description))
                {
                    MessageBox.Show($"Нова погода у {CityName}: {Weather.Temperature}, {Weather.Description}");
                    Close();
                }
                else
                {
                    MessageBox.Show("Не вдалося зберегти дані про погоду.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public object WeatherCondition
        {
            get => Weather.Description;
            set
            {
                Weather.Description = (value as ComboBoxItem)?.Content.ToString();
                OnPropertyChanged();
            }
        }
    }
}
