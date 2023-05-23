using UnityEditor;
using UnityEngine;

namespace Editor.HarmonieGames.Timer
{
    public class StatisticsEditorWindow : EditorWindow
    {
        [MenuItem("Tools/Harmonie Games/Project Timer/Open Statistics Window")]
        private static void ShowWindow()
        {
            var window = GetWindow<StatisticsEditorWindow>();
            window.titleContent = new GUIContent("Statistics");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Statistics");
            
            //Overall
            GUILayout.Label("Overall");
            
            foreach (var stats in Statistics.GetStatistics(Statistics.GetOverallSessions()))
            {
                GUILayout.Label(stats);
            }
            
            //Current week
            GUILayout.Label("Current week");
            
            foreach (var stats in Statistics.GetStatistics(Statistics.GetCurrentWeekSessions()))
            {
                GUILayout.Label(stats);
            }
            
            //Previous week
            GUILayout.Label("Previous week");
            foreach (var stats in Statistics.GetStatistics(Statistics.GetPreviousWeekSessions()))
            {
                GUILayout.Label(stats);
            }
            
        }
    }
}