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
            _menuFileNewGame.Size = new Size(133, 22);
            _menuFileNewGame.Text = "New game";
            _menuFileNewGame.Click += _menuFileNewGame_Click;
            // 
            // _menuFileLoadgame
            // 
            _menuFileLoadgame.Name = "_menuFileLoadgame";
            _menuFileLoadgame.Size = new Size(133, 22);
            _menuFileLoadgame.Text = "Load game";
            // 
            // _menuFileSaveGame
            // 
            _menuFileSaveGame.Name = "_menuFileSaveGame";
            _menuFileSaveGame.Size = new Size(133, 22);
            _menuFileSaveGame.Text = "Save game";
            _menuFileSaveGame.Click += _menuFileSaveGame_Click;
            // 
            // _openFileDialog
            // 
            _openFileDialog.FileName = "openFileDialog1";
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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
    }
}
