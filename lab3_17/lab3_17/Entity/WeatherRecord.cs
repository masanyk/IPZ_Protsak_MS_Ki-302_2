namespace lab3_17.Entity;

public class WeatherRecord
{
    public int Id { get; set; }
    public string Date { get; set; } = string.Empty;
    public string Temperature { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsPredicted { get; set; }
    
    public int CityId { get; set; }
    public City City { get; set; }
}
