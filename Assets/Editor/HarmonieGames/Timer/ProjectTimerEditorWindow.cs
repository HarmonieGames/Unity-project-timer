using UnityEditor;
using UnityEngine;

namespace Editor.HarmonieGames.Timer
{
    public class ProjectTimerEditorWindow : EditorWindow
    {
        private GUISkin _skin;
        private string _folderPath;
        private Timer _timer;

        [MenuItem("Tools/Harmonie Games/Project Timer")]
        public static void OpenWindow()
        {
            var window = GetWindow<ProjectTimerEditorWindow>("Project Timer");
            window.minSize = new Vector2(250, 325);
            window.Show();
        }

        private void OnEnable()
        {
            _folderPath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)).Replace("/ProjectTimerEditorWindow.cs", "");
        
            _skin = AssetDatabase.LoadAssetAtPath<GUISkin>($"{_folderPath}/Stylesheet.guiskin");
        
            _timer = new Timer();
            _timer.LoadSessions(SaveTime.LoadSessions());
            _timer.SetTimer(true);
        
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
            GUILayout.Label("Current session time", _skin.GetStyle("CurrentTimerSurtitle"));
            GUILayout.Label(_timer.GetLastSessionTime(), _skin.GetStyle("CurrentTimerLabel"));

            //Button
            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(_timer.GetTimerStatus() ? $"{_folderPath}/Assets/pause-icon.png" : $"{_folderPath}/Assets/play-icon.png");

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button(icon,_skin.GetStyle("PlayButton")))
            {
                _timer.ToggleTimer();
            }
            
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            //Total time
        
            GUILayout.Label("Total project time", _skin.GetStyle("TotalTimerSurtitle"));
            GUILayout.Label(_timer.GetTotalTime(), _skin.GetStyle("TotalTimerLabel"));

            GUILayout.FlexibleSpace();
        }
    
        private void UpdateUI()
        {
            Repaint();
        }
    
    
    }
}
