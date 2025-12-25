using System.Windows;
using lab2_17.Entity;

namespace lab2_17.Requests;

public class UpdateWeather
{
    public bool Update(Weather weather, string city)
    {
        var result = RequestClient.Send<object>(new
        {
            command = "update_weather",
            city,
            date = weather.Date,
            temperature = weather.Temperature,
            description = weather.Description
        });

        if (!result.Success)
        {
            MessageBox.Show(result.Message, "Помилка сервера", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        return true;
    }
}
