using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Editor.HarmonieGames.Timer
{
    public static class DateUtils
    {
        public static string FormatDateString(DateTime date, DateFormat dateFormat)
        {
            return dateFormat switch
            {
                DateFormat.ddmmyyyy => $"{date.Day:00}/{date.Month:00}/{date.Year}",
                DateFormat.mmddyyyy => $"{date.Month:00}/{date.Day:00}/{date.Year}",
                _ => $"{date.Day:00}/{date.Month:00}/{date.Year}"
            };
        }
        
        public static DateTime ToDateTime(string dateString, DateFormat dateFormat)
        {
            return dateFormat switch
            {
                DateFormat.ddmmyyyy => DateTime.ParseExact(dateString, "dd/MM/yyyy", new CultureInfo("fr-Fr")),
                DateFormat.mmddyyyy => DateTime.ParseExact(dateString, "MM/dd/yyyy", new CultureInfo("fr-Fr")),
                _ => DateTime.ParseExact(dateString, "dd/MM/yyyy", new CultureInfo("fr-Fr"))
            };
        }

        public static string GetDateFormatString(DateFormat dateFormat)
        {
            return dateFormat switch
            {
                DateFormat.ddmmyyyy => "dd/MM/yyyy",
                DateFormat.mmddyyyy => "MM/dd/yyyy",
                _ => "dd/MM/yyyy"
            };
        }
    }

    public enum DateFormat
    {
        ddmmyyyy,
        mmddyyyy,
    }
}