using System.Windows;

namespace falling_words
{
    /// Window with informations about game
    public partial class InfoWindow : Window
    {
        public InfoWindow()
        {
            InitializeComponent();
        }

        /// Go to main menu
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var menuWindow = new MainMenuWindow();
            menuWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            menuWindow.Show();
            this.Close();
        }
    }
}
