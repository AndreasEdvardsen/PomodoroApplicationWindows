using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace TimerLibrary
{
    public class Timer
    {
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
        
        public Time WorkDuration { get; set; }
        public Time PauseDuration { get; set; }
        private Time Duration { get; set; }
        public int TimeLeftSeconds { get; set; }
        public int TotalTimeSeconds { get; set; }
        
        private Time ActiveDurationLeft { get; set; }
        private Time TimeOfStart { get; set; }
        private bool IsRunning { get; set; }

        public Timer()
        {
            
        }

        private Time SwitchState(Time current)
        {
            return current.State == 0 ? PauseDuration : WorkDuration;
        }
        
        public void Reset()
        {
            Stop();
            ActiveDurationLeft = Duration;
            TimeLeftSeconds = ConvertToSeconds(Duration);
            _TimerEventHandler(ConvertToTimeObject(TimeLeftSeconds));
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
                TimeLeftSeconds = ConvertToSeconds(ActiveDurationLeft) - (ConvertToSeconds(GetTime()) - ConvertToSeconds(TimeOfStart));
                _TimerEventHandler(ConvertToTimeObject(TimeLeftSeconds));
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
                if (Duration == null) Duration = WorkDuration;
                TotalTimeSeconds = ConvertToSeconds(Duration);
                TimeLeftSeconds = TotalTimeSeconds;
                ActiveDurationLeft = Duration;
            }
            IsRunning = true;
            Count();
        }

        private void Stop()
        {
            IsRunning = false;
            ActiveDurationLeft = ConvertToTimeObject(TimeLeftSeconds);
        }

        private Time ConvertToTimeObject(int time)
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

        private int ConvertToSeconds(Time time)
        {
            return time.Sec + (time.Min * 60) + ((time.Hour * 60) * 60);
        }
    }
}