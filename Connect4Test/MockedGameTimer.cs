using Connect4.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Test
{
    public class MockedGameTimer : IGameTimer
    {
        public bool IsRunning { get; private set; }
        public int RemainingTime { get; private set; }

        public event EventHandler<int>? TimerTick;
        public event EventHandler? TimeExpired;

        public MockedGameTimer(int startTime = 10)
        {
            RemainingTime = startTime;
        }

        public void Start()
        {
            IsRunning = true;
        }

        public void Pause()
        {
            IsRunning = false;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void Resume()
        {
            if (RemainingTime > 0)
                IsRunning = true;
        }

        public void Reset(int n)
        {
            RemainingTime = n;
            TimerTick?.Invoke(this, RemainingTime);
        }

        public void RaiseTicked()
        {
            if (!IsRunning)
                return;

            RemainingTime--;
            TimerTick?.Invoke(this, RemainingTime);

            if (RemainingTime <= 0)
            {
                IsRunning = false;
                TimeExpired?.Invoke(this, EventArgs.Empty);
            }
        }
    }

}
