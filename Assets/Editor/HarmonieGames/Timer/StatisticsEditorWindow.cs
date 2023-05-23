using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor.HarmonieGames.Timer
{
    public class StatisticsEditorWindow : EditorWindow
    {
        private GUISkin _skin;
        private string _folderPath;
        
        [MenuItem("Tools/Harmonie Games/Project Timer/Open Statistics Window")]
        private static void ShowWindow()
        {
            var window = GetWindow<StatisticsEditorWindow>();
            window.titleContent = new GUIContent("Project Timer Statistics");
            window.Show();
        }

        private void OnEnable()
        {
            _folderPath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)).Replace("/StatisticsEditorWindow.cs", "");
            _skin = AssetDatabase.LoadAssetAtPath<GUISkin>($"{_folderPath}/Stylesheet.guiskin");
        }

        private void OnGUI()
        {
            GUILayout.Label("Project Timer Statistics", _skin.GetStyle("H1"));
            
            DisplaySection("Overall", Statistics.GetStatistics(Statistics.GetOverallSessions()));
            DisplaySection("Current week", Statistics.GetStatistics(Statistics.GetCurrentWeekSessions()));
            DisplaySection("Previous week", Statistics.GetStatistics(Statistics.GetPreviousWeekSessions()));
        }

        private void DisplaySection(string sectionTitle, IReadOnlyList<string> stats)
        {
            GUILayout.Space(25); 
            GUILayout.BeginHorizontal();
            GUILayout.Label(sectionTitle, _skin.GetStyle("H2"));
            GUILayout.Space(25); 
            
            GUILayout.BeginVertical();
            GUILayout.Label(stats[0], _skin.GetStyle("h/d")); //hours/day
            GUILayout.Label(stats[1], _skin.GetStyle("Subtitle")); //hours during weekend
            GUILayout.EndVertical();
            
            GUILayout.EndHorizontal();
            GUILayout.Space(12);
            
            GUILayout.Label(stats[2],_skin.GetStyle("List label"));   //GetHoursPerDayOfWeek or HoursPerMonth
            
            GUILayout.Label(stats[3],_skin.GetStyle("List label"));   //Total hours
        }
    }
}