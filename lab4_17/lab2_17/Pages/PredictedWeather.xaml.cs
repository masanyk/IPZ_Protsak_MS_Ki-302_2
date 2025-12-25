using System.Windows;
using lab2_17.Entity;
using lab2_17.Requests;

namespace lab2_17.Pages
{
    public partial class PredictedWeather : Window
    {
        public string CityName {get; set;}
        public Weather Weather {get; set;}

        public PredictedWeather(string cityName)
        {
            InitializeComponent();
            CityName = cityName;
            Weather = new GetPredictedWeather().Get(CityName);
            DataContext = this;
        }
    }
}