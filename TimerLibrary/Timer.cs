using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace TimerLibrary
{
    public class Timer
    {
        public Time Duration { private get; set; }
        public delegate void TimerEventHandler(Time e);
        public TimerEventHandler _TimerEventHandler;
        
        public event TimerEventHandler TimerCounted
        {
            add => _TimerEventHandler += value;
            remove
            {
                if (_TimerEventHandler != null) _TimerEventHandler -= value;
            }
        }

        private Time ActiveDurationLeft { get; set; }
        private Time TimeOfStart { get; set; }
        private int TimeLeftSeconds { get; set; }
        private bool IsRunning { get; set; }

        public void Reset()
        {
            Stop();
            ActiveDurationLeft = Duration;
            TimeLeftSeconds = GetTimeInSeconds(Duration);
            _TimerEventHandler(GetTimeAsObject(TimeLeftSeconds));
        }

        public void Flip()
        {
            if (IsRunning) Stop();
            else Start();
        }

        private async void Count()
        {
            TimeOfStart = GetTime();
            while (IsRunning)
            {
                TimeLeftSeconds = GetTimeInSeconds(ActiveDurationLeft) - (GetTimeInSeconds(GetTime()) - GetTimeInSeconds(TimeOfStart));
                _TimerEventHandler(GetTimeAsObject(TimeLeftSeconds));
                CheckState();
                await Task.Delay(100);
            }
        }

        private Time GetTime()
        {
            return new Time
            {
                Hour = DateTime.Now.Hour,
                Min = DateTime.Now.Minute,
                Sec = DateTime.Now.Second
            };
        }

        private void CheckState()
        {
            if (TimeLeftSeconds <= 0) Stop();
        }

        private void Start()
        {
            if (TimeLeftSeconds <= 0)
            {
                TimeLeftSeconds = GetTimeInSeconds(Duration);
                ActiveDurationLeft = Duration;

            }
            IsRunning = true;
            Count();
        }

        private void Stop()
        {
            IsRunning = false;
            ActiveDurationLeft = GetTimeAsObject(TimeLeftSeconds);
        }

        private Time GetTimeAsObject(int time)
        {
            var hour = Math.Max(0, (time / 60) / 60);
            var min = time/60 - 60 * hour;
            var sec = time - 60 * min;
                
            return new Time
            {
                Hour = hour,
                Min = min,
                Sec = sec
                
            };
        }

        private int GetTimeInSeconds(Time time)
        {
            return time.Sec + (time.Min * 60) + ((time.Hour * 60) * 60);
        }
    }
}