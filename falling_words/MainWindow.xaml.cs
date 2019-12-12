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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace falling_words
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DateTime StartTime;
        private int NumberOfWroteChars = 0;
        private DispatcherTimer TimerGenerateNewWord = new DispatcherTimer();
        private DispatcherTimer TimerAnimation = new DispatcherTimer();
        private DispatcherTimer TimerChangeWordsSpeed = new DispatcherTimer();
        private DispatcherTimer TimerCountTime = new DispatcherTimer();
        private int CharSpeed;
        private int WordsLength;
        private int AddToCharSpeed;
        private int Time;
        private int Counter = 1000000;
        private int TimeBetweenWords;
        private List<string> WordList = new List<string>();

        public MainWindow(int startSpeed, int endSpeed, int wordsLength, int gameTime)
        {
            InitializeComponent();

            CharSpeed = startSpeed;

            WordsLength = wordsLength;
            AddToCharSpeed = CountAddToCharSpeed(gameTime, startSpeed, endSpeed);
            Time = gameTime;
            StartTime =  DateTime.Now;
            TimeBetweenWords = 60000 / (CharSpeed / WordsLength);

            RefreshWordSpeed(CharSpeed);
            InitializeTimers();
        }

        private void InitializeTimers()
        {
            TimerAnimation.Interval = TimeSpan.FromMilliseconds(1);
            TimerAnimation.Tick += SetWordsNewPosition;
            TimerAnimation.Start();

            TimerGenerateNewWord.Interval = TimeSpan.FromMilliseconds(10);
            TimerGenerateNewWord.Tick += GenerateNewWord;
            TimerGenerateNewWord.Start();

            TimerChangeWordsSpeed.Interval = TimeSpan.FromSeconds(1);
            TimerChangeWordsSpeed.Tick += SetNewWordsSpeed;
            TimerChangeWordsSpeed.Start();

            TimerCountTime.Interval = TimeSpan.FromSeconds(1);
            TimerCountTime.Tick += CountTime;
            TimerCountTime.Start();
        }

        private void StopGame()
        {
            TimerAnimation.Stop();
            TimerGenerateNewWord.Stop();
            TimerChangeWordsSpeed.Stop();
            TimerCountTime.Stop();
        }

        private int CountAddToCharSpeed(int gameTime, int startSpeed, int stopSpeed)
        {
            gameTime -= 10;
            return (int)Math.Ceiling((stopSpeed - startSpeed) / (float)gameTime);
        }

        void SetWordsNewPosition(object sender, EventArgs e)
        {
            bool lost = false;
            foreach (var wordLabel in Canvas1.Children.OfType<System.Windows.Controls.Label>())
            {
                Canvas.SetTop(wordLabel, Canvas.GetTop(wordLabel) + 2 + (CharSpeed - 60)/60/4);
                if (Canvas.GetTop(wordLabel) == 600)
                {
                    lost = true;
                }
            }
            if(lost)
            {
                StopGame();
                ShowEndMessage("You lost. \nGo to menu and try again \nor pick easier level to train.");
            }
        }

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

        private void RemoveLabels(Canvas canvas, List<Label> labelsList)
        {
            foreach (Label wordToRemove in labelsList)
            {
                canvas.Children.Remove(wordToRemove);
            }
        }

        private void RefreshResult(int wordLength)
        {
            NumberOfWroteChars += wordLength;
            double timeFromStart = (DateTime.Now - StartTime).TotalMinutes;
            ResultLabel.Content = $"Your average speed: \n{Math.Floor(NumberOfWroteChars/timeFromStart)}chars per minute";
        }

        private void GenerateNewWord(object sender, EventArgs e)
        {
            Counter += 10;
            if (Counter >= TimeBetweenWords)
            {
                Counter = 0;
                Random rnd = new Random();
                string newWord = Words.GetWord(WordsLength);
                Label wordLabel = new Label
                {
                    Content = newWord,
                    Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                    FontSize = 28,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(2)
                };
                WordList.Add(newWord);
                Canvas.SetLeft(wordLabel, rnd.Next(675 - (25 * WordsLength)));
                Canvas.SetTop(wordLabel, 0);
                Canvas1.Children.Add(wordLabel);
                UserInput.Focus();
            }
        }

        private void SetNewWordsSpeed(object sender, EventArgs e)
        {
            CharSpeed += AddToCharSpeed;
            TimeBetweenWords =  60000 / (CharSpeed / WordsLength);
            RefreshWordSpeed(CharSpeed);
        }

        private void RefreshWordSpeed(int CharSpeed)
        {
            WordsSpeedLabel.Content = $"Words Speed: \n{CharSpeed} chars per minute";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.Show();
            this.Close();
        }

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
                ShowEndMessage("Congratulations! \nYou won this level \nGo to menu and choose next level");
            }
        }

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

        private void ShowEndMessage(string text)
        {
            TextBlock wonLabel = new TextBlock
            {
                Text = text,
                Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                FontSize = 30,
                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255))
            };
            Canvas.SetLeft(wonLabel, 150);
            Canvas.SetTop(wonLabel, 250);
            Canvas1.Children.Add(wonLabel);
        }
    }
}