using System;
using System.Drawing;
using System.Windows.Forms;
using Connect4.Model;
using Connect4.Persistance;


namespace Connect4WinForms
{
    public partial class GameForm : Form
    {
        private GameModel _gameModel;
        private Button[,] _buttonGrid;



        public GameForm()
        {
            InitializeComponent();

            IGameTableDataAccess _dataAccess = new GameTableDataAccess();
            _gameModel = new GameModel(_dataAccess);

            _gameModel.OnBoardChanged += new EventHandler<Connect4FieldEventArgs>(Game_BoardChanged);
            _gameModel.OnGameEnded += new EventHandler<Connect4EventArgs>(Game_Ended);
            _gameModel.OnTimerTick += new Eventhandler<EventArgs>(Game_TimerTick);
            
            SetupMenus();
        }

         

        private void SetupTable(int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                        [i, j].Text = _model[i,j] == FieldStatus.NONE
                            ? String.Empty
                            : _model[i, j].ToString();
                        
                        _buttonGrid[i, j].Enabled = true;
                        _buttonGrid[i, j].BackColor = Color.White;
                }
            }

        }
    }
}
