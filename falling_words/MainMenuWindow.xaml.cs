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
            int startSpeed = 0;
            int endSpeed;
            int gameTime;
            int correctField = 0;
            if (Int32.TryParse(StartSpeed.Text, out startSpeed) == true 
                && startSpeed >= 60
                && startSpeed <= 1000)
            {
                StartSpeedErrorLabel.Visibility = Visibility.Hidden;
                correctField++;
            }
            else
            {
                StartSpeedErrorLabel.Visibility = Visibility.Visible;
            }

            if (Int32.TryParse(EndSpeed.Text, out endSpeed) == true
                && endSpeed >= 60
                && endSpeed <= 1000
                && endSpeed >= startSpeed)
            {
                EndSpeedErrorLabel.Visibility = Visibility.Hidden;
                correctField++;
            }
            else
            {
                EndSpeedErrorLabel.Visibility = Visibility.Visible;
                if(endSpeed < startSpeed)
                {
                    StartSpeedErrorLabel.Visibility = Visibility.Visible;
                }
            }

            if (Int32.TryParse(GameTime.Text, out gameTime) == true
                   && gameTime >= 30
                   && gameTime <= 300)
            {
                TimeErrorLabel.Visibility = Visibility.Hidden;
                correctField++;
            }
            else
            {
                TimeErrorLabel.Visibility = Visibility.Visible;
            }

            if(correctField == 3)
            {
                if( endSpeed < startSpeed)
                {
                    StartSpeedErrorLabel.Visibility = Visibility.Visible;
                    EndSpeedErrorLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    int wordLength = GetWordLengthValue();
                    var gameWindow = new MainWindow(new LevelSettings(startSpeed, endSpeed, wordLength, gameTime));
                    gameWindow.Show();
                    this.Close();
                }
            }

        }

        private int GetWordLengthValue()
        {
            if (Radio3.IsChecked == true) return 3;
            if (Radio4.IsChecked == true) return 4;
            if (Radio5.IsChecked == true) return 5;
            if (Radio6.IsChecked == true) return 6;
            if (Radio7.IsChecked == true) return 7;

            throw new ArgumentOutOfRangeException("Non radio button is checked.");
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ComboBox.SelectedIndex == 0)
            {
                StartSpeed.Text = "60";
                EndSpeed.Text = "210";
                GameTime.Text = "30";
                Radio3.IsChecked = true;

                SetIsEnabledForAllFields(false);

                StartButon.IsEnabled = true;
            }
            if (ComboBox.SelectedIndex == 1)
            {
                StartSpeed.Text = "60";
                EndSpeed.Text = "210";
                GameTime.Text = "30";
                Radio4.IsChecked = true;

                SetIsEnabledForAllFields(false);

                StartButon.IsEnabled = true;
            }
            if (ComboBox.SelectedIndex == 2)
            {
                StartSpeed.Text = "210";
                EndSpeed.Text = "460";
                GameTime.Text = "30";
                Radio3.IsChecked = true;

                SetIsEnabledForAllFields(false);

                StartButon.IsEnabled = true;
            }
            if (ComboBox.SelectedIndex == 3)
            {
                SetIsEnabledForAllFields(true);

                StartButon.IsEnabled = true;
            }
        }

        private void SetIsEnabledForAllFields(bool setTo)
        {
            StartSpeed.IsEnabled = setTo;
            EndSpeed.IsEnabled = setTo;
            GameTime.IsEnabled = setTo;
            Radio3.IsEnabled = setTo;
            Radio4.IsEnabled = setTo;
            Radio5.IsEnabled = setTo;
            Radio6.IsEnabled = setTo;
            Radio7.IsEnabled = setTo;
        }
    }
}
