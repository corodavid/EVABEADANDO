using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Model
{
    public class Connect4TimerEventArgs : EventArgs
    {
        WhichPlayer _whichPlayer;
        int _remainingTime;

        public Connect4TimerEventArgs(WhichPlayer whichPlayer, int remainingTime)
        {
            _whichPlayer = whichPlayer;
            _remainingTime = remainingTime;
        }
    }
}
