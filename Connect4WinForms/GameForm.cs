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
        private Label[,] _labelGrid;
        private Button _buttons;



        public GameForm()
        {
            InitializeComponent();

            IGameTableDataAccess _dataAccess = new GameTableDataAccess();
        }
    }
}
