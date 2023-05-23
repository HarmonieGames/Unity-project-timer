using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Editor.HarmonieGames.Timer
{
    public static class Statistics
    {
        public static string[] GetStatistics(Session[] sessions)
        {
            var isOverall = sessions.Length == SaveTime.LoadSessions().Count;
            
            var statistics = new string[4];
            
            statistics[0] = $"~{GetHoursPerDay(sessions):0.00} hours/day";
            if (GetWeekEndHours(sessions) == 0)
                statistics[1] = "No hours during weekend, congrats!";
            else
                statistics[1] = $"~{GetWeekEndHours(sessions):0.00} hours during weekend";

            var hoursPerPeriod = new Dictionary<string, float>();
            var hoursPerPeriodString = "";

            hoursPerPeriod = isOverall ? GetHoursPerMonth(sessions) : GetHoursPerDayOfWeek(sessions);

            foreach (var period in hoursPerPeriod)
            {
                hoursPerPeriodString += period.Key + ": " + period.Value.ToString("0.00") + "\n";
            }
            
            statistics[2] = hoursPerPeriodString;

            return statistics;
        }

        //
        //Get average hours per day
        //
        private static float GetHoursPerDay(Session[] sessions)
        {
            var totalHours = sessions.Sum(session => (float) session.ToTimeSpan().TotalHours);
            return totalHours / sessions.Count();
        }
        
        //
        //Get all sessions where the day is a week end day
        //
        private static float GetWeekEndHours(IEnumerable<Session> sessions)
        {
            return (from session in sessions let date = DateTime.ParseExact(session.date, "dd/MM/yyyy", null) where date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday select (float) session.ToTimeSpan().TotalHours).Sum();
        }
        
        //
        //total hours per month + year
        //
        private static Dictionary<string,float> GetHoursPerMonth(IEnumerable<Session> sessions)
        {
            var hoursPerMonth = new Dictionary<string, float>();
            
            foreach (var session in sessions)
            {
                var date = DateTime.ParseExact(session.date, "dd/MM/yyyy", null);
                var key = date.ToString("MMMM") + " " + date.Year;
                
                if (hoursPerMonth.ContainsKey(key))
                    hoursPerMonth[key] += (float) session.ToTimeSpan().TotalHours;
                else
                    hoursPerMonth.Add(key, (float) session.ToTimeSpan().TotalHours);
            }

            return hoursPerMonth;
        }
        
        //
        //Get total hours per day of week
        //
        private static Dictionary<string,float> GetHoursPerDayOfWeek(IEnumerable<Session> sessions)
        {
            var hoursPerDayOfWeek = new Dictionary<string, float>();
            
            foreach (var session in sessions)
            {
                var date = DateTime.ParseExact(session.date, "dd/MM/yyyy", null);
                var key = date.DayOfWeek.ToString();
                
                if (hoursPerDayOfWeek.ContainsKey(key))
                    hoursPerDayOfWeek[key] += (float) session.ToTimeSpan().TotalHours;
                else
                    hoursPerDayOfWeek.Add(key, (float) session.ToTimeSpan().TotalHours);
            }

            return hoursPerDayOfWeek;
        }
        
        public static Session[] GetOverallSessions()
        {
            return ProjectTimerEditorWindow.ProjectTimer.GetSessions();
        }

        public static Session[] GetCurrentWeekSessions()
        {
            var sessions = SaveTime.LoadSessions();

            return (from session in sessions let date = DateTime.ParseExact(session.date, "dd/MM/yyyy", null) let currentWeek = DateTime.Now.Date.AddDays(-(int) DateTime.Now.DayOfWeek) where date >= currentWeek select session).ToArray();
        }
        
        public static Session[] GetPreviousWeekSessions()
        {
            var sessions = SaveTime.LoadSessions();

            return (from session in sessions let date = DateTime.ParseExact(session.date, "dd/MM/yyyy", null) let currentWeek = DateTime.Now.Date.AddDays(-(int) DateTime.Now.DayOfWeek) let lastWeek = currentWeek.AddDays(-7) where date >= lastWeek && date < currentWeek select session).ToArray();
        }
    }
}