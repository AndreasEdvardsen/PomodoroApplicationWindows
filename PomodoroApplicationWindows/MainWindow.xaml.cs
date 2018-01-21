
using System.Windows;
using System.Windows.Input;
using TimerLibrary;

namespace PomodoroApplicationWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Timer Timer;
        public MainWindow()
        {
            InitializeComponent();
            Timer = new Timer
            {
                Duration = new Time
                {
                    Hour = 0,
                    Min = 20,
                    Sec = 0
                }
            };
            Timer.TimerCounted += TimerOnCounted;
        }

        private void TimerOnCounted(Time e)
        {
            CurrentTimeText.Text = e.Min + "m " + e.Sec + "s";
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