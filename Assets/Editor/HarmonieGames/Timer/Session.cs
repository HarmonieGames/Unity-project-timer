using System;

namespace Editor.HarmonieGames.Timer
{
    [Serializable]
    public struct Session
    {
        public string date;
        public int hours;
        public int minutes;
        public int seconds;

        public Session(TimeSpan timeSpan, DateTime dateTime)
        {
            date = dateTime.ToString("dd/MM/yyyy");
            hours = timeSpan.Hours;
            minutes = timeSpan.Minutes;
            seconds = timeSpan.Seconds;
        }

        public TimeSpan ToTimeSpan()
        {
            return new TimeSpan(hours, minutes, seconds);
        }
    }
}