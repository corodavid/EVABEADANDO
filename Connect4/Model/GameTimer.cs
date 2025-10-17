using System;
using System.ComponentModel.DataAnnotations;
using System.Timers;

namespace Connect4.Model
{
    public class GameTimer : IGameTimer
    {
        private System.Timers.Timer _timer;
        private int _remainingTime;
        private bool _isRunning;

        public event EventHandler<int>? TimerTick;
        public event EventHandler? TimeExpired;

        public bool IsRunning { get { return _isRunning; } }
        public int RemainingTime { get { return _remainingTime; } }

        public GameTimer(int startTimeInSeconds)
        {
            _remainingTime = startTimeInSeconds;
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = true;
        }

        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            if (!_isRunning) return;

            _remainingTime--;
            TimerTick?.Invoke(this, _remainingTime);

            if (_remainingTime <= 0)
            {
                Stop();
                TimeExpired?.Invoke(this,EventArgs.Empty);
            }
        }

        public void Start()
        {
            _isRunning = true;
            _timer.Start();
        }

        public void Stop()
        {
            _isRunning = false;
            _timer.Stop();
        }

        public void Reset(int newTime)
        {
            _remainingTime = newTime;
            TimerTick?.Invoke(this, _remainingTime);
        }

        public void Pause() => Stop();

        public void Resume()
        {
            if (_remainingTime > 0)
                Start();
        }

        public void RaiseTicked()
        {
            TimerTick?.Invoke(this, -1);
        }
    }
}
