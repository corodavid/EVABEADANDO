namespace Connect4WinForms
{
    partial class GameForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            _menuFile = new ToolStripMenuItem();
            _menuFileNewGame = new ToolStripMenuItem();
            _menuFileLoadgame = new ToolStripMenuItem();
            _menuFileSaveGame = new ToolStripMenuItem();
            _openFileDialog = new OpenFileDialog();
            _saveFileDialog = new SaveFileDialog();
            _firstPlayer = new Label();
            _firstPlayerTimeLeftLabel = new Label();
            _secondPlayer = new Label();
            _secondPlayerTimeLeftLabel = new Label();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { _menuFile });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // _menuFile
            // 
            _menuFile.DropDownItems.AddRange(new ToolStripItem[] { _menuFileNewGame, _menuFileLoadgame, _menuFileSaveGame });
            _menuFile.Name = "_menuFile";
            _menuFile.Size = new Size(37, 20);
            _menuFile.Text = "File";
            // 
            // _menuFileNewGame
            // 
            _menuFileNewGame.Name = "_menuFileNewGame";
            _menuFileNewGame.Size = new Size(180, 22);
            _menuFileNewGame.Text = "New game";
            _menuFileNewGame.Click += _menuFileNewGame_Click;
            // 
            // _menuFileLoadgame
            // 
            _menuFileLoadgame.Name = "_menuFileLoadgame";
            _menuFileLoadgame.Size = new Size(180, 22);
            _menuFileLoadgame.Text = "Load game";
            _menuFileLoadgame.Click += _menuFileLoadgame_Click;
            // 
            // _menuFileSaveGame
            // 
            _menuFileSaveGame.Name = "_menuFileSaveGame";
            _menuFileSaveGame.Size = new Size(180, 22);
            _menuFileSaveGame.Text = "Save game";
            _menuFileSaveGame.Click += _menuFileSaveGame_Click;
            // 
            // _openFileDialog
            // 
            _openFileDialog.FileName = "openFileDialog1";
            _openFileDialog.Filter = "Connect4 table  (*.con)|*.con";
            _openFileDialog.Tag = "Load game";
            // 
            // _saveFileDialog
            // 
            _saveFileDialog.Filter = "Sudoku tábla (*.con)|*.con";
            _saveFileDialog.Title = "Save game";
            // 
            // _firstPlayer
            // 
            _firstPlayer.AutoSize = true;
            _firstPlayer.Location = new Point(637, 36);
            _firstPlayer.Name = "_firstPlayer";
            _firstPlayer.Size = new Size(51, 15);
            _firstPlayer.TabIndex = 1;
            _firstPlayer.Text = "Player 1:";
            // 
            // _firstPlayerTimeLeftLabel
            // 
            _firstPlayerTimeLeftLabel.AutoSize = true;
            _firstPlayerTimeLeftLabel.Location = new Point(709, 36);
            _firstPlayerTimeLeftLabel.Name = "_firstPlayerTimeLeftLabel";
            _firstPlayerTimeLeftLabel.Size = new Size(49, 15);
            _firstPlayerTimeLeftLabel.TabIndex = 2;
            _firstPlayerTimeLeftLabel.Text = "03:00:00";
            // 
            // _secondPlayer
            // 
            _secondPlayer.AutoSize = true;
            _secondPlayer.Location = new Point(637, 65);
            _secondPlayer.Name = "_secondPlayer";
            _secondPlayer.Size = new Size(51, 15);
            _secondPlayer.TabIndex = 3;
            _secondPlayer.Text = "Player 2:";
            // 
            // _secondPlayerTimeLeftLabel
            // 
            _secondPlayerTimeLeftLabel.AutoSize = true;
            _secondPlayerTimeLeftLabel.Location = new Point(709, 65);
            _secondPlayerTimeLeftLabel.Name = "_secondPlayerTimeLeftLabel";
            _secondPlayerTimeLeftLabel.Size = new Size(49, 15);
            _secondPlayerTimeLeftLabel.TabIndex = 4;
            _secondPlayerTimeLeftLabel.Text = "03:00:00";
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(_secondPlayerTimeLeftLabel);
            Controls.Add(_secondPlayer);
            Controls.Add(_firstPlayerTimeLeftLabel);
            Controls.Add(_firstPlayer);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "GameForm";
            Text = "Connect4";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem _menuFile;
        private ToolStripMenuItem _menuFileNewGame;
        private ToolStripMenuItem _menuFileLoadgame;
        private ToolStripMenuItem _menuFileSaveGame;
        private OpenFileDialog _openFileDialog;
        private SaveFileDialog _saveFileDialog;
        private Label _firstPlayer;
        private Label _firstPlayerTimeLeftLabel;
        private Label _secondPlayer;
        private Label _secondPlayerTimeLeftLabel;
    }
}
