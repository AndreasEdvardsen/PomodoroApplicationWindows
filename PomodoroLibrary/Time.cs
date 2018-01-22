namespace PomodoroLibrary
{
    public class Time
    {
        public int State { get; set; } //0 for work duration, 1 for pause duration.
        public int Sec { get; set; }
        public int Min { get; set; }
        public int Hour { get; set; }
    }
}