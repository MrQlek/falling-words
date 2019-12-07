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
        private int WordsSpeed = 1000;
        private int WordsLength = 3;


        public MainWindow()
        {
            InitializeComponent();

            StartTime =  DateTime.Now;
            NumberOfWroteChars = 0;

            DispatcherTimer timerAnimation = new DispatcherTimer();
            timerAnimation.Interval = TimeSpan.FromMilliseconds(1);
            timerAnimation.Tick += SetNewWordsPosition;
            timerAnimation.Start();

            TimerGenerateNewWord.Interval = TimeSpan.FromMilliseconds(60000 / (WordsSpeed / WordsLength));
            TimerGenerateNewWord.Tick += GenerateNewWord;
            TimerGenerateNewWord.Start();

            DispatcherTimer timerChangeWordsSpeed = new DispatcherTimer();
            timerChangeWordsSpeed.Interval = TimeSpan.FromSeconds(5);
            timerChangeWordsSpeed.Tick += SetNewWordsSpeed;
            timerChangeWordsSpeed.Start();
        }

        void SetNewWordsPosition(object sender, EventArgs e)
        {
            foreach(var textBox in Canvas1.Children)
            {
                try
                {
                    Label wordLabel = (Label)textBox;
                    Canvas.SetTop(wordLabel, Canvas.GetTop(wordLabel) + 2 + (WordsSpeed - 60)/60/4);
                    if (Canvas.GetTop(wordLabel) == 600)
                    {
                        //TimerGenerateNewWord.Stop();
                        //MessageBoxResult result = MessageBox.Show("Would you like to greet the world with a \"Hello, world\"?", "My App", MessageBoxButton.YesNo);
                    }
                }
                catch
                {
                    ;
                }
            }
        }

        private void UserInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Label> wordsToRemove = new List<Label>();
            foreach (var textBox in Canvas1.Children)
            {
                try
                {
                    Label wordLabel = (Label)textBox;
                    if (UserInput.Text == (string)wordLabel.Content)
                    {
                        wordsToRemove.Add(wordLabel);
                        RefreshResult(UserInput.Text.Length);
                        UserInput.Text = "";
                    }
                }
                catch
                {
                    ;
                }
            }
            foreach (Label wordToRemove in wordsToRemove)
            {
                Canvas1.Children.Remove(wordToRemove);
            }
        }

        private void RefreshResult(int wordLength)
        {
            NumberOfWroteChars += wordLength;
            double timeFromStart = (DateTime.Now - StartTime).TotalMinutes;
            Result.Content = NumberOfWroteChars/timeFromStart;
        }

        private void GenerateNewWord(object sender, EventArgs e)
        {
            Random rnd = new Random();
            Label wordLabel = new Label
            {
                Content = Words.GetWord(WordsLength),
                Foreground = new SolidColorBrush(Color.FromRgb(0,0,0)),
                FontSize = 28,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(2)
            };
            Canvas.SetLeft(wordLabel, rnd.Next(675 - (25*WordsLength)));
            Canvas.SetTop(wordLabel, 0);
            Canvas1.Children.Add(wordLabel);
        }

        private void SetNewWordsSpeed(object sender, EventArgs e)
        {
            WordsSpeed += 5;
            if( WordsSpeed == 700)
            {
                MessageBoxResult result = MessageBox.Show("You won!!!", "My App", MessageBoxButton.YesNo);
            }
            //TimerGenerateNewWord.Interval = TimeSpan.FromMilliseconds(60000 / (WordsSpeed / WordsLength));
        }
    }
}