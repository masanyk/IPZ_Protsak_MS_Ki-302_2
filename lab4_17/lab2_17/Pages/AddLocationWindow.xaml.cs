using System.Windows;

namespace lab2_17
{
    public partial class AddLocationWindow : Window
    {
        public string CityName { get; private set; }

        public AddLocationWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CityName = CityNameTextBox.Text;
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}