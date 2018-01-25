using System;
using System.Threading.Tasks;

namespace TimerLibrary
{
    public class TimerLogic
    {
        public delegate void TimerEventHandler(Time e);

        public TimerLogic(int workDuration, int pauseDuration)
        {
            Timer = new Timer
            {
                WorkTime = workDuration,
                PauseTime = pauseDuration,
                IsWorkMode = true
            };
        }

        public Timer Timer;
        private bool IsRunning { get; set; }
        
        public event TimerEventHandler TimerCounted;
        public event TimerEventHandler CountdownCompleted;

        public void Reset()
        {
            var ost = IsRunning;
            if (ost)
            {
                
            }
            Timer.Reset();
            Stop();
            TimerCounted?.Invoke(BuildTimeReport());
        }

        public void Flip()
        {
            if (IsRunning) Stop();
            else Start();
        }

        private async void Count()
        {
            var timeOfStart = GetTime();
            while (IsRunning)
            {
                //TODO Refactor Time left seconds thingy
                var timeSpan = (GetTime() - timeOfStart).TotalSeconds;
                Timer.TimeLeft = Timer.TotalTime - (int)timeSpan;
                
                TimerCounted?.Invoke(BuildTimeReport());
                
                CheckState();
                await Task.Delay(100);
            }
        }

        private DateTime GetTime()
        {
            return DateTime.Now;
        }

        private void CheckState()
        {
            if (Timer.TimeLeft > 0) return;
            CountdownCompleted?.Invoke(BuildTimeReport());
            Stop();
        }

        private void Start()
        {
            IsRunning = true;
            Count();
        }

        private void Stop()
        {
            IsRunning = false;
            Timer.TotalTime = Timer.TimeLeft;
        }

        private Time BuildTimeReport()
        {
            var timeLeft = Timer.TimeLeft;
            var hour = Math.Max(0, timeLeft / 60 / 60);
            var min = timeLeft / 60 - 60 * hour;
            var sec = timeLeft - 60 * min;

            return new Time
            {
                Hour = hour,
                Min = min,
                Sec = sec
            };
        }
    }
}