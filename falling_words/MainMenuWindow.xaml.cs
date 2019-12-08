using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace falling_words
{
    /// <summary>
    /// Interaction logic for MainMenuWindow.xaml
    /// </summary>
    public partial class MainMenuWindow : Window
    {
        public MainMenuWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int startSpeed = Int32.Parse(StartSpeed.Text);
            int endSpeed = Int32.Parse(EndSpeed.Text);
            int wordLength = Int32.Parse(WordLength.Text);
            int gameTime = Int32.Parse(GameTime.Text);
            var gameWindow = new MainWindow(startSpeed, endSpeed, wordLength, gameTime);
            gameWindow.Show();
            this.Close();
        }
    }
}
