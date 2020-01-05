using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace falling_words
{
    /// Game window
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer TimerGenerateNewWord = new DispatcherTimer();
        private readonly DispatcherTimer TimerAnimation = new DispatcherTimer();
        private readonly DispatcherTimer TimerChangeWordsSpeed = new DispatcherTimer();
        private readonly DispatcherTimer TimerCountTime = new DispatcherTimer();
        private readonly DateTime StartTime;

        private int NumberOfWroteChars = 0;
        private readonly LevelSettings Settings;
        private float CharSpeed;
        private readonly float AddToCharSpeed;
        private int Time;
        private int Counter = 1000000;
        private float TimeBetweenWords;
        private readonly List<string> WordList = new List<string>();

        
        /// Initialize game window
        public MainWindow(LevelSettings settings)
        {
            InitializeComponent();

            Settings = settings;
            CharSpeed = Settings.StartSpeed;

            AddToCharSpeed = CountAddToCharSpeed(Settings.GameTime, Settings.StartSpeed, Settings.EndSpeed);
            Time = settings.GameTime;
            StartTime =  DateTime.Now;
            TimeBetweenWords = 60000 / (CharSpeed / Settings.WordsLength);

            RefreshWordSpeed(CharSpeed);
            InitializeTimers();
        }

        /// Inialize all needed timers
        private void InitializeTimers()
        {
            TimerAnimation.Interval = TimeSpan.FromMilliseconds(1);
            TimerAnimation.Tick += SetWordsNewPosition;
            TimerAnimation.Start();

            TimerGenerateNewWord.Interval = TimeSpan.FromMilliseconds(100);
            TimerGenerateNewWord.Tick += GenerateNewWord;
            TimerGenerateNewWord.Start();

            TimerChangeWordsSpeed.Interval = TimeSpan.FromSeconds(1);
            TimerChangeWordsSpeed.Tick += SetNewWordsSpeed;
            TimerChangeWordsSpeed.Start();

            TimerCountTime.Interval = TimeSpan.FromSeconds(1);
            TimerCountTime.Tick += CountTime;
            TimerCountTime.Start();
        }

        /// Stop all timers and disable UserInput field
        private void StopGame()
        {
            TimerAnimation.Stop();
            TimerGenerateNewWord.Stop();
            TimerChangeWordsSpeed.Stop();
            TimerCountTime.Stop();

            UserInput.IsEnabled = false;
        }

        /// counts how fast chars speed will change
        private int CountAddToCharSpeed(int gameTime, int startSpeed, int stopSpeed)
        {
            gameTime -= 10;
            return (int)Math.Ceiling((stopSpeed - startSpeed) / (float)gameTime);
        }

        /// Animate words falling and check if user lost 
        void SetWordsNewPosition(object sender, EventArgs e)
        {
            bool lost = false;
            foreach (var wordLabel in Canvas1.Children.OfType<System.Windows.Controls.Label>())
            {
                Canvas.SetTop(wordLabel, Canvas.GetTop(wordLabel) + 2 + (CharSpeed - 60)/60/4);
                if (Canvas.GetTop(wordLabel) >= 590)
                {
                    lost = true;
                }
            }
            if(lost)
            {
                StopGame();
                ShowEndMessage("You lost. \nTry again or go to the menu \nand pick easier level to train.");
            }
        }

        /// Check if user correctly written a word
        private void UserInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (WordList.Contains(UserInput.Text))
            {
                WordList.Remove(UserInput.Text);
                var labelsToRemove = FindLabelsToRemove(Canvas1, UserInput.Text);
                RemoveLabels(Canvas1, labelsToRemove);
                RefreshResult(UserInput.Text.Length);
                UserInput.Text = "";
            }
        }

        /// Find labels that contains written by user word
        private List<Label> FindLabelsToRemove(Canvas canvas, string text)
        {
            List<Label> wordsToRemove = new List<Label>();
            foreach (var wordLabel in canvas.Children.OfType<System.Windows.Controls.Label>())
            {
                if (text == wordLabel.Content.ToString())
                {
                    wordsToRemove.Add(wordLabel);
                }
            }
            return wordsToRemove;
        }

        /// Remove given labels from canvas
        private void RemoveLabels(Canvas canvas, List<Label> labelsList)
        {
            foreach (Label wordToRemove in labelsList)
            {
                canvas.Children.Remove(wordToRemove);
            }
        }

        /// Count and dispaly average speed
        private void RefreshResult(int wordLength)
        {
            NumberOfWroteChars += wordLength;
            double timeFromStart = (DateTime.Now - StartTime).TotalMinutes;
            ResultLabel.Content = $"Your average speed: \n{Math.Floor(NumberOfWroteChars/timeFromStart)}chars per minute";
        }


        /// Generate new word and add it to Canvas
        private void GenerateNewWord(object sender, EventArgs e)
        {
            Counter += 100;
            if (Counter >= TimeBetweenWords)
            {
                Counter = 0;
                Random rnd = new Random();
                string newWord = Words.GetWord(Settings.WordsLength);
                Label wordLabel = new Label
                {
                    Content = newWord,
                    Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                    FontSize = 28,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(2)
                };
                WordList.Add(newWord);
                Canvas.SetLeft(wordLabel, rnd.Next(675 - (25 * Settings.WordsLength)));
                Canvas.SetTop(wordLabel, 0);
                Canvas1.Children.Add(wordLabel);
                UserInput.Focus();
            }
        }

        /// Count and set new words speed
        private void SetNewWordsSpeed(object sender, EventArgs e)
        {
            CharSpeed += AddToCharSpeed;
            TimeBetweenWords =  60000 / (CharSpeed / Settings.WordsLength);
            RefreshWordSpeed(CharSpeed);
        }

        /// Display actual words speed
        private void RefreshWordSpeed(float charSpeed)
        {
            WordsSpeedLabel.Content = $"Words Speed: \n{Math.Round(charSpeed)} chars per minute";
        }

        /// Go to main menu window
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mainMenuWindow.Show();
            this.Close();
        }

        /// Go to new game window with same level settings
        private void Button_Click_TryAgain(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow(Settings);
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mainWindow.Show();
            this.Close();
        }

        /// Count time and check win condition
        private void CountTime(object sender, EventArgs e)
        {
            Time -= 1;
            SetTimer(Time);
            if (Time == 10)
            {
                TimerChangeWordsSpeed.Stop();
            }
            if(Time == 0)
            {
                StopGame();
                ShowEndMessage("Congratulations! \nYou won this level. \nGo to the menu and choose next level \nor try this one again");
            }
        }

        /// Dispaly clock value
        private void SetTimer(int time)
        {
            int minutes = time / 60;
            int seconds = time - 60 * minutes;

            if (seconds > 9)
            {
                TimeLabel.Content = $"0{minutes}:{seconds}";
            }
            else
            {
                TimeLabel.Content = $"0{minutes}:0{seconds}";
            }
        }

        /// Dsplay message for user
        private void ShowEndMessage(string text)
        {
            Label wonLabel = new Label
            {
                Content = text,
                Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                FontSize = 30,
                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(2),
                Padding = new Thickness(20)
            };
            Canvas.SetLeft(wonLabel, 50);
            Canvas.SetTop(wonLabel, 250);
            Canvas1.Children.Add(wonLabel);
        }
    }
}