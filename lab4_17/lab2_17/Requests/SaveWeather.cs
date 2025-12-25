using System.Windows;

namespace lab2_17.Requests;

public class SaveWeather
{
    public bool Save(string location, string temperature, string weatherCondition)
    {
        var result = RequestClient.Send<object>(new
        {
            command = "save_weather",
            city = location,
            temperature,
            description = weatherCondition
        });

        if (!result.Success)
        {
            MessageBox.Show(result.Message, "Помилка сервера", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        return true;
    }
}
