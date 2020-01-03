using System.Windows;

namespace falling_words
{
    /// <summary>
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        public InfoWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var menuWindow = new MainMenuWindow();
            menuWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            menuWindow.Show();
            this.Close();
        }
    }
}
