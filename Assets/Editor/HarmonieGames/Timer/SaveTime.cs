using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Editor.HarmonieGames.Timer
{
    public static class SaveTime
    {
        private const string FilePath = "Assets/Editor/HarmonieGames/Timer/Time.json";
        
        private static List<Session> _sessions = new List<Session>();
        
        //Load all line from json file as List<Session>
        public static List<Session> LoadSessions()
        {
            _sessions = new List<Session>();

            //check if file exists
            if (!System.IO.File.Exists(FilePath))
            {
                //if not, create file
                var newSession = new Session(new TimeSpan(), DateTime.Now.Date);
                _sessions.Add(newSession);
                
                System.IO.File.WriteAllText(FilePath, JsonUtility.ToJson(newSession));
            }
            
            var json = System.IO.File.ReadAllText(FilePath);
            
            //Remove brackets
            json = json.Replace("[", "").Replace("]", "");
            
            var timeSpanData = json.Split('}');
           
            _sessions.AddRange(FromStringToSession(timeSpanData));

            return _sessions;
        }

        private static List<string> FromSessionToString()
        {
            return _sessions.Select(session => JsonUtility.ToJson(session)).ToList();
        }

        private static IEnumerable<Session> FromStringToSession(IEnumerable<string> strings)
        {
            var sessions = new List<Session>();
            
            foreach (var s in strings)
            {
                var cleanString = s + "}";
                
                //If first charactere is comma, remove it
                if (cleanString[0] == ',')
                {
                    cleanString = cleanString.Substring(1);
                }
                
                //if not empty
                if (s != "")
                    sessions.Add(JsonUtility.FromJson<Session>(cleanString));
            }

            return sessions;
        }

        //Save timeSpan to json file
        public static void SaveTimeSpan(TimeSpan timeSpan)
        {
            //Last Session in List
            var lastSession = _sessions[_sessions.Count - 1];

            if (lastSession.ToDateTime() == DateTime.Now.Date)
            {
                //Update last session
                _sessions[_sessions.Count - 1] = new Session(timeSpan, lastSession.ToDateTime());
            }
            else
            {
                //Add new session
                _sessions.Add(new Session(timeSpan, DateTime.Now.Date));
            }

            var json = "[";
            
            //Serialize timeSpanData to json
            foreach (var s in FromSessionToString())
            {
                json += s;
                
                //if not last element, add comma
                if (s != FromSessionToString()[FromSessionToString().Count - 1])
                {
                    json += ",";
                }
            }
            
            json += "]";
            
            //Write json to file
            System.IO.File.WriteAllText(FilePath, json);
        }
    }
    
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
        
        public DateTime ToDateTime()
        {
            return DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
    }

}