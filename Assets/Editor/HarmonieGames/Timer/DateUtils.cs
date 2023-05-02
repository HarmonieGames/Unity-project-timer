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
                DateFormat.ddMMyyyy => $"{date.Day:00}/{date.Month:00}/{date.Year}",
                DateFormat.MMddyyyy => $"{date.Month:00}/{date.Day:00}/{date.Year}",
                DateFormat.yyyyMMdd => $"{date.Year}/{date.Month:00}/{date.Day:00}",
                _ => $"{date.Day:00}/{date.Month:00}/{date.Year}"
            };
        }
        
        public static DateTime ToDateTime(string dateString, DateFormat dateFormat)
        {
            return dateFormat switch
            {
                DateFormat.ddMMyyyy => DateTime.ParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                DateFormat.MMddyyyy => DateTime.ParseExact(dateString, "MM/dd/yyyy", CultureInfo.InvariantCulture),
                DateFormat.yyyyMMdd => DateTime.ParseExact(dateString, "yyyy/MM/dd", CultureInfo.InvariantCulture),
                _ => DateTime.ParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture)
            };
        }
    }

    public enum DateFormat
    {
        ddMMyyyy,
        MMddyyyy,
        yyyyMMdd
    }
}