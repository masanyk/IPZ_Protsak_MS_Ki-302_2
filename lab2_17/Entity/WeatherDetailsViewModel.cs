using System.Collections.ObjectModel;
using System.ComponentModel;
using lab2_17.Requests;

namespace lab2_17.Entity
{
    public class WeatherDetailsViewModel : INotifyPropertyChanged
    {
        public string SelectedCity { get; }
        public ObservableCollection<Weather> WeatherHistory { get; set; }

        public WeatherDetailsViewModel(string city)
        {
            SelectedCity = city;
            WeatherHistory = new GetWeatherHistory().WeatherHistory(SelectedCity);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}