using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Model
{
    public interface IGameTimer
    {
        bool IsRunning { get; }
        int RemainingTime { get; }

        event EventHandler<int>? TimerTick;
        event EventHandler? TimeExpired;

        void Start();

        void Pause();

        void Stop();

        void Resume();

        void Reset(int n);

        void RaiseTicked();
    }
}
