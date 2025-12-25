namespace lab3_17.Services;

public class WeatherDto
{
    public string City { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Temperature { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsPredicted { get; set; }
}
