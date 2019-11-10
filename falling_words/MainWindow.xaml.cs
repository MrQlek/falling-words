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
        public MainWindow()
        {
            InitializeComponent();
            Text(0, 0, "sth2", Color.FromRgb(0, 0, 0));
            Text(30, 30, "sth", Color.FromRgb(0, 0, 0));


            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += setNewWordsPosition;
            timer.Start();
        }

        
        private void Text(double x, double y, string text, Color color)
        {
            Label word = new Label();
            word.Content = text;
            word.Foreground = new SolidColorBrush(color);
            word.BorderBrush = Brushes.Black;
            word.BorderThickness = new Thickness(2);
            Canvas.SetLeft(word, x);
            Canvas.SetTop(word, y);
            Canvas1.Children.Add(word);
        }
        void setNewWordsPosition(object sender, EventArgs e)
        {
            foreach(var textBox in Canvas1.Children)
            {
                try
                {
                    Label word = (Label)textBox;
                    Canvas.SetTop(word, Canvas.GetTop(word) + 1);
                    if (Canvas.GetTop(word) == 650)
                    {
                        MessageBoxResult result = MessageBox.Show("Would you like to greet the world with a \"Hello, world\"?", "My App", MessageBoxButton.YesNo);
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
                    Label word = (Label)textBox;
                    if (UserInput.Text == (string)word.Content)
                    {
                        wordsToRemove.Add(word);
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
    }
}
