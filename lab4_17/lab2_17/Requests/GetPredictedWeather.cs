using System.Windows;
using lab2_17.Entity;

namespace lab2_17.Requests;

public class GetPredictedWeather
{
    public Weather Get(string city)
    {
        var result = RequestClient.Send<Weather>(new
        {
            command = "get_predicted_weather",
            city
        });

        if (!result.Success)
        {
            MessageBox.Show(result.Message, "Помилка сервера", MessageBoxButton.OK, MessageBoxImage.Error);
            return new Weather();
        }

        return result.Data ?? new Weather();
    }
}
