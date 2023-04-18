using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Editor.HarmonieGames.Timer
{
    public class Timer
    {
        private TimeSpan _timeSpan;
        private bool _isTimerRunning = true;
        private CancellationTokenSource _cancellationToken;
        private List<Session> _sessions;
        
        public static Action OnTimerUpdate;

        public string GetTime()
        {
            return _timeSpan.ToString("hh':'mm':'ss");
        }

        public bool GetTimerStatus()
        {
            return _isTimerRunning;
        }
    
        public void ToggleTimer()
        {
            SetTimer(!_isTimerRunning);
        }

        public void SetTimer(bool isRunning)
        {
            _isTimerRunning = isRunning;
            AwaitOneSecond();
        }

        public void LoadSessions(List<Session> sessions)
        {
            _sessions = sessions;
            AssignLastSession(sessions[sessions.Count - 1]);
        }

        public void AssignLastSession(Session session)
        {
            _timeSpan = session.ToTimeSpan();
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
                    OnTimerUpdate?.Invoke();

                    SaveTime.SaveTimeSpan(_timeSpan);
                }
            }
            catch (OperationCanceledException) {
                Console.WriteLine("Task Cancelled");
            }
        }
    }
}
