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

            IGameTableDataAccess _dataAccess = null;
            _gameModel = new GameModel(_dataAccess);

            _gameModel.BoardChanged += new EventHandler<Connect4FieldEventArgs>(Game_BoardChanged);
            _gameModel.GameEnded += new EventHandler<Connect4EventArgs>(Game_Ended);
            //_gameModel.TimerTick += new EventHandler<Connect4TimerEventArgs>(Game_TimerTick);

            //SetupMenus();
            SetupTable(10);
        }



        private void SetupTable(int n)
        {
            
            _buttonGrid = new Button[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    _buttonGrid[i, j] = new Button();
                    _buttonGrid[i, j].Text = String.Empty;

                    _buttonGrid[i, j].Enabled = true;
                    _buttonGrid[i, j].BackColor = Color.White;
                    Controls.Add(_buttonGrid[i, j]);
                }
            }

        }

        private void Game_Ended(object sender, Connect4EventArgs args)
        {
            GameState state = args.GameState;
            (int Row, int Col)[] coords = args.WinningCoordinates;
            switch (state)
            {
                case GameState.DRAW:
                    break;
                case GameState.FIRST:
                    break;
                case GameState.SECOND:
                    break;
                default:
                    throw new Exception();
            }
        }

        private void Game_BoardChanged(object sender, Connect4FieldEventArgs args)
        {
            _buttonGrid[args.X, args.Y].Text = _gameModel[args.X, args.Y].ToString();
        }

        private void _menuFileNewGame_Click(object sender, EventArgs e)
        {
            _gameModel.Pause();

            if (_openFileDialog.ShowDialog() == DialogResult.OK) // ha kiválasztottunk egy fájlt
            {
                try
                {
                    // játék betöltése
                    await _model.LoadGameAsync(_openFileDialog.FileName);
                    _menuFileSaveGame.Enabled = true;
                }
                catch (Connect4FileException)
                {
                    MessageBox.Show(
                        "Játék betöltése sikertelen!" + Environment.NewLine +
                        "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    _model.NewGame();
                    _menuFileSaveGame.Enabled = true;
                }

                SetupTable();
            }
        }
    }
}
