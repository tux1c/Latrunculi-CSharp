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
namespace Ludus_Latrunculorum
{
    using System.Windows.Forms;
    using System.Drawing;

    partial class MainMenu
    {
        private  Panel          board;
        private  Label          msgBox;
        private  Label          stColour;
        private  Button         gameButton;
        private  Button         settingsButton;
        private  PictureBox     exitButton;
        private  PictureBox     cpuImg;
        private  PictureBox[,]  piecesImg;
        private  CheckBox       aiCheck;
        private  RadioButton    whiteBullet;
        private  RadioButton    blackBullet; 

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            
            this.board                    = new Panel();
            this.msgBox                   = new Label();
            this.stColour                 = new Label();
            this.gameButton               = new Button();
            this.settingsButton           = new Button();
            this.cpuImg                   = new PictureBox();
            this.exitButton               = new PictureBox();
            this.aiCheck                  = new CheckBox();
            this.whiteBullet              = new RadioButton();
            this.blackBullet              = new RadioButton();

            // Sets background and title.
            this.BackgroundImage          = Properties.Resources.Background;
            this.BackgroundImageLayout    = ImageLayout.Stretch;
            this.DoubleBuffered           = true;
            this.ClientSize               = new Size(900, 600);
            this.MaximumSize              = new Size(900, 600);
            this.MinimumSize              = new Size(900, 600);
            this.Name                     = "MainMenu";
            this.Text                     = "Ludus Latrunculorum";
            this.BackColor                = Color.Tan;
            this.TransparencyKey          = Color.Tan;
            this.FormBorderStyle          = FormBorderStyle.None;

            // Board panel.
            this.board.Location           = new System.Drawing.Point(306, 69);
            this.board.Size               = new System.Drawing.Size(454, 454);
            this.board.Name               = "board";
            this.board.BackColor          = Color.Transparent;

            // Win/Lose messagebox.
            this.msgBox.Location          = new Point(120, 30);
            this.msgBox.Size              = new Size(191, 53);
            this.msgBox.Name              = "msgBox";
            this.msgBox.Text              = "";
            this.msgBox.Font              = new Font("Times New Roman", 32F);
            this.msgBox.BackColor         = Color.Transparent;
            this.msgBox.ForeColor         = Color.DarkViolet;
            this.msgBox.BorderStyle       = BorderStyle.None;

            // Settings messagebox.
            this.stColour.Location        = new Point(168, 244);
            this.stColour.Size            = new Size(40, 20);
            this.stColour.Name            = "stColour";
            this.stColour.Text            = "Colour:";
            this.stColour.BackColor       = Color.Transparent;
            this.stColour.BorderStyle     = BorderStyle.None;

            // gameButton (Play/Restart).
            this.gameButton.Location      = new Point(131, 100);
            this.gameButton.Size          = new Size(151, 53);
            this.gameButton.Name          = "gameButton";
            this.gameButton.Text          = "Play";
            this.gameButton.BackColor     = Color.Transparent;
            this.gameButton.Click        += new System.EventHandler(this.GameButton_Click);

            // Setting button.
            this.settingsButton.Location  = new Point(131, 160);
            this.settingsButton.Size      = new Size(151, 53);
            this.settingsButton.Name      = "settingsButton";
            this.settingsButton.Text      = "Settings";
            this.settingsButton.BackColor = Color.White;
            this.settingsButton.Click    += new System.EventHandler(this.SettingsButton_Click);

            // CPU Image
            this.cpuImg.Location          = new Point(515, 30);
            this.cpuImg.Size              = new Size(32, 32);
            this.cpuImg.Name              = "cpuImg";
            this.cpuImg.Image             = Properties.Resources.CPU;
            this.cpuImg.BackColor         = Color.Transparent;

            // Exit button.
            this.exitButton.Location      = new Point(775, 40);
            this.exitButton.Size          = new Size(15, 18);
            this.exitButton.Name          = "exitButton";
            this.exitButton.Image         = Properties.Resources.Close;
            this.exitButton.BackColor     = Color.Transparent;
            this.exitButton.Click        += new System.EventHandler(this.ExitButton_Click);

            // AI Checkbox.
            this.aiCheck.Location         = new Point(170, 220);
            this.aiCheck.Name             = "aiCheck";
            this.aiCheck.Text             = "Play vs A.I.";
            this.aiCheck.Checked          = true;
            this.aiCheck.BackColor        = Color.Transparent;

            // White bullet.
            this.whiteBullet.Location     = new Point(210, 240);
            this.whiteBullet.Name         = "whiteBullet";
            this.whiteBullet.Text         = "White";
            this.whiteBullet.Checked      = true;
            this.whiteBullet.BackColor    = Color.Transparent;

            // Black bullet.
            this.blackBullet.Location     = new Point(210, 260);
            this.blackBullet.Name         = "blackBullet";
            this.blackBullet.Text         = "Black";
            this.blackBullet.Checked      = false;
            this.blackBullet.BackColor    = Color.Transparent;

            // Hides settings.
            this.stColour.Hide();
            this.aiCheck.Hide();
            this.whiteBullet.Hide();
            this.blackBullet.Hide();

            // Hides CPU Image.
            this.cpuImg.Hide();

            // Adds everything.
            this.Controls.Add(this.board);
            this.Controls.Add(this.msgBox);
            this.Controls.Add(this.stColour);
            this.Controls.Add(this.gameButton);
            this.Controls.Add(this.settingsButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.cpuImg);
            this.Controls.Add(this.aiCheck);
            this.Controls.Add(this.whiteBullet);
            this.Controls.Add(this.blackBullet);

            this.ResumeLayout();
        }
    }
}

