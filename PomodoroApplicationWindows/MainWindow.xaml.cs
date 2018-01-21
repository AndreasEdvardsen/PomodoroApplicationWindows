
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TimerLibrary;

namespace PomodoroApplicationWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Timer Timer;
        private Time _workDuration;
        private Time _pauseDuration;
        private Color currentColor;
        
        public MainWindow()
        {
            InitializeComponent();
            
            Timer = new Timer
            {
                WorkDuration = new Time
                {
                    Hour = 0,
                    Min = 1,
                    Sec = 0,
                    State = 0
                },
                
                PauseDuration = new Time
                {
                    Hour = 0,
                    Min = 5,
                    Sec = 0,
                    State = 1
                }
            };
            Timer.TimerCounted += TimerOnCounted;
        }

        private void ColorChanger(int current, int original)
        {
            var percentage = ((decimal)current / (decimal)original) * 100;
            var timerEllipse = TimerEllipse;
            if (percentage <= 0)
            {
                var colorAnimation = new ColorAnimation();
                colorAnimation.From = currentColor;
                colorAnimation.To = (Color)ColorConverter.ConvertFromString("#405d27");
                colorAnimation.Duration = new Duration().TimeSpan.Add(TimeSpan.FromSeconds(1));
                
                timerEllipse.Fill = ColorFromString("#405d27");
            } 
            else if (percentage < 30)
            {
                timerEllipse.Fill = ColorFromString("#82b74b");
            }
            else if (percentage < 60)
            {
                timerEllipse.Fill = ColorFromString("#eea29a");
            } 
            else if (percentage < 100)
            {
                timerEllipse.Fill = ColorFromString("#c94c4c");
            }
            else if (percentage == 100)
            {
                timerEllipse.Fill = ColorFromString("#3e4444");
            }
            
        }

        private SolidColorBrush ColorFromString(string color)
        {
            return new SolidColorBrush {Color = (Color) ColorConverter.ConvertFromString(color)};
        }

        private void TimerOnCounted(Time e)
        {
            CurrentTimeText.Text = e.Min + "m " + e.Sec + "s";
            ColorChanger(Timer.TimeLeftSeconds, Timer.TotalTimeSeconds);
        }

        private void UIElement_OnMouseUpFlip(object sender, MouseButtonEventArgs e)
        {
            Timer.Flip();
        }

        private void ButtonBase_OnClickReset(object sender, RoutedEventArgs e)
        {
            Timer.Reset();
        }
    }
}