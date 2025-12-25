using lab3_17.Entity;
using Microsoft.EntityFrameworkCore;

namespace lab3_17;

public class AppDbContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    public DbSet<WeatherRecord> WeatherRecords { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<City>().HasData(
            new City { Id = 1, Name = "Київ" },
            new City { Id = 2, Name = "Львів" },
            new City { Id = 3, Name = "Одеса" },
            new City { Id = 4, Name = "Харків" },
            new City { Id = 5, Name = "Дніпро" }
        );

        modelBuilder.Entity<WeatherRecord>().HasData(
            new WeatherRecord { Id = 1, CityId = 1, Date = "2024-12-01", Temperature = "+6°C", Description = "сонячно", IsPredicted = false },
            new WeatherRecord { Id = 2, CityId = 1, Date = "2024-12-02", Temperature = "+5°C", Description = "легкий дощ", IsPredicted = false },
            new WeatherRecord { Id = 3, CityId = 1, Date = "2024-12-03", Temperature = "+4°C", Description = "хмарно", IsPredicted = true },

            new WeatherRecord { Id = 4, CityId = 2, Date = "2024-12-01", Temperature = "+3°C", Description = "туман", IsPredicted = false },
            new WeatherRecord { Id = 5, CityId = 2, Date = "2024-12-02", Temperature = "+4°C", Description = "сонячно", IsPredicted = false },
            new WeatherRecord { Id = 6, CityId = 2, Date = "2024-12-03", Temperature = "+2°C", Description = "дощ", IsPredicted = true },

            new WeatherRecord { Id = 7, CityId = 3, Date = "2024-12-01", Temperature = "+10°C", Description = "сонячно", IsPredicted = false },
            new WeatherRecord { Id = 8, CityId = 3, Date = "2024-12-02", Temperature = "+9°C", Description = "вітер", IsPredicted = false },
            new WeatherRecord { Id = 9, CityId = 3, Date = "2024-12-03", Temperature = "+8°C", Description = "хмарно", IsPredicted = true },

            new WeatherRecord { Id = 10, CityId = 4, Date = "2024-12-01", Temperature = "+4°C", Description = "дощ", IsPredicted = false },
            new WeatherRecord { Id = 11, CityId = 4, Date = "2024-12-02", Temperature = "+5°C", Description = "хмарно", IsPredicted = false },
            new WeatherRecord { Id = 12, CityId = 4, Date = "2024-12-03", Temperature = "+6°C", Description = "сонячно", IsPredicted = true },

            new WeatherRecord { Id = 13, CityId = 5, Date = "2024-12-01", Temperature = "+7°C", Description = "сонячно", IsPredicted = false },
            new WeatherRecord { Id = 14, CityId = 5, Date = "2024-12-02", Temperature = "+6°C", Description = "легкий вітер", IsPredicted = false },
            new WeatherRecord { Id = 15, CityId = 5, Date = "2024-12-03", Temperature = "+5°C", Description = "хмарно", IsPredicted = true }
        );
    }
}
