using System;
using System.Drawing;
using System.Windows.Forms;
using Connect4.Model;
using Connect4.Persistance;


namespace Connect4WinForms
{
    public partial class GameForm : Form
    {
        private int _size;
        private GameModel _gameModel;
        private Button[,] _buttonGrid;



        public GameForm()
        {
            InitializeComponent();
            _size = 10;
            _gameModel = new GameModel(10);

            _gameModel.BoardChanged += new EventHandler<Connect4FieldEventArgs>(Game_BoardChanged);
            _gameModel.GameEnded += new EventHandler<Connect4EventArgs>(Game_Ended);
            //_gameModel.TimerTick += new EventHandler<Connect4TimerEventArgs>(Game_TimerTick);
            //SetupMenus();
            GenerateTable();
            SetupTable();
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
                    _buttonGrid[i, j].TabIndex = 100 + i * _gameModel.TableSize + j; // a gomb számát a TabIndex-ben tároljuk
                    _buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lapított stípus
                    _buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    // közös eseménykezelõ hozzárendelése minden gombhoz

                    Controls.Add(_buttonGrid[i, j]);
                    // felvesszük az ablakra a gombot
                }
        }


        private void SetupTable()
        {

            _buttonGrid = new Button[_size, _size];
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    _buttonGrid[i, j] = new Button();
                    _buttonGrid[i, j].Text = String.Empty;
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
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    _buttonGrid[i, j].MouseClick -= new MouseEventHandler(ButtonGrid_MouseClick);
                    Controls.Remove(_buttonGrid[i, j]);
                    _buttonGrid[i, j].Dispose();

                }
            }  
            Random random = new Random();
            _size = 8;
            _gameModel = new GameModel(_size);
            GenerateTable();
            SetupTable();
        }

        private void _menuFileSaveGame_Click(object sender, EventArgs e)
        {
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    throw new Connect4FileException();
                    // játé mentése
                    //await _gameModel.SaveAsync(_saveFileDialog.FileName);
                }
                catch (Connect4FileException)
                {
                    MessageBox.Show(
                        "Játék mentése sikertelen!" + Environment.NewLine +
                        "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButtons.OK,
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
    }
}
