namespace TimerLibrary
{
    public class Timer
    {
        public int WorkTime { get; set; }
        public int PauseTime { get; set; }
        
        public int TotalTime { get; set; }
        public int TimeLeft { get; set; }
        private bool _isWorkMode;
        
        public bool IsWorkMode
        {
            get { return _isWorkMode;}
            set
            {
                _isWorkMode = value;
                TotalTime = _isWorkMode == true ? WorkTime : PauseTime;
                TimeLeft = TotalTime;
            }
        }

        public decimal GetPercentage()
        {
            return TimeLeft / (decimal)TotalTime * 100;
        }

        public void Reset()
        {
            _isWorkMode = true;
            ResetTime();
        }

        private void ResetTime()
        {
            TotalTime = WorkTime;
            TimeLeft = TotalTime;
        }
    }
}