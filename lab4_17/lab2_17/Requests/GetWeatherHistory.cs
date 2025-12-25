using System.Collections.ObjectModel;
using System.Windows;
using lab2_17.Entity;

namespace lab2_17.Requests
{
    public class GetWeatherHistory
    {
        public ObservableCollection<Weather> WeatherHistory(string city)
        {
            var result = RequestClient.Send<List<Weather>>(new
            {
                command = "get_weather_history",
                city
            });

            if (!result.Success)
            {
                MessageBox.Show(result.Message, "Помилка сервера", MessageBoxButton.OK, MessageBoxImage.Error);
                return new ObservableCollection<Weather>();
            }

            var history = result.Data ?? new List<Weather>();
            return new ObservableCollection<Weather>(history);
        }
    }
}
