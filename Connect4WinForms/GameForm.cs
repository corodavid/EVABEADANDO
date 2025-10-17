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

        #region Fields
        
        
        private GameModel _gameModel = null!;
        private Button[,] _buttonGrid = null!;

        #endregion


        #region Constructors

        public GameForm()
        {
            InitializeComponent();

            InitializeModel();

            GenerateTable();
            _gameModel!.StartGame();
           

        }

        #endregion


        #region Initialization

        public void InitializeModel()
        {
            _gameModel = new GameModel(7);
            _gameModel.BoardChanged += new EventHandler<Connect4FieldEventArgs>(Game_BoardChanged);
            _gameModel.GameEnded += new EventHandler<Connect4EventArgs>(Game_Ended);
            _gameModel.TimerTick += new EventHandler<Connect4TimerEventArgs>(Game_TimerTick);
        }



        /// <summary>
        /// Creates a new button grid, initializes all of them
        /// </summary>

        private void GenerateTable()
        {
            _buttonGrid = new Button[_gameModel.TableSize, _gameModel.TableSize];
            for (Int32 i = 0; i < _gameModel.TableSize; i++)
                for (Int32 j = 0; j < _gameModel.TableSize; j++)
                {
                    _buttonGrid[i, j] = new();
                    _buttonGrid[i, j].Location = new Point(5 + 50 * j, 35 + 50 * i); // elhelyezkedés
                    _buttonGrid[i, j].Size = new Size(50, 50); // méret
                    _buttonGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold); // betûtípus
                    _buttonGrid[i, j].Enabled = true;
                    _buttonGrid[i, j].Text = String.Empty;
                    _buttonGrid[i, j].TabIndex = 100 + i * _gameModel.TableSize + j; // a gomb számát a TabIndex-ben tároljuk
                    _buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lapított stípus
                    _buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick!);
                    // közös eseménykezelõ hozzárendelése minden gombhoz
                    _buttonGrid[i, j].BackColor = Color.White;
                    Controls.Add(_buttonGrid[i, j]);
                    // felvesszük az ablakra a gombot
                }
        }

        #endregion

        #region private Table methods

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
                    _buttonGrid[i,j].Enabled = _gameModel[i,j] == FieldStatus.NONE;
                }
            }

        }
        /// <summary>
        /// Disposes all of the buttons inside of button grid.
        /// ALWAYS to be called before any model methods that change the size of the table.
        /// </summary>
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

        #endregion


        #region Model event handlers

        /// <summary>
        /// Changes the shown time every second.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>

        private void Game_TimerTick(object? sender, Connect4TimerEventArgs args)
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
        /// <summary>
        /// Highlights the winning fields if thez exist.
        /// Shows the result in a MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args">Contains the state and the winning coords, if the latter is null we skip highlighting.</param>
        /// <exception cref="Exception"></exception>
        private void Game_Ended(object? sender, Connect4EventArgs args)
        {
            GameState state = args.GameState;
            (int Row, int Col)[]? coords = args.WinningCoordinates;
            if(coords != null)
            {
                for(int i = 0; i < coords.Length; i++) 
                {
                    _buttonGrid[coords[i].Row, coords[i].Col].BackColor = Color.Red;
                }
            }
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

        /// <summary>
        /// Inputs the changes of the model into the UI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>

        private void Game_BoardChanged(object? sender, Connect4FieldEventArgs args)
        {
            _buttonGrid[args.X, args.Y].Text = _gameModel[args.X, args.Y].ToString();
            _buttonGrid[args.X, args.Y].Enabled = false;
        }

        #endregion

        #region Menu event handlers
        /// <summary>
        /// Pauses the game for consistency, resets the table, 
        /// then generates a new one.
        /// TODO: Input the size of the field!!!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileNewGame_Click(object? sender, EventArgs e)
        {
            _gameModel.Pause();
            ResetTable();
            _firstPlayerTimeLeftLabel.Text = "03:00:00";
            _secondPlayerTimeLeftLabel.Text = "03:00:00";
            _gameModel.NewGame((int)_sizeOfNextTable.Value);
            GenerateTable();
            _gameModel.StartGame();

        }
        /// <summary>
        /// Checks if the game is running, and in that case will resume it after saving.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void menuFileSaveGame_Click(object? sender, EventArgs e)
        {
            _gameModel.Pause();
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    await _gameModel.SaveAsync(_saveFileDialog.FileName);

                }
                catch (Connect4FileException)
                {
                    MessageBox.Show(
                        "Game saving was unsuccessul!" + Environment.NewLine +
                       "Incorrect path to file or format.", "Error!", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                _gameModel.Resume();
            }

        }

        /// <summary>
        /// Gets the coordinates from the button and then passes the model the column.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>

        private void ButtonGrid_MouseClick(object? sender, MouseEventArgs args)
        {
            if (sender is Button button)
            {

                // a TabIndex-bõl megkapjuk a sort és oszlopot
                Int32 y = (button.TabIndex - 100) % _gameModel.TableSize;

                _gameModel.Round(y); // lépés a játékban
            }
        }

        /// <summary>
        /// Resets the table and loads in the given table from the file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private async void menuFileLoadgame_Click(object sender, EventArgs e)
        {
            _gameModel.Pause();
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
            else { _gameModel.Resume(); }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _gameModel.Pause();

            if (MessageBox.Show("Biztosan ki szeretne lépni?", "Sudoku játék", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
            { 
                _gameModel.Resume();
                e.Cancel = true;
            }
        }

        #endregion
    }
}
