using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor.HarmonieGames.Timer
{
    public static class Export
    {
        [MenuItem("Tools/Harmonie Games/Project Timer/Export to csv")]
        public static void ExportTimerInCsv()
        {
            //Create string with headers
            var headers = new[] {"Date", "Time"};
        
            var dateFormat = (DateFormat)EditorPrefs.GetInt("customSettings.dateFormat");
            var format = DateUtils.GetDateFormatString(dateFormat);

            //Create string data for table
            var data = (from s in ProjectTimerEditorWindow.ProjectTimer.GetSessions() let nDate = DateTime.ParseExact(s.date, "dd/MM/yyyy", NumberFormatInfo.InvariantInfo) select new[] {nDate.ToString(format), s.ToTimeSpan().TotalHours.ToString(CultureInfo.InvariantCulture)}).ToList();
            
            ExportInCsv(headers,data);
        }

        private static void ExportInCsv(string[] headers, IEnumerable<string[]> data)
        {
            var csv = new List<string>();
            
            var delimiter = DelimiterToString((Delimiter)EditorPrefs.GetInt("customSettings.delimiter"));

            var header = string.Join(delimiter, headers);
            csv.Add(header);

            csv.AddRange(data.Select(d => string.Join(delimiter, d)));

            //Ask user where to save file
            var filePath = EditorUtility.SaveFilePanel("Save CSV", "", "Export.csv", "csv");

            if (filePath == "") return;

            File.WriteAllLines(filePath, csv.ToArray());
        }
        
        public static string DelimiterToString(Delimiter delimiter)
        {
            return delimiter switch
            {
                Delimiter.Comma => ",",
                Delimiter.Semicolon => ";",
                Delimiter.Tab => "\t",
                _ => ","
            };
        }
    }

    public enum Delimiter
    {
        Comma,
        Semicolon,
        Tab
    }
}
