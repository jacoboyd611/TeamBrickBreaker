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
    public partial class EndScreen : UserControl
    {
        System.Windows.Media.MediaPlayer backMedia = new System.Windows.Media.MediaPlayer();
        public EndScreen()
        {
            InitializeComponent();
            backMedia.Open(new Uri(Application.StartupPath + "/Resources/endScreen.wav"));
            backMedia.MediaEnded += new EventHandler(backMedia_MediaEnded);
            backMedia.Play();

            if (GameScreen.endValue == 1)
            {
                gameoverLabel.Text = "You won!";
            }
            else if (GameScreen.endValue == 2)
            {
                gameoverLabel.Text = "Game Over";
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            backMedia.Stop();
            MenuScreen ms = new MenuScreen();
            Form form = this.FindForm();

            form.Controls.Add(ms);
            form.Controls.Remove(this);

            ms.Location = new Point((form.Width - ms.Width) / 2, (form.Height - ms.Height) / 2);
        }

        private void backMedia_MediaEnded(object sender, EventArgs e)
        {
            backMedia.Stop();
            backMedia.Play();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            backMedia.Stop();
            if (GameScreen.level == 10) { GameScreen.level = 1; }
            else { GameScreen.level++; }
            GameScreen gs = new GameScreen();
            Form form = this.FindForm();

            form.Controls.Add(gs);
            form.Controls.Remove(this);

            gs.Location = new Point((form.Width - gs.Width) / 2, (form.Height - gs.Height) / 2);
        }

        private void retryButton_Click(object sender, EventArgs e)
        {
            backMedia.Stop();
            GameScreen gs = new GameScreen();
            Form form = this.FindForm();

            form.Controls.Add(gs);
            form.Controls.Remove(this);

            gs.Location = new Point((form.Width - gs.Width) / 2, (form.Height - gs.Height) / 2);
        }
    }
}
