/*
    Copyright (C) 2015  Yan A. 

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ludus_Latrunculorum
{
    public partial class MainMenu : Form
    {
        private         Board   game;

        public  static  bool    ai                  = true;
        public  static  int     playerColour        = 1;
        public  static  string  msg                 = "";
        private         bool    didChoose           = false;
        private         int     srcX                = 0;
        private         int     srcY                = 0;
        private const   int     WM_NCHITTEST        = 0x84;
        private const   int     HT_CLIENT           = 0x1;
        private const   int     HT_CAPTION          = 0x2;
        private const   int     WM_NCLBUTTONDBLCLK  = 0x00A3;

        public MainMenu() {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));

            InitializeComponent(); 
        }

        
        // Makes the form movable && disable double click for fullscreen.
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_NCLBUTTONDBLCLK) {
                m.Result = IntPtr.Zero;
                return;
            }

            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }

        // ClickHandler for gameButton.
        private void GameButton_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            ai = aiCheck.Checked;
            playerColour = whiteBullet.Checked ? 1 : -1;

            this.cpuImg.Location = playerColour == 1 ? new Point(515, 30) : new Point(515, 540);
            if (ai)
                this.cpuImg.Show();
            else
                this.cpuImg.Hide();

            if (b.Text == "Play") {
                b.Text = "Restart";
                game = new Board();
                PrintBoard();
            } else {
                RestartBoard();
                this.msgBox.Text = "";
                msg = "";
            }
        }

        // ClickHandler for settingButton.
        private void SettingsButton_Click(object sender, EventArgs e)
        {
            if (!aiCheck.Visible) {
                ShowSettings();
            } else {
                HideSettings();
            }
        }

        // ClickHandler for exitButton
        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ClickHandler for piece.
        private void Piece_Click(object sender, EventArgs e)
        {
            int[] arr;

            PictureBox b = (PictureBox)sender;
            if ((game.GetAIState() && playerColour == game.GetTurn()) || !game.GetAIState()) { 
                if (!didChoose) {
                    if (game.GetPiece(b.Location.Y / 57, b.Location.X / 57) != null && game.GetPiece(b.Location.Y / 57, b.Location.X / 57).GetColour() == game.GetTurn()) {
                        srcX = b.Location.Y / 57;
                        srcY = b.Location.X / 57;
                        didChoose = true;

                        // Selects the piece.
                        this.piecesImg[srcX, srcY].Image = ImageHandler.PlaceImage(game.GetPiece(srcX, srcY).GetColour(), game.GetPiece(srcX, srcY).IsDux(), true);
                    }
                } else {
                    if (game.GetPiece(b.Location.Y / 57, b.Location.X / 57) != null && game.GetPiece(b.Location.Y / 57, b.Location.X / 57).GetColour() == game.GetTurn()) {
                        // Reverts the previous selected piece.
                        this.piecesImg[srcX, srcY].Image = ImageHandler.PlaceImage(game.GetPiece(srcX, srcY).GetColour(), game.GetPiece(srcX, srcY).IsDux(), false);

                        srcX = b.Location.Y / 57;
                        srcY = b.Location.X / 57;

                        // Selects the piece.
                        this.piecesImg[srcX, srcY].Image = ImageHandler.PlaceImage(game.GetPiece(srcX, srcY).GetColour(), game.GetPiece(srcX, srcY).IsDux(), true);
                    } else if (game.GetPiece(b.Location.Y / 57, b.Location.X / 57) == null && game.UpdateBoard(srcX, srcY, b.Location.Y / 57, b.Location.X / 57)) {
                        UpdateBoard(srcX, srcY, b.Location.Y / 57, b.Location.X / 57);
                        didChoose = false;
                    }
                }
            }
            if (game.GetTurn() == game.GetPlayerColour() * -1 && (game.GetAIState() && !didChoose)) {
                Application.DoEvents();
                arr = AI.MakeTurn(game, 0);
                if (game.UpdateBoard(arr[0], arr[1], arr[2], arr[3]))
                    UpdateBoard(arr[0], arr[1], arr[2], arr[3]);
            }
        }

        // Initialy prints the board.
        private void PrintBoard()
        {
            this.piecesImg = new PictureBox[8, 8];

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++) {
                    this.piecesImg[i, j] = new PictureBox();
                    this.piecesImg[i, j].ClientSize = new Size(52, 52);
                    if (game.GetPiece(i, j) != null)
                        this.piecesImg[i, j].Image = ImageHandler.PlaceImage(game.GetPiece(i, j).GetColour(), game.GetPiece(i, j).IsDux(), false);
                    this.piecesImg[i, j].BackColor = Color.Transparent;
                    this.piecesImg[i, j].Location = new Point(j * 57, i * 57);
                    this.piecesImg[i, j].Click += new System.EventHandler(Piece_Click);
                    this.board.Controls.Add(this.piecesImg[i, j]);
                }
        }

        // Updates the board with moves, deaths, etc.
        public void UpdateBoard(int srcX, int srcY, int dstX, int dstY)
        {
            if(game.GetTurn() == 0)
                this.msgBox.Text = msg;
            this.piecesImg[srcX, srcY].Image = null;

            for (int i = -1; i < 2; i += 2) {
                try {
                    this.piecesImg[dstX + i, dstY].Image = game.GetPiece(dstX + i, dstY) == null ? null : ImageHandler.PlaceImage(game.GetPiece(dstX + i, dstY).GetColour(), game.GetPiece(dstX + i, dstY).IsDux(), false);
                } catch (System.IndexOutOfRangeException) { }

                try {
                    this.piecesImg[dstX, dstY + i].Image = game.GetPiece(dstX, dstY + i) == null ? null : ImageHandler.PlaceImage(game.GetPiece(dstX, dstY + i).GetColour(), game.GetPiece(dstX, dstY + i).IsDux(), false);
                } catch (System.IndexOutOfRangeException) { }
            }
            this.piecesImg[dstX, dstY].Image = ImageHandler.PlaceImage(game.GetPiece(dstX, dstY).GetColour(), game.GetPiece(dstX, dstY).IsDux(), false);
        }

        public static int MAXLEVEL = 0;

        // Restarts the game.
        private void RestartBoard()
        {
            game.Restart();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    this.piecesImg[i, j].Image = game.GetPiece(i, j) == null ? null : ImageHandler.PlaceImage(game.GetPiece(i, j).GetColour(), game.GetPiece(i, j).IsDux(), false);
        }

        private void ShowSettings()
        {
            this.aiCheck.Show();
            this.stColour.Show();
            this.whiteBullet.Show();
            this.blackBullet.Show();
        }

        private void HideSettings()
        {
            this.aiCheck.Hide();
            this.stColour.Hide();
            this.whiteBullet.Hide();
            this.blackBullet.Hide();
        }
    }
}

