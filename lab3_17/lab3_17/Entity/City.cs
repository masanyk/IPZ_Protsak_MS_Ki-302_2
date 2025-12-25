namespace lab3_17.Entity;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<WeatherRecord> WeatherRecords { get; set; } = new();
}
