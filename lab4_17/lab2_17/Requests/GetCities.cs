using System.Collections.ObjectModel;
using System.Windows;

namespace lab2_17.Requests;

public class GetCities
{
    public ObservableCollection<string> Get()
    {
        var result = RequestClient.Send<List<string>>(new { command = "get_cities" });
        if (!result.Success)
        {
            MessageBox.Show(result.Message, "Помилка сервера", MessageBoxButton.OK, MessageBoxImage.Error);
            return new ObservableCollection<string>();
        }

        var cities = result.Data ?? new List<string>();
        return new ObservableCollection<string>(cities);
    }
}
