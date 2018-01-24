using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using PomodoroLibrary;

namespace PomodoroApplicationWindows
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Timer _timer;

        public MainWindow()
        {
            InitializeComponent();
            _timer = new Timer(new Time
                {
                    Hour = 0,
                    Min = 20,
                    Sec = 0,
                    State = 0
                },
                new Time
                {
                    Hour = 0,
                    Min = 5,
                    Sec = 0,
                    State = 1
                });
            _timer.TimerCounted += TimerOnCounted;
            _timer.CountdownCompleted += TimerCompleted;
        }

        private void TimerCompleted(Time e)
        {
            var stringToShow = e.State == 0 ? "Time for a break!" : "Time for work!";
            MessageBox.Show(stringToShow, stringToShow, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void ColorChanger(int current, int original)
        {
            var currentColor = (SolidColorBrush) TimerEllipse.Fill;
            var colorAnimation = new ColorAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(5)),
                From = currentColor.Color
            };

            var percentage = current / (decimal) original * 100;
            if (percentage <= 0)
                colorAnimation.To = ColorFromString("#405d27");
            else if (percentage < 30)
                colorAnimation.To = ColorFromString("#82b74b");
            else if (percentage < 60)
                colorAnimation.To = ColorFromString("#eea29a");
            else if (percentage < 100)
                colorAnimation.To = ColorFromString("#c94c4c");
            else if (percentage == 100)
                colorAnimation.To = ColorFromString("#3e4444");
            TimerEllipse.Fill.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
        }

        private Color ColorFromString(string color)
        {
            return (Color) ColorConverter.ConvertFromString(color);
        }

        private void TimerOnCounted(Time e)
        {
            var kake = e.Min + "m " + e.Sec + "s";
            CurrentTimeText.Text = kake;
            ColorChanger(_timer.TimeLeftSeconds, _timer.TotalTimeSeconds);
        }

        private void UIElement_OnMouseUpFlip(object sender, MouseButtonEventArgs e)
        {
            _timer.Flip();
        }

        private void ButtonBase_OnClickReset(object sender, RoutedEventArgs e)
        {
            _timer.Reset();
        }

        private void Polygon_Clicked(object sender, MouseButtonEventArgs e)
        {
            ToggleDropdownMenu();
        }

        private void ChangeCurrentTime(object sender, MouseButtonEventArgs e)
        {
            var button = (TextBlock) sender;
            var timeTochangeTo = Convert.ToInt32(button.Tag);

            _timer.WorkDuration = new Time
            {
                Hour = 0,
                Min = timeTochangeTo,
                Sec = 0,
                State = 0
            };
            _timer.Reset();
        }

        private void ToggleDropdownMenu()
        {
            DropDownMenu.Visibility = DropDownMenu.Visibility == Visibility.Collapsed
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void TextBlockBackgroungChanger(object o, MouseEventArgs mouseEventArgs)
        {
            var textBlock = (TextBlock) o;
            textBlock.Background.Opacity = textBlock.Background.Opacity == 0 ? 0.5 : 0;
        }

        private void Menu_Leave(object sender, MouseEventArgs e)
        {
            ToggleDropdownMenu();
        }
    }
}