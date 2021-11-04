namespace BrickBreaker
{
    partial class GameScreen
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameScreen));
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.life1Box = new System.Windows.Forms.PictureBox();
            this.life3Box = new System.Windows.Forms.PictureBox();
            this.life2Box = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.life1Box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.life3Box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.life2Box)).BeginInit();
            this.SuspendLayout();
            // 
            // gameTimer
            // 
            this.gameTimer.Enabled = true;
            this.gameTimer.Interval = 1;
            this.gameTimer.Tick += new System.EventHandler(this.gameTimer_Tick);
            // 
            // life1Box
            // 
            this.life1Box.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("life1Box.BackgroundImage")));
            this.life1Box.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.life1Box.InitialImage = null;
            this.life1Box.Location = new System.Drawing.Point(12, 552);
            this.life1Box.Name = "life1Box";
            this.life1Box.Size = new System.Drawing.Size(51, 67);
            this.life1Box.TabIndex = 0;
            this.life1Box.TabStop = false;
            // 
            // life3Box
            // 
            this.life3Box.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("life3Box.BackgroundImage")));
            this.life3Box.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.life3Box.InitialImage = null;
            this.life3Box.Location = new System.Drawing.Point(153, 552);
            this.life3Box.Name = "life3Box";
            this.life3Box.Size = new System.Drawing.Size(51, 67);
            this.life3Box.TabIndex = 1;
            this.life3Box.TabStop = false;
            // 
            // life2Box
            // 
            this.life2Box.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("life2Box.BackgroundImage")));
            this.life2Box.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.life2Box.InitialImage = null;
            this.life2Box.Location = new System.Drawing.Point(84, 552);
            this.life2Box.Name = "life2Box";
            this.life2Box.Size = new System.Drawing.Size(51, 67);
            this.life2Box.TabIndex = 2;
            this.life2Box.TabStop = false;
            // 
            // GameScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Controls.Add(this.life2Box);
            this.Controls.Add(this.life3Box);
            this.Controls.Add(this.life1Box);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GameScreen";
            this.Size = new System.Drawing.Size(1100, 700);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GameScreen_Paint);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GameScreen_KeyUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.GameScreen_PreviewKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.life1Box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.life3Box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.life2Box)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.PictureBox life1Box;
        private System.Windows.Forms.PictureBox life3Box;
        private System.Windows.Forms.PictureBox life2Box;
    }
}
