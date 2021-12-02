using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrickBreaker
{
    public partial class LevelSelect : UserControl
    {
        public LevelSelect()
        {
            InitializeComponent();
        }

        private void level1_Click(object sender, EventArgs e)
        {
            GameScreen.level = 1;
            OpenGameScreen();
        }

        private void level2_Click(object sender, EventArgs e)
        {
            GameScreen.level = 2;
            OpenGameScreen();
        }

        private void level3_Click(object sender, EventArgs e)
        {
            GameScreen.level = 3;
            OpenGameScreen();
        }

        private void level4_Click(object sender, EventArgs e)
        {
            GameScreen.level = 4;
            OpenGameScreen();
        }

        private void level5_Click(object sender, EventArgs e)
        {
            GameScreen.level = 5;
            OpenGameScreen();
        }

        private void level6_Click(object sender, EventArgs e)
        {
            GameScreen.level = 6;
            OpenGameScreen();
        }

        private void level7_Click(object sender, EventArgs e)
        {
            GameScreen.level = 7;
            OpenGameScreen();
        }

        private void level8_Click(object sender, EventArgs e)
        {
            GameScreen.level = 8;
            OpenGameScreen();
        }

        private void level9_Click(object sender, EventArgs e)
        {
            GameScreen.level = 9;
            OpenGameScreen();
        }

        private void level10_Click(object sender, EventArgs e)
        {
            GameScreen.level = 10;
            OpenGameScreen();
        }

        private void OpenGameScreen()
        {
            GameScreen gs = new GameScreen();
            Form form = this.FindForm();

            form.Controls.Add(gs);
            form.Controls.Remove(this);

            gs.Location = new Point((form.Width - gs.Width) / 2, (form.Height - gs.Height) / 2);
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            MenuScreen ms = new MenuScreen();
            Form form = this.FindForm();

            form.Controls.Add(ms);
            form.Controls.Remove(this);

            ms.Location = new Point((form.Width - ms.Width) / 2, (form.Height - ms.Height) / 2);
        }
    }
}
