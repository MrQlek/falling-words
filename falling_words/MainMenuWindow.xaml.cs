using System;
using System.Windows;
using System.Windows.Controls;

namespace falling_words
{
    /*! \mainpage Dokumentacja gry 
 * 
 * \section intro_sec Tytuł gry: 
 * 
 * Falling words
 * 
 * \section install_sec Wykonał:
 * 
 * Kacper Czajkowski 171617
 * 
 */

    /// Window with menu
    public partial class MainMenuWindow : Window
    {
        public MainMenuWindow()
        {
            InitializeComponent();
        }

        /// Check inputs values and go to game window
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int startSpeed = 0;
            int endSpeed;
            int gameTime;
            int correctField = 0;
            if (Int32.TryParse(StartSpeed.Text, out startSpeed) == true 
                && startSpeed >= 100
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
                && endSpeed >= 100
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
                    gameWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    gameWindow.Show();
                    this.Close();
                }
            }

        }

        /// Getting radio button value
        private int GetWordLengthValue()
        {
            if (Radio3.IsChecked == true) return 3;
            if (Radio4.IsChecked == true) return 4;
            if (Radio5.IsChecked == true) return 5;
            if (Radio6.IsChecked == true) return 6;
            if (Radio7.IsChecked == true) return 7;

            throw new ArgumentOutOfRangeException("Non radio button is checked.");
        }


        /// Set level settings depending on selected level
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ComboBox.SelectedIndex == 0)
            {
                StartSpeed.Text = "100";
                EndSpeed.Text = "100";
                GameTime.Text = "30";
                Radio5.IsChecked = true;

                SetIsEnabledForAllFields(false);

                StartButon.IsEnabled = true;
            }
            if (ComboBox.SelectedIndex == 1)
            {
                StartSpeed.Text = "100";
                EndSpeed.Text = "100";
                GameTime.Text = "30";
                Radio4.IsChecked = true;

                SetIsEnabledForAllFields(false);

                StartButon.IsEnabled = true;
            }
            if (ComboBox.SelectedIndex == 2)
            {
                StartSpeed.Text = "100";
                EndSpeed.Text = "100";
                GameTime.Text = "30";
                Radio3.IsChecked = true;

                SetIsEnabledForAllFields(false);

                StartButon.IsEnabled = true;
            }
            if (ComboBox.SelectedIndex == 3)
            {
                StartSpeed.Text = "250";
                EndSpeed.Text = "250";
                GameTime.Text = "30";
                Radio5.IsChecked = true;

                SetIsEnabledForAllFields(false);

                StartButon.IsEnabled = true;
            }
            if (ComboBox.SelectedIndex == 4)
            {
                StartSpeed.Text = "250";
                EndSpeed.Text = "250";
                GameTime.Text = "30";
                Radio4.IsChecked = true;

                SetIsEnabledForAllFields(false);

                StartButon.IsEnabled = true;
            }
            if (ComboBox.SelectedIndex == 5)
            {
                StartSpeed.Text = "250";
                EndSpeed.Text = "250";
                GameTime.Text = "30";
                Radio3.IsChecked = true;

                SetIsEnabledForAllFields(false);

                StartButon.IsEnabled = true;
            }
            if (ComboBox.SelectedIndex == 6)
            {
                StartSpeed.Text = "300";
                EndSpeed.Text = "300";
                GameTime.Text = "60";
                Radio4.IsChecked = true;

                SetIsEnabledForAllFields(false);

                StartButon.IsEnabled = true;
            }
            if (ComboBox.SelectedIndex == 7)
            {
                StartSpeed.Text = "425";
                EndSpeed.Text = "425";
                GameTime.Text = "60";
                Radio5.IsChecked = true;

                SetIsEnabledForAllFields(false);

                StartButon.IsEnabled = true;
            }
            if (ComboBox.SelectedIndex == 8)
            {
                StartSpeed.Text = "425";
                EndSpeed.Text = "425";
                GameTime.Text = "60";
                Radio4.IsChecked = true;

                SetIsEnabledForAllFields(false);

                StartButon.IsEnabled = true;
            }
            if (ComboBox.SelectedIndex == 9)
            {
                StartSpeed.Text = "600";
                EndSpeed.Text = "600";
                GameTime.Text = "90";
                Radio4.IsChecked = true;

                SetIsEnabledForAllFields(false);

                StartButon.IsEnabled = true;
            }
            if (ComboBox.SelectedIndex == 10)
            {
                SetIsEnabledForAllFields(true);

                StartButon.IsEnabled = true;
            }
        }

        /// Changing Is Enabled propertis for all fields in window
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

        /// Go to information window
        private void StartButon_Copy_Click(object sender, RoutedEventArgs e)
        {
            var infoWindow = new InfoWindow();
            infoWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            infoWindow.Show();
            this.Close();
        }
    }
}
