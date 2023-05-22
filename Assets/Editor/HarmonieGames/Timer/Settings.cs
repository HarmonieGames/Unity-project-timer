#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor.HarmonieGames.Timer
{
    [InitializeOnLoad]
    public class Settings
    {
        private const string timerLaunchedByDefaultKey = "customSettings.timerLaunchedByDefault";
        private const string dateFormatKey = "customSettings.dateFormat";
        private const string delimiterKey = "customSettings.delimiter";

        public class TimerSettings
        {
            public bool TimerLaunchedByDefault;
            public DateFormat DateFormat;
            public Delimiter Delimiter;
        }

        public static TimerSettings GetEditorSettings()
        {
            return new TimerSettings
            {
                TimerLaunchedByDefault = EditorPrefs.GetBool(timerLaunchedByDefaultKey, true),
                DateFormat = EditorPrefs.GetInt(dateFormatKey) == 0 ? DateFormat.ddmmyyyy : (DateFormat)EditorPrefs.GetInt(dateFormatKey),
                Delimiter = EditorPrefs.GetInt(delimiterKey) == 0 ? Delimiter.Comma : (Delimiter)EditorPrefs.GetInt(delimiterKey)
            };
        }

        public static void SetEditorSettings(TimerSettings settings)
        {
            EditorPrefs.SetBool(timerLaunchedByDefaultKey, settings.TimerLaunchedByDefault);
            EditorPrefs.SetInt(dateFormatKey, (int)settings.DateFormat);
            EditorPrefs.SetInt(delimiterKey, (int)settings.Delimiter);
        }
    }

    internal class SettingsGUIContent
    {
        private static GUIContent enableTimerLaunchedByDefault = new GUIContent("Timer launched by default", "Tooltip for custom setting 1 goes here");
        private static GUIContent enableDateFormat = new GUIContent("Export Date Format", "Tooltip for date format goes here");
        private static GUIContent enableDelimiter = new GUIContent("Export Delimiter", "Tooltip for delimiter goes here");

        public static void DrawSettingsButtons(Settings.TimerSettings settings)
        {
            EditorGUI.indentLevel += 1;

            settings.TimerLaunchedByDefault = EditorGUILayout.ToggleLeft(enableTimerLaunchedByDefault, settings.TimerLaunchedByDefault);
            settings.DateFormat = (DateFormat) EditorGUILayout.EnumPopup(enableDateFormat, settings.DateFormat);
            settings.Delimiter = (Delimiter) EditorGUILayout.EnumPopup(enableDelimiter, settings.Delimiter);

            EditorGUI.indentLevel -= 1;
        }
    }

#if UNITY_2018_3_OR_NEWER
    static class SettingsProvider
    {
        [SettingsProvider]
        public static UnityEditor.SettingsProvider CreateSettingsProvider()
        {
            var provider = new UnityEditor.SettingsProvider("Preferences/Project Timer Settings", SettingsScope.User)
            {
                label = "Project Timer Settings",

                guiHandler = (searchContext) =>
                {
                    Settings.TimerSettings settings = Settings.GetEditorSettings();

                    EditorGUI.BeginChangeCheck();
                    SettingsGUIContent.DrawSettingsButtons(settings);

                    if (EditorGUI.EndChangeCheck())
                    {
                        Settings.SetEditorSettings(settings);
                    }
                },

                //Keywords for the search bar in the Unity Preferences menu
                keywords = new HashSet<string>(new[] { "Project", "Timer", "Settings, Date" })
            };

            return provider;
        }
    }
#endif

}

#endif