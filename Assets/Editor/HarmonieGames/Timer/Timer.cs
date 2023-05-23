using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Editor.HarmonieGames.Timer
{
    public class Timer
    {
        private TimeSpan _timeSpan;
        private bool _isTimerRunning = true;
        private CancellationTokenSource _cancellationToken;
        private List<Session> _sessions;
        
        public static Action OnTimerUpdate;

        public string GetLastSessionTime()
        {
            return _timeSpan.ToString("hh':'mm':'ss");
        }

        public string GetTotalTime()
        {
            var totalTime = new TimeSpan();

            totalTime = _sessions.Aggregate(totalTime, (current, session) => current + session.ToTimeSpan());

            return $"{totalTime.TotalHours:00}:{totalTime.Minutes:00}:{totalTime.Seconds:00}";
        }

        public bool GetTimerStatus()
        {
            return _isTimerRunning;
        }
        
        public Session[] GetSessions()
        {
            return _sessions.ToArray();
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
            _cancellationToken?.Cancel();

            if (_isTimerRunning == false) return;

            _cancellationToken = new CancellationTokenSource();
            await WaitOneSecondTask(_cancellationToken.Token);

            AwaitOneSecond();
        }
    
        private async Task WaitOneSecondTask(CancellationToken token) {
            try {
                while (true) {
                    //Check if the task has been cancelled
                    token.ThrowIfCancellationRequested();

                    //Wait 1 second
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
