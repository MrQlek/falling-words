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
        private DateTime StartTime;
        private int NumberOfWroteChars;
        private DispatcherTimer TimerGenerateNewWord = new DispatcherTimer();
        private int CharSpeed = 60;
        private int EndCharSpeed = 700;
        private int WordsLength = 5;
        private List<string> WordList = new List<string>();


        public MainWindow(int startSpeed, int endSpeed, int wordsLength, int gameTime)
        {
            InitializeComponent();

            CharSpeed = startSpeed;
            EndCharSpeed = endSpeed;
            WordsLength = wordsLength;

            StartTime =  DateTime.Now;
            NumberOfWroteChars = 0;
            RefreshWordSpeed(CharSpeed);

            DispatcherTimer timerAnimation = new DispatcherTimer();
            timerAnimation.Interval = TimeSpan.FromMilliseconds(1);
            timerAnimation.Tick += SetWordsNewPosition;
            timerAnimation.Start();

            TimerGenerateNewWord.Interval = TimeSpan.FromMilliseconds(60000 / (CharSpeed / WordsLength));
            TimerGenerateNewWord.Tick += GenerateNewWord;
            TimerGenerateNewWord.Start();

            DispatcherTimer timerChangeWordsSpeed = new DispatcherTimer();
            timerChangeWordsSpeed.Interval = TimeSpan.FromSeconds(5);
            timerChangeWordsSpeed.Tick += SetNewWordsSpeed;
            timerChangeWordsSpeed.Start();
        }

        void SetWordsNewPosition(object sender, EventArgs e)
        {
            foreach (var wordLabel in Canvas1.Children.OfType<System.Windows.Controls.Label>())
            {
                Canvas.SetTop(wordLabel, Canvas.GetTop(wordLabel) + 2 + (CharSpeed - 60)/60/4);
                if (Canvas.GetTop(wordLabel) == 600)
                {
                    //TimerGenerateNewWord.Stop();
                    //MessageBoxResult result = MessageBox.Show("Would you like to greet the world with a \"Hello, world\"?", "My App", MessageBoxButton.YesNo);
                }
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
            Result.Content = "Your average speed: \n" + Math.Floor(NumberOfWroteChars/timeFromStart) + " chars per minute";
        }

        private void GenerateNewWord(object sender, EventArgs e)
        {
            Random rnd = new Random();
            string newWord = Words.GetWord(WordsLength);
            Label wordLabel = new Label
            {
                Content = newWord,
                Foreground = new SolidColorBrush(Color.FromRgb(0,0,0)),
                FontSize = 28,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(2)
            };
            WordList.Add(newWord);
            Canvas.SetLeft(wordLabel, rnd.Next(675 - (25*WordsLength)));
            Canvas.SetTop(wordLabel, 0);
            Canvas1.Children.Add(wordLabel);
            UserInput.Focus();
        }

        private void SetNewWordsSpeed(object sender, EventArgs e)
        {
            CharSpeed += 5;
            if(CharSpeed == EndCharSpeed)
            {
                //MessageBoxResult result = MessageBox.Show("You won!!!", "My App", MessageBoxButton.YesNo);
            }
            TimerGenerateNewWord.Interval = TimeSpan.FromMilliseconds(60000 / (CharSpeed / WordsLength));
            RefreshWordSpeed(CharSpeed);
        }

        private void RefreshWordSpeed(int CharSpeed)
        {
            WordsSpeed.Content = "Words Speed: \n" + CharSpeed + " chars per minute";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.Show();
            this.Close();
        }
    }
}