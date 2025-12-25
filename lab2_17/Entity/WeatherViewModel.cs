using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using lab2_17;
using lab2_17.Pages;
using lab2_17.Requests;

public class WeatherViewModel : INotifyPropertyChanged
{
    private string _selectedCity;
    private string _weatherInfo;
    public ObservableCollection<string> Cities { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
    public ICommand OpenWeatherDetailsCommand { get; }
    public ICommand SaveWeatherCommand { get; }
    public ICommand PredictWeatherCommand { get; }

    public WeatherViewModel()
    {
        Cities = new GetCities().Get();

        // Ініціалізуємо команди
        AddLocationCommand = new RelayCommand(AddLocation);
        OpenWeatherDetailsCommand = new RelayCommand(OpenWeatherDetails, CanOpenWeatherDetails);
        SaveWeatherCommand = new RelayCommand(SaveWeather, CanSaveWeather);
        PredictWeatherCommand = new RelayCommand(PredictWeather, CanPredictWeather);
    }

    private void OpenWeatherDetails()
    {
        try
        {
            if (!string.IsNullOrEmpty(SelectedCity))
            {
                var detailsWindow = new WeatherDetailsWindow(SelectedCity);
                detailsWindow.ShowDialog();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Сталася помилка при відкритті деталей погоди: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void SaveWeather()
    {
        try
        {
            if (!string.IsNullOrEmpty(SelectedCity))
            {
                var saveWindow = new SaveWeatherPage(SelectedCity);
                saveWindow.ShowDialog();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Сталася помилка при збереженні погоди: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private bool CanSaveWeather() // Додано новий метод
    {
        return !string.IsNullOrEmpty(SelectedCity);
    }

    private bool CanOpenWeatherDetails()
    {
        return !string.IsNullOrEmpty(SelectedCity);
    }

    public string SelectedCity
    {
        get { return _selectedCity; }
        set
        {
            _selectedCity = value;
            OnPropertyChanged(nameof(SelectedCity));
            WeatherInfo = GetWeatherForCity(_selectedCity);
        }
    }

    public string WeatherInfo
    {
        get { return _weatherInfo; }
        set
        {
            _weatherInfo = value;
            OnPropertyChanged(nameof(WeatherInfo));
        }
    }

    public ICommand AddLocationCommand { get; }

    private void AddLocation()
    {
        try
        {
            AddLocationWindow addLocationWindow = new AddLocationWindow();
            if (addLocationWindow.ShowDialog() == true)
            {
                string newCity = addLocationWindow.CityName;
                if (new AddLocation().Add(newCity) && !string.IsNullOrEmpty(newCity) && !Cities.Contains(newCity))
                {
                    Cities.Add(newCity);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Сталася помилка при додаванні міста: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private string GetWeatherForCity(string city)
    {
        switch (city)
        {
            case "Київ":
                return "Погода у Києві: сонячно, +18°C";
            case "Львів":
                return "Погода у Львові: хмарно, +15°C";
            case "Одеса":
                return "Погода в Одесі: вітряно, +20°C";
            case "Харків":
                return "Погода у Харкові: дощ, +12°C";
            case "Дніпро":
                return "Погода у Дніпрі: сонячно, +19°C";
            default:
                return "Немає даних для вибраного міста.";
        }
    }

    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    private bool CanPredictWeather()
    {
        return !string.IsNullOrEmpty(SelectedCity);
    }

    private void PredictWeather()
    {
        try
        {
            if (!string.IsNullOrEmpty(SelectedCity))
            {
                var predicred = new PredictedWeather(SelectedCity);
                predicred.Show();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Сталася помилка при прогнозуванні погоди: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
