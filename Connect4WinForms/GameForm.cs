using System;
using System.Drawing;
using System.Runtime.CompilerServices;
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

            InitializeModel();

            GenerateTable();
            //SetupMenus();
            //SetupTable();

            _gameModel!.StartGame();

        }


        public void InitializeModel()
        {
            _gameModel = new GameModel(10);
            _gameModel.BoardChanged += new EventHandler<Connect4FieldEventArgs>(Game_BoardChanged);
            _gameModel.GameEnded += new EventHandler<Connect4EventArgs>(Game_Ended);
            _gameModel.TimerTick += new EventHandler<Connect4TimerEventArgs>(Game_TimerTick);
        }

        private void GenerateTable()
        {
            _buttonGrid = new Button[_gameModel.TableSize, _gameModel.TableSize];
            for (Int32 i = 0; i < _gameModel.TableSize; i++)
                for (Int32 j = 0; j < _gameModel.TableSize; j++)
                {
                    _buttonGrid[i, j] = new Button();
                    _buttonGrid[i, j].Location = new Point(5 + 50 * j, 35 + 50 * i); // elhelyezkedés
                    _buttonGrid[i, j].Size = new Size(50, 50); // méret
                    _buttonGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold); // betûtípus
                    _buttonGrid[i, j].Enabled = true;
                    _buttonGrid[i, j].Text = String.Empty;
                    _buttonGrid[i, j].TabIndex = 100 + i * _gameModel.TableSize + j; // a gomb számát a TabIndex-ben tároljuk
                    _buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lapított stípus
                    _buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    // közös eseménykezelõ hozzárendelése minden gombhoz
                    _buttonGrid[i, j].BackColor = Color.White;
                    Controls.Add(_buttonGrid[i, j]);
                    // felvesszük az ablakra a gombot
                }
        }

        /// <summary>
        /// Inserts values of the model into the buttons.
        /// To be used when a save is loaded!!!
        /// </summary>
        private void SetupTable()
        {
            for (int i = 0; i < _gameModel.TableSize; i++)
            {
                for (int j = 0; j < _gameModel.TableSize; j++)
                {
                    _buttonGrid[i, j].BackColor = Color.White;
                    _buttonGrid[i, j].Text = _gameModel[i, j] != FieldStatus.NONE ? _gameModel[i, j].ToString() : String.Empty;
                }
            }

        }

        private void ResetTable()
        {
            for (int i = 0; i < _gameModel.TableSize; i++)
            {
                for (int j = 0; j < _gameModel.TableSize; j++)
                {
                    _buttonGrid[i, j].MouseClick -= new MouseEventHandler(ButtonGrid_MouseClick);
                    Controls.Remove(_buttonGrid[i, j]);
                    _buttonGrid[i, j].Dispose();

                }
            }
        }


        private void Game_TimerTick(object sender, Connect4TimerEventArgs args)
        {
            WhichPlayer whichPlayer = args.WhichPlayer;
            if (whichPlayer == WhichPlayer.FIRST)
            {
                if (_firstPlayerTimeLeftLabel.InvokeRequired)
                    _firstPlayerTimeLeftLabel.Invoke(new Action(() =>
                    {
                        _firstPlayerTimeLeftLabel.Text = TimeSpan.FromSeconds(args.RemaingTime).ToString("g");
                    }));
            }
            else
            {
                if (_secondPlayerTimeLeftLabel.InvokeRequired)
                    _secondPlayerTimeLeftLabel.Invoke(new Action(() =>
                    {
                        _secondPlayerTimeLeftLabel.Text = TimeSpan.FromSeconds(args.RemaingTime).ToString("g");
                    }));
            }
        }

        private void Game_Ended(object sender, Connect4EventArgs args)
        {
            GameState state = args.GameState;
            (int Row, int Col)[] coords = args.WinningCoordinates;
            switch (state)
            {
                case GameState.DRAW:
                    MessageBox.Show("Game ended in a draw!!!", "Game ended", MessageBoxButtons.OK);
                    break;
                case GameState.FIRST:
                    MessageBox.Show("First PLayer WINS!!!", "Game ended", MessageBoxButtons.OK);
                    break;
                case GameState.SECOND:
                    MessageBox.Show("Second PLayer WINS!!!", "Game ended", MessageBoxButtons.OK);
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
            ResetTable();
            _firstPlayerTimeLeftLabel.Text = "03:00:00";
            _secondPlayerTimeLeftLabel.Text = "03:00:00";
            Random random = new Random();
            _gameModel.NewGame(random.Next(7,12));
            GenerateTable();
            _gameModel.StartGame();

        }

        private async void _menuFileSaveGame_Click(object sender, EventArgs e)
        {
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    bool restartNeeded = _gameModel.State == GameState.RUNNING;
                    if (restartNeeded)
                        _gameModel.Pause();

                    await _gameModel.SaveAsync(_saveFileDialog.FileName);
                    if(restartNeeded)
                        _gameModel.Resume();
                    // játék mentése
                }
                catch (Connect4FileException)
                {
                    MessageBox.Show(
                        "Game saving was unsuccessul!" + Environment.NewLine +
                       "Incorrect path to file or format.", "Error!", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

        }



        private void ButtonGrid_MouseClick(object sender, EventArgs args)
        {
            if (sender is Button button)
            {

                // a TabIndex-bõl megkapjuk a sort és oszlopot
                Int32 x = (button.TabIndex - 100) / _gameModel.TableSize;
                Int32 y = (button.TabIndex - 100) % _gameModel.TableSize;

                _gameModel.Round(y); // lépés a játékban
            }
        }

        private async void _menuFileLoadgame_Click(object sender, EventArgs e)
        {
            if(_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try 
                {
                    ResetTable();
                    await _gameModel.LoadAsync(_openFileDialog.FileName);
                    GenerateTable();
                    SetupTable();
                    _gameModel.Resume();
                }
                catch(Connect4FileException) 
                {
                    MessageBox.Show(
                       "Game loading was unsuccessul!" + Environment.NewLine +
                       "Incorrect path to file or format.", "Error!", MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                }
            }
        }
    }
}
