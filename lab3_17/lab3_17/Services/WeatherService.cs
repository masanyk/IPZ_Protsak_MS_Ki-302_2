using lab3_17.Entity;
using Microsoft.EntityFrameworkCore;

namespace lab3_17.Services;

public class WeatherService
{
    private readonly AppDbContext _context;

    public WeatherService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(bool Success, string Message, List<string>? Cities)> GetCitiesAsync()
    {
        var cities = await _context.Cities
            .OrderBy(c => c.Name)
            .Select(c => c.Name)
            .ToListAsync();

        if (cities.Count == 0)
            return (false, "Список міст порожній.", null);

        return (true, "Список міст отримано.", cities);
    }

    public async Task<(bool Success, string Message, List<WeatherDto>? Records)> GetWeatherHistoryAsync(string cityName)
    {
        if (string.IsNullOrWhiteSpace(cityName))
            return (false, "Місто не вказано.", null);

        if (!IsLengthValid(cityName, 2, 50))
            return (false, "Назва міста повинна бути від 2 до 50 символів.", null);

        var city = (await _context.Cities
                .Include(c => c.WeatherRecords)
                .ToListAsync())
            .FirstOrDefault(c => string.Equals(c.Name, cityName, StringComparison.InvariantCultureIgnoreCase));

        if (city == null)
            return (false, "Місто не знайдено.", null);

        var history = city.WeatherRecords
            .Where(r => !r.IsPredicted)
            .OrderByDescending(r => r.Date)
            .Select(MapRecord)
            .ToList();

        if (history.Count == 0)
            return (false, "Історія погоди відсутня.", null);

        return (true, "Історія погоди отримана.", history);
    }

    public async Task<(bool Success, string Message, WeatherDto? Record)> GetPredictedWeatherAsync(string cityName)
    {
        if (string.IsNullOrWhiteSpace(cityName))
            return (false, "Місто не вказано.", null);

        if (!IsLengthValid(cityName, 2, 50))
            return (false, "Назва міста повинна бути від 2 до 50 символів.", null);

        var city = (await _context.Cities
                .Include(c => c.WeatherRecords)
                .ToListAsync())
            .FirstOrDefault(c => string.Equals(c.Name, cityName, StringComparison.InvariantCultureIgnoreCase));

        if (city == null)
            return (false, "Місто не знайдено.", null);

        var predicted = city.WeatherRecords
            .Where(r => r.IsPredicted)
            .OrderByDescending(r => r.Date)
            .FirstOrDefault();

        if (predicted != null)
            return (true, "Прогноз погоди отримано.", MapRecord(predicted));

        var fallback = new WeatherRecord
        {
            CityId = city.Id,
            Date = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"),
            Temperature = "+5°C",
            Description = "дані відсутні, прогноз тимчасовий",
            IsPredicted = true
        };

        _context.WeatherRecords.Add(fallback);
        await _context.SaveChangesAsync();

        fallback.City = city;
        return (true, "Прогноз погоди згенеровано.", MapRecord(fallback));
    }

    public async Task<(bool Success, string Message, City? City)> AddLocationAsync(string cityName)
    {
        if (string.IsNullOrWhiteSpace(cityName))
            return (false, "Назва міста не може бути порожньою.", null);

        if (!IsLengthValid(cityName, 2, 50))
            return (false, "Назва міста повинна бути від 2 до 50 символів.", null);

        var exists = (await _context.Cities.ToListAsync())
            .Any(c => string.Equals(c.Name, cityName, StringComparison.InvariantCultureIgnoreCase));
        if (exists)
            return (false, "Таке місто вже існує.", null);

        var city = new City { Name = cityName.Trim() };
        _context.Cities.Add(city);
        await _context.SaveChangesAsync();

        return (true, "Місто додано.", city);
    }

    public async Task<(bool Success, string Message, WeatherDto? Record)> SaveWeatherAsync(
        string cityName, string temperature, string description)
    {
        if (string.IsNullOrWhiteSpace(cityName))
            return (false, "Місто не вказано.", null);

        if (!IsLengthValid(cityName, 2, 50))
            return (false, "Назва міста повинна бути від 2 до 50 символів.", null);

        if (string.IsNullOrWhiteSpace(temperature) || temperature.Length > 10)
            return (false, "Температура повинна бути вказана і не перевищувати 10 символів.", null);

        if (!IsLengthValid(description, 2, 100))
            return (false, "Опис має бути від 2 до 100 символів.", null);

        var city = (await _context.Cities.ToListAsync())
            .FirstOrDefault(c => string.Equals(c.Name, cityName, StringComparison.InvariantCultureIgnoreCase));
        if (city == null)
            return (false, "Місто не знайдено.", null);

        var record = new WeatherRecord
        {
            CityId = city.Id,
            Date = DateTime.Now.ToString("yyyy-MM-dd"),
            Temperature = string.IsNullOrWhiteSpace(temperature) ? "N/A" : temperature,
            Description = string.IsNullOrWhiteSpace(description) ? "без опису" : description,
            IsPredicted = false
        };

        _context.WeatherRecords.Add(record);
        await _context.SaveChangesAsync();

        record.City = city;
        return (true, "Погоду збережено.", MapRecord(record));
    }

    public async Task<(bool Success, string Message, WeatherDto? Record)> UpdateWeatherAsync(
        string cityName, string date, string temperature, string description)
    {
        if (string.IsNullOrWhiteSpace(cityName))
            return (false, "Місто не вказано.", null);

        if (string.IsNullOrWhiteSpace(date))
            return (false, "Дата не вказана.", null);

        if (!IsLengthValid(cityName, 2, 50))
            return (false, "Назва міста повинна бути від 2 до 50 символів.", null);

        if (!IsValidDate(date))
            return (false, "Дата повинна бути у форматі yyyy-MM-dd.", null);

        if (!string.IsNullOrWhiteSpace(temperature) && temperature.Length > 10)
            return (false, "Температура не повинна перевищувати 10 символів.", null);

        if (!string.IsNullOrWhiteSpace(description) && !IsLengthValid(description, 2, 100))
            return (false, "Опис має бути від 2 до 100 символів.", null);

        var city = (await _context.Cities.ToListAsync())
            .FirstOrDefault(c => string.Equals(c.Name, cityName, StringComparison.InvariantCultureIgnoreCase));
        if (city == null)
            return (false, "Місто не знайдено.", null);

        var record = await _context.WeatherRecords
            .FirstOrDefaultAsync(r => r.CityId == city.Id && r.Date == date);

        if (record == null)
            return (false, "Запис з цією датою не знайдено.", null);

        if (!string.IsNullOrWhiteSpace(temperature))
            record.Temperature = temperature;

        if (!string.IsNullOrWhiteSpace(description))
            record.Description = description;

        await _context.SaveChangesAsync();

        record.City = city;
        return (true, "Погоду оновлено.", MapRecord(record));
    }

    private static string NormalizeCity(string value)
    {
        return value.Trim().ToLowerInvariant();
    }

    private static WeatherDto MapRecord(WeatherRecord record)
    {
        return new WeatherDto
        {
            City = record.City?.Name ?? string.Empty,
            Date = record.Date,
            Temperature = record.Temperature,
            Description = record.Description,
            IsPredicted = record.IsPredicted
        };
    }

    private static bool IsLengthValid(string? value, int min, int max)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;
        var len = value.Trim().Length;
        return len >= min && len <= max;
    }

    private static bool IsValidDate(string value)
    {
        return DateTime.TryParseExact(value, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _);
    }
}
