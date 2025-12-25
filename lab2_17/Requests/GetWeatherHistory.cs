using System.Collections.ObjectModel;
using lab2_17.Entity;

namespace lab2_17.Requests
{
    public class GetWeatherHistory
    {
        public ObservableCollection<Weather> WeatherHistory(string city)
        {
            return new ObservableCollection<Weather>
            {
                new Weather { Date = "2024-10-15", Temperature = "+17°C", Description = "сонячно" },
                new Weather { Date = "2024-10-14", Temperature = "+16°C", Description = "дощ" },
                new Weather { Date = "2024-10-13", Temperature = "+18°C", Description = "хмарно" },
                new Weather { Date = "2024-10-12", Temperature = "+15°C", Description = "вітер" },
                new Weather { Date = "2024-10-11", Temperature = "+19°C", Description = "сонячно" }
            };
        }
    }
}