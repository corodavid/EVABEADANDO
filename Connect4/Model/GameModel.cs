using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connect4.Persistance;
using Connect4.Model;
using System.Runtime.Serialization.Formatters;
using System.Drawing;




namespace Connect4.Model
{

    public enum WhichPlayer
    {
        FIRST,
        SECOND
    }

    public enum GameState
    {
        RUNNING,
        PAUSED,
        DRAW,
        FIRST,
        SECOND
    }


    public class GameModel
    {
        #region Fields

        
        private GameTable _table; //Table of the values

        private WhichPlayer _whichPlayer;

        private IGameTableDataAccess _dataAccess;

        private GameState _state;

        private GameTimer _firstPlayerTimer;

        private GameTimer _secondPlayerTimer;


        #endregion


        #region Properties

        public GameState State { get { return _state; } }

        public FieldStatus this[int row, int col]
        {
            get { return _table[row, col]; }
            
            set { _table[row, col] = value; }
        }



        #endregion

        #region Events
        public event EventHandler<Connect4TimerEventArgs>? TimerTick;
        public event EventHandler<Connect4FieldEventArgs>? BoardChanged;
        public event EventHandler<Connect4EventArgs>? GameEnded;


        #endregion

        #region Constructors

        public GameModel(GameTable gameTable)
        {
            _table = gameTable;
            _whichPlayer = WhichPlayer.FIRST; // first player
            _dataAccess = new GameTableDataAccess();
            _state = GameState.RUNNING;

            _firstPlayerTimer = new GameTimer(180);
            _secondPlayerTimer = new GameTimer(180);

            _firstPlayerTimer.TimerTick += OnTimerTick;
            _secondPlayerTimer.TimerTick += OnTimerTick;

            _firstPlayerTimer.TimeExpired += OnFirstPlayerExpired;
            _secondPlayerTimer.TimeExpired += OnSecondPlayerExpired;

            //_firstPlayerTimer.Start();

        }

        public GameModel(IGameTableDataAccess dataAccess) 
        {
            _dataAccess = dataAccess;
        }

        #endregion

        #region Public methods


        public void StartGame()
        {
            _whichPlayer = WhichPlayer.FIRST;
            _firstPlayerTimer.Start();
        }

        /// <summary>
        /// Tries to insert in the given column, if it is full it does nothing
        /// It triggers BoardChanged after it inserted into the table
        /// It triggers GameEnded if there is a winner by move or a draw if it is full
        /// </summary>
        /// <param name="col">The y coordinate of the button</param>
        public void Round(int col)
        {
            TryInsert(col);
            _state = CurrentGameState(col);
            int row = _table.FirstNoneFieldInColumn(col) + 1;
            BoardChanged?.Invoke(this, new Connect4FieldEventArgs(row, col));
            switch (State)
            {
                case GameState.FIRST:
                    {
                        //Console.WriteLine($"THE FIRST PLAYER WINS!!!\nWinning Coords:");
                        (int Row,int Col)[] winners = findWinners(_table.FirstNoneFieldInColumn(col) + 1,col);
                        GameEnded?.Invoke(this, new Connect4EventArgs(GameState.FIRST, winners));
                        return;
                    }
                case GameState.SECOND: 
                    {
                        //Console.WriteLine($"THE SECOND PLAYER WINS!!!\nWinning Coords:");
                        (int, int)[] winners = findWinners(_table.FirstNoneFieldInColumn(col) + 1, col);
                        GameEnded?.Invoke(this, new Connect4EventArgs(GameState.SECOND, winners));
                        return;
                    }
                default:
                    break;
            }

            if(_whichPlayer == WhichPlayer.FIRST)
            {
                _whichPlayer = WhichPlayer.SECOND;
                _firstPlayerTimer.Stop();
                _secondPlayerTimer.Start();
            }
            else
            {
                _whichPlayer = WhichPlayer.FIRST;
                _secondPlayerTimer.Stop();
                _firstPlayerTimer.Start();
            }
        }

        public void Pause()
        {
            if (_whichPlayer == WhichPlayer.FIRST)
                _firstPlayerTimer.Pause();
            else
                _secondPlayerTimer.Pause();
            _state = GameState.PAUSED;

        }

        public void Resume()
        {
            if (_whichPlayer == WhichPlayer.FIRST)
                _firstPlayerTimer.Resume();
            else
                _secondPlayerTimer.Resume();
            _state = GameState.RUNNING;
        }



        /// <summary>
        /// True if insertion is successfull
        /// </summary>
        /// <param name="col">The column of the most recent click</param>
        /// <returns></returns>
        public bool TryInsert(int col)
        {
            if (_table.IsColumnFull(col))
                return false;
            int row = _table.FirstNoneFieldInColumn(col);
            FieldStatus fieldStatus;
            if (_whichPlayer == WhichPlayer.FIRST)
                fieldStatus = FieldStatus.X;
            else
                fieldStatus = FieldStatus.O;
            _table[row, col] = fieldStatus;
            return true;
        }

        /// <summary>
        /// Runs all the checks for wins, and draw
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>

        public GameState CurrentGameState(int col)
        {
            if (_table.IsFilled)
                return GameState.DRAW;
            int row = _table.FirstNoneFieldInColumn(col) + 1;
            if (checkColumn(row,col))
            {
                return _whichPlayer == WhichPlayer.FIRST ? GameState.FIRST : GameState.SECOND;
            }
            if (checkRow(row,col))
            {
                return _whichPlayer == WhichPlayer.FIRST ? GameState.FIRST : GameState.SECOND;
            }
            if(checkDiagonals(row,col))
            {
                return _whichPlayer == WhichPlayer.FIRST ? GameState.FIRST : GameState.SECOND;
            }
            return GameState.RUNNING;



        }


        #endregion

        #region Public File

        public async Task LoadGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");
            _table = await _dataAccess.LoadAsync(path);
            int db = 0;
            for(int i = 0; i < _table.Size; i++)
            {
                for(int j = 0; j < _table.Size;j++)
                {
                    if (_table[i, j] != FieldStatus.NONE)
                        db++; 
                }
            }
            if(db%2 == 0)
            {
                _whichPlayer = WhichPlayer.FIRST;
            }

        }

        #endregion

        #region Private methods

        /// <summary>
        /// Its enough to check 3 cells downwards
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>

        private bool checkColumn(int row, int col)
        {         
            int counter = 0;
            for (int i = row + 1;counter < 3 && i < _table.Size && counter > -1; i++)
            {
                if (_table[row, col] != _table[i, col])
                    counter = -1;
                else
                    counter++;
            }
            return counter == 3;
        }

        private bool checkRow(int row, int col)
        { 
            int counter = 0;
            for (int j = (col - 3) > 0 ? col - 3 : 0; j < col + 4 && counter < 4 && j < _table.Size; j++)
            {
                if (_table[row, j] == _table[row, col])
                    counter++;
                else
                    counter = 0;
            }
            return counter == 4;
                
        }

        /// <summary>
        /// Checking from left to right both diagonals
        /// top to bottom
        /// bottom to top
        /// 
        /// The board is flipped 
        /// (0,0) (0,1) ... (0,n)
        /// .   .             .  
        /// .      .          .  
        /// .          .      .  
        /// (n,0) (n,1) ... (n,n)
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>


        public  bool checkDiagonals(int row, int col)
        {
            int counter = 0;

            for(int increment = startingPointUpToDown(row,col); increment <= 3 &&
                row + increment >= 0 && row + increment < _table.Size &&
                col + increment >= 0 && col + increment < _table.Size
                && counter < 4; increment++)
            {
                if (_table[row + increment, col + increment] == _table[row, col])
                    counter++;
                else
                    counter = 0;
            }
            if(counter == 4)
                return true;

            counter = 0;

            for (int increment = startingPointDownToUp(row, col); increment <= 3 &&
                row - increment >= 0 && row - increment < _table.Size &&
                col + increment >= 0 && col + increment < _table.Size
                && counter < 4; increment++)
            {
                if (_table[row - increment, col + increment] == _table[row, col])
                    counter++;
                else
                    counter = 0;
            }
            return counter == 4;
        }


        /// <summary>
        /// Gives you the first possible increment for which both values are valid (0..n-1)
        /// For ↘ check
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private int startingPointUpToDown(int row, int col)
        {
            if (row >= 3 && col >= 3)
                return -3;
            return Math.Max(-row, -col);
        }

        /// <summary>
        /// Gives you the first possible increment for which both values are valid (0..n-1)
        /// For ↗ check
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private int startingPointDownToUp(int row , int col)
        {
            if (row + 3 < _table.Size && col >= 3)
                return -3;
            return Math.Max(-col,row - _table.Size + 1);
        }


        /// <summary>
        /// Finds the winners and return them in an array of int tuples
        /// </summary>
        /// <param name="row"> row of the newest insertion</param>
        /// <param name="col"> column of the newest insertion</param>
        /// 

        private (int,int)[] findWinners(int row, int col)
        {
            int counter = 0;
            (int, int)[] winners = new (int, int)[4];
            for (int i = row; counter < 4 && i < _table.Size && counter > -1; i++)
            {
                if (_table[row, col] == _table[i, col])
                {
                    winners[counter] = (i, col);
                    counter++;
                }
                else
                    counter = -1;
            }
            if (counter == 4)
                return winners;
            counter = 0;
            winners = new (int, int)[4];
            for (int j = (col - 3) > 0 ? col - 3 : 0; j < col + 4 && counter < 4 && j < _table.Size; j++)
            {
                if (_table[row, j] == _table[row, col])
                {
                    winners[counter] = (row,j);
                    counter++;
                }
                else
                    counter = 0;
            }
            if (counter == 4)
                return winners;
            winners = new (int, int)[4];
            counter = 0;
            for (int increment = startingPointUpToDown(row, col); increment <= 3 &&
               row + increment >= 0 && row + increment < _table.Size &&
               col + increment >= 0 && col + increment < _table.Size
               && counter < 4; increment++)
            {
                if (_table[row + increment, col + increment] == _table[row, col])

                {
                    winners[counter] = (row+increment, col+increment);
                    counter++;
                }
                else
                    counter = 0;
            }
            if (counter == 4)
                return (winners);

            counter = 0;
            winners = new (int, int)[4];
            for (int increment = startingPointDownToUp(row, col); increment <= 3 &&
                row - increment >= 0 && row - increment < _table.Size &&
                col + increment >= 0 && col + increment < _table.Size
                && counter < 4; increment++)
            {
                if (_table[row - increment, col + increment] == _table[row, col])
                {
                    winners[counter] = (row - increment, col + increment);
                    counter++;
                }
                else
                    counter = 0;
            }
            if (counter == 4)
                return winners;
            throw new NoWinnersFoundException();

        }
        #endregion

        #region Private event handlers

        private void OnBoardChanged(int row, int col)
        {
            BoardChanged?.Invoke(this, new Connect4FieldEventArgs(row, col));
        }

        private void OnGameEnded((int Row,int Col)[] winners)
        {
            _firstPlayerTimer.Stop();
            _secondPlayerTimer.Stop();
            GameEnded?.Invoke(this, new Connect4EventArgs(_state, winners));
        }


        #endregion

        #region Private timer event Handlers

        private void OnTimerTick(object sender, int remainingTime)
        {
            TimerTick?.Invoke(this,new Connect4TimerEventArgs(_whichPlayer, remainingTime));
        }

        private void OnFirstPlayerExpired(object? sender, EventArgs e)
        {
            _state = GameState.SECOND;
            _firstPlayerTimer.Stop();
            _secondPlayerTimer.Stop();
            OnGameEnded(null);
            
        }

        private void OnSecondPlayerExpired(object? sender, EventArgs e)
        {
            _state = GameState.FIRST;
            _firstPlayerTimer.Stop();
            _secondPlayerTimer.Stop();
            OnGameEnded(null);
        }

        #endregion
    }

}
