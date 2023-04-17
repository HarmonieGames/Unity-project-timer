using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.HarmonieGames.Timer
{
    public class TimerEditor : EditorWindow
    {
        private VisualElement _root;

        //UI
        private Label _timerLabel;
        private Button _timerButton;
        private IMGUIContainer _buttonIcon;
        private Sprite _playIconSprite, _pauseIconSprite;

        //Timer
        private TimeSpan _timeSpan;
        private bool _isTimerRunning = true;
        private CancellationTokenSource _cancellationToken;
        
        [MenuItem("Tools/Harmonie Games/Timer test")]
        public static void ShowWindow()
        {
            var window = GetWindow<TimerEditor>();
            window.titleContent = new GUIContent("TimerEditor");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            _root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/HarmonieGames/Timer/TimerEditor.uxml");
            var visualElement = visualTree.Instantiate();
            _root.Add(visualElement);

            //SetupEditor();
        }

        private void SetupEditor()
        {
            //TimerLabel
            _timerLabel = _root.Query<Label>(className: "timer-label").First();
            
            //TimerButton
            _timerButton = _root.Query<Button>(className: "button-timer").First();
            _buttonIcon = _root.Query<IMGUIContainer>(className: "button-icon").First();
            
            _playIconSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Editor/HarmonieGames/Timer/Assets/pause-icon.png");
            _pauseIconSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Editor/HarmonieGames/Timer/Assets/play-icon.png");
            
            if (_timerButton != null) _timerButton.clicked += ToggleTimer;

            _timerLabel.text = _timeSpan.ToString("hh':'mm':'ss");
            
            var sessions = SaveTime.LoadSessions();
            
            if(sessions.Count == 0) return; //TODO:TOREMOVE
            
            _timeSpan = sessions[sessions.Count - 1].ToTimeSpan();
            
            SetTimer(true);
        }
        
        private void ToggleTimer()
        {
            SetTimer(!_isTimerRunning);
        }

        private void SetTimer(bool isRunning)
        {
            _isTimerRunning = isRunning;

            //Change buttonIcon sprite
            _buttonIcon.style.backgroundImage = _isTimerRunning ? new StyleBackground(_playIconSprite.texture) : new StyleBackground(_pauseIconSprite.texture);

            AwaitOneSecond();
        }

        private async void AwaitOneSecond()
        {
            if (_isTimerRunning == false && _cancellationToken != null)
            {
                _cancellationToken.Cancel();
                return;
            }

            _cancellationToken = new CancellationTokenSource();
            await WaitOneSecondTask(_cancellationToken.Token);

            AwaitOneSecond();
        }
    
        private async Task WaitOneSecondTask(CancellationToken token) {
            try {
                while (true) {
                    // Vérifier si la tâche a été annulée
                    token.ThrowIfCancellationRequested();

                    // Attendre 1 seconde
                    await Task.Delay(1000, token);

                    _timeSpan += new TimeSpan(0, 0, 1);
                    _timerLabel.text = _timeSpan.ToString("hh':'mm':'ss");
                    
                    //SaveTime.SaveTimeSpan(_timeSpan);
                }
            }
            catch (OperationCanceledException) {
                Console.WriteLine("Task Cancelled");
            }
        }
    }
}