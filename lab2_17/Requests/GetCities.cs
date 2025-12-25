using System.Collections.ObjectModel;

namespace lab2_17.Requests;

public class GetCities
{
    public ObservableCollection<string> Get()
    {
        return new ObservableCollection<string>
        {
            "Київ",
            "Львів",
            "Одеса",
            "Харків",
            "Дніпро"
        };
    }
}