using lab2_17.Entity;

namespace lab2_17.Requests;

public class GetPredictedWeather
{
    public Weather Get(string city)
    {
        return new Weather
        {
            Date = "19.11.2024",
            Temperature = "12°C",
            Description = "Сонячно з невеликими хмарами"
        };
    }
}