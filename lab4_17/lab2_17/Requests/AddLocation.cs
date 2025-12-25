using System.Windows;

namespace lab2_17.Requests;

public class AddLocation
{
    public bool Add(string location)
    {
        var result = RequestClient.Send<object>(new
        {
            command = "add_location",
            city = location
        });

        if (!result.Success)
        {
            MessageBox.Show(result.Message, "Помилка сервера", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        return true;
    }
}
