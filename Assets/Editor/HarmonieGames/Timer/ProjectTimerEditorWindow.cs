using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor.HarmonieGames.Timer
{
    public class ProjectTimerEditorWindow : EditorWindow, IHasCustomMenu
    {
        private GUISkin _skin;
        private string _folderPath;
        public static Timer ProjectTimer;
        
        //Settings
        private bool _isTimerLaunchedByDefault;

        [MenuItem("Tools/Harmonie Games/Project Timer/Open Project Timer")]
        public static void OpenWindow()
        {
            var window = GetWindow<ProjectTimerEditorWindow>("Project Timer");
            window.minSize = new Vector2(250, 325);
            window.Show();
        }
        
        void IHasCustomMenu.AddItemsToMenu(GenericMenu menu)
        {
            var content = new GUIContent("Export Timer to CSV");
            menu.AddItem(content,false, Export.ExportTimerInCsv);
        }

        private void OnEnable()
        {
            _folderPath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)).Replace("/ProjectTimerEditorWindow.cs", "");
            _skin = AssetDatabase.LoadAssetAtPath<GUISkin>($"{_folderPath}/Stylesheet.guiskin");
        
            ProjectTimer = new Timer();
            ProjectTimer.LoadSessions(SaveTime.LoadSessions());
            
            //Settings
            _isTimerLaunchedByDefault = EditorPrefs.GetBool("customSettings.timerLaunchedByDefault");

            ProjectTimer.SetTimer(_isTimerLaunchedByDefault);
        
            Timer.OnTimerUpdate += UpdateUI;
        }

        private void OnDisable()
        {
            Timer.OnTimerUpdate -= UpdateUI;
        }
    
        private void OnGUI()
        {
            GUILayout.FlexibleSpace();
            
            //Current session time
            GUILayout.Label("Current session time", _skin.GetStyle("Surtitle"));
            GUILayout.Label(ProjectTimer.GetLastSessionTime(), _skin.GetStyle("CurrentTimerLabel"));

            //Button
            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(ProjectTimer.GetTimerStatus() ? $"{_folderPath}/Assets/pause-icon.png" : $"{_folderPath}/Assets/play-icon.png");

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button(icon,_skin.GetStyle("PlayButton")))
            {
                ProjectTimer.ToggleTimer();
            }
            
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            //Total time
            GUILayout.Label("Total project time", _skin.GetStyle("Surtitle-small"));
            GUILayout.Label(ProjectTimer.GetTotalTime(), _skin.GetStyle("TotalTimerLabel"));

            GUILayout.FlexibleSpace();
        }
    
        private void UpdateUI()
        {
            Repaint();
        }
    
    
    }
}
