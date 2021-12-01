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
    }
}
