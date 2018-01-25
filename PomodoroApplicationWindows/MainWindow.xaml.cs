using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TimerLibrary;

namespace PomodoroApplicationWindows
{
    public partial class MainWindow
    {
        private readonly TimerLogic _timerLogic;

        public MainWindow()
        {
            InitializeComponent();
            _timerLogic = new TimerLogic(1200, 300);
            _timerLogic.TimerCounted += TimerLogicOnCounted;
            _timerLogic.CountdownCompleted += TimerLogicCompleted;
        }

        private void TimerLogicCompleted(Time e)
        {
            var stringToShow = e.IsWorkMode == false ? "Time for a break!" : "Time for work!";
            MessageBox.Show(stringToShow, stringToShow, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void ColorChanger()
        {
            var currentColor = (SolidColorBrush) TimerEllipse.Fill;
            var colorAnimation = new ColorAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(5)),
                From = currentColor.Color
            };

            var percentage = _timerLogic.Timer.GetPercentage();
            
            if (percentage <= 0) colorAnimation.To = ColorFromString("#405d27");
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

        private void TimerLogicOnCounted(Time e)
        {
            var kake = e.Min + "m " + e.Sec + "s";
            CurrentTimeText.Text = kake;
            ColorChanger();
        }

        private void UIElement_OnMouseUpFlip(object sender, MouseButtonEventArgs e)
        {
            _timerLogic.Flip();
        }

        private void ButtonBase_OnClickReset(object sender, RoutedEventArgs e)
        {
            _timerLogic.Reset();
        }

        private void Polygon_Clicked(object sender, MouseButtonEventArgs e)
        {
            ToggleDropdownMenu();
        }

        private void ChangeCurrentTime(object sender, MouseButtonEventArgs e)
        {
            var button = (TextBlock) sender;
            var timeTochangeTo = Convert.ToInt32(button.Tag);

            _timerLogic.Timer.WorkTime = timeTochangeTo * 60;
            _timerLogic.Reset();
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