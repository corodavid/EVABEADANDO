using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Model
{
    public class Connect4EventArgs : EventArgs
    {
        private GameState _gameState;
        private (int Row, int Col)[] _winningFieldCoordinates;

        public GameState GameState { get { return _gameState; } }

        public (int Row, int Col)[] WinningCoordinates { get { return _winningFieldCoordinates; } }

        public Connect4EventArgs(GameState gameState, (int Row, int Col)[] winningFieldCoordinates)
        {
            _gameState = gameState;
            _winningFieldCoordinates = winningFieldCoordinates;

        }


    }
}
