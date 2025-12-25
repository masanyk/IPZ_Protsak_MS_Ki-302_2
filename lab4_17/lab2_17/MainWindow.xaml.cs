using System.Windows;

namespace lab2_17
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new WeatherViewModel();
        }
    }
}