﻿/*  Created by: 
 *  Project: Brick Breaker
 *  Date: 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Xml;
using System.IO;

namespace BrickBreaker
{
    public partial class GameScreen : UserControl
    {

        public static int level = 1;
        public static int endValue = 0;
        Bitmap jellyFish = Properties.Resources.jellyfish;
        Brush black = new SolidBrush(Color.Black);
        Font drawFont = new Font("Arial", 15);
        Pen wumboPen = new Pen(Color.Yellow, 10);
        Pen krabbyPen = new Pen(Color.Red, 10);
        Random rand = new Random();

        Brush[] krabby = { new SolidBrush(Color.Red), new SolidBrush(Color.Orange) };
        float arcLength = -360;

        System.Windows.Media.MediaPlayer blipSound = new System.Windows.Media.MediaPlayer();
        
        bool krabbyPatty = false;
        int krabbyTime = 0;
        int totalKrabby = 0;
        int totalWumbo = 0;
        #region global values

        //player1 button control keys - DO NOT CHANGE
        public static Boolean leftArrowDown, rightArrowDown; 

        // Game values
        int lives;

        // Paddle and Ball objects
        Paddle paddle;
        Ball ball;

        // list of all blocks for current level
        List<Block> blocks = new List<Block>();

        // Brushes
        SolidBrush paddleBrush = new SolidBrush(Color.White);

        SolidBrush ballBrush = new SolidBrush(Color.Yellow);
        Pen borderPen = new Pen(Color.Black);

        //PowerUp list 
        List<PowerUp> powerUps = new List<PowerUp>();
        int powerUpSize = 20;

        //random
        Random rnd = new Random();

        public static List<Ball> balls = new List<Ball>();

        public static Color[] colour = new Color[] { Color.White, Color.Lavender, Color.DarkTurquoise, Color.PaleVioletRed, Color.PowderBlue, Color.MediumTurquoise, Color.Teal, Color.SkyBlue};

        System.Windows.Media.MediaPlayer backMedia = new System.Windows.Media.MediaPlayer();

        #endregion

        public GameScreen()
        {
            InitializeComponent();
            backMedia.Open(new Uri(Application.StartupPath + "/Resources/grassSkirt.wav"));
            backMedia.MediaEnded += new EventHandler(backMedia_MediaEnded);
            ReadXml();
            OnStart();
        }

        public void OnStart()
        {
            blipSound.Open(new Uri(Application.StartupPath + "/Resources/ping.wav"));
            backMedia.Play();
            balls.Clear();
            //set life counter
            lives = 3;
            //BackColor = colour[level];

            //set all button presses to false.
            leftArrowDown = rightArrowDown = false;

            // setup starting paddle values and create paddle object 

            int paddleWidth = 80;
            int paddleHeight = 20;
            int paddleX = ((this.Width / 2) - (paddleWidth / 2));
            int paddleY = (this.Height - paddleHeight) - 60;
            int paddleSpeed = 12;
            paddle = new Paddle(paddleX, paddleY, paddleWidth, paddleHeight, paddleSpeed, Color.White);
            paddle.wumbo = false;

            // setup starting ball values
            int ballX = this.Width / 2 - 10;
            int ballY = this.Height - paddle.height - 80;

            // Creates a new ball
            int xSpeed = 8;
            int ySpeed = 8;
            int ballSize = 20;
            ball = new Ball(ballX, ballY, xSpeed, ySpeed, ballSize);
            balls.Add(ball);

            #region Creates blocks for generic level. Need to replace with code that loads levels.          


            #endregion


            // start the game engine loop
            gameTimer.Enabled = true;
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //player 1 button presses
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
                default:
                    break;
            }
        }

        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            //player 1 button releases
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
                default:
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            // Move the paddle
            if (leftArrowDown && paddle.x > 0)
            {
                paddle.Move("left");
            }
            if (rightArrowDown && paddle.x < (this.Width - paddle.width))
            {
                paddle.Move("right");
            }
            if (leftArrowDown == false)
            {
                paddle.Move("noMove");
            }
            if (rightArrowDown == false)
            {
                paddle.Move("noMove");
            }

            // Move ball
            foreach (Ball b in balls)
            {
                b.Move();
                b.WallCollision(this);
                b.PaddleCollision(paddle, leftArrowDown, rightArrowDown);
            }

            #region Bottom Collision
            for (int i = 0; i < balls.Count(); i++)
            {
                if (balls[i].BottomCollision(this))
                {
                    // Moves the ball back to origin
                    if (balls.Count() > 1)
                    {
                        balls.RemoveAt(i);
                    }
                    else
                    {
                        balls[i].x = ((paddle.x - (ball.size / 2)) + (paddle.width / 2));
                        balls[i].y = (this.Height - paddle.height) - 85;
                        lives--;
                    }
                }
            }

            if (lives < 0)
            {
                gameTimer.Enabled = false;
                endValue = 2;
                OnEnd();
            }
            #endregion

            #region Block Collision
            foreach (Ball b in balls)
            {
                b.PaddleCollision(paddle, leftArrowDown, rightArrowDown);
            }
            // Check if ball has collided with any blocks
            foreach (Ball b in balls)
            {
                for (int i = 0; i < blocks.Count(); i++)
                {
                    if (b.BlockCollision(blocks[i], krabbyPatty))
                    {
                        blocks[i].hp--;
                        blipSound.Play();
                        //5% chance to make power up when block breaks
                        MakePowerUp(blocks[i].x, blocks[i].y);
                        if (blocks[i].hp == 0) { blocks.Remove(blocks[i]); }

                        if (blocks.Count == 0)
                        {
                            gameTimer.Enabled = false;
                            endValue = 1;
                            OnEnd();
                        }
                        break;
                    }
                }
            }
            #endregion

            // power up move
            foreach (PowerUp p in powerUps)
            {
                p.Move();
            }

            #region power up collision
            for (int i = 0; i < powerUps.Count(); i++)
            {
                if (powerUps[i].PaddleCollision(paddle))
                {
                    if (powerUps[i].type == "scatterShot")
                    {
                        int xSpeed = 8;
                        int ySpeed = 8;
                        int ballSize = 20;
                        Ball ball = new Ball(paddle.x, paddle.y, xSpeed, ySpeed, ballSize);
                        balls.Add(ball);
                    }

                    else if (powerUps[i].type == "wumbo" && paddle.wumbo == false)
                    {
                        paddle.width += 200;
                        paddle.x -= 100;
                        paddle.wumboTime = 300;
                        totalWumbo = 300;
                        paddle.wumbo = true;
                    }
                    else if (powerUps[i].type == "krabbyPatty" && krabbyPatty == false)
                    {
                        krabbyPatty = true;
                        krabbyTime = 120;
                        totalKrabby = krabbyTime;
                    }
                    powerUps.Remove(powerUps[i]);
                }
            }
            #endregion

            if (paddle.wumbo && paddle.wumboTime > 0)
            {
                paddle.wumboTime--;
            }
            else if (paddle.wumbo == true)
            {
                paddle.wumbo = false;
                paddle.width -= 200;
                paddle.x += 100;
                paddle.wumboTime = 300;
            }

            if (krabbyPatty && krabbyTime > 0)
            {
                krabbyTime--;
            }
            else if (krabbyPatty)
            {
                krabbyPatty = false;
            }
            Refresh();
        }

        public void OnEnd()
        {
            backMedia.Stop();
            // Goes to the game over screen
            Form form = this.FindForm();
            EndScreen ps = new EndScreen();

            ps.Location = new Point((form.Width - ps.Width) / 2, (form.Height - ps.Height) / 2);

            form.Controls.Add(ps);
            form.Controls.Remove(this);
        }

        public void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            // Draws paddle
            e.Graphics.FillRectangle(paddleBrush, paddle.x, paddle.y, paddle.width, paddle.height);
            e.Graphics.DrawRectangle(borderPen, paddle.x, paddle.y, paddle.width, paddle.height);

            // Draws blocks
            foreach (Block b in blocks)
            {
                e.Graphics.FillRectangle(b.brush, b.x, b.y, b.width, b.height);
                e.Graphics.DrawRectangle(borderPen, b.x, b.y, b.width, b.height);
                e.Graphics.DrawString($"{b.hp}", drawFont, black, b.x + b.width/2 - drawFont.Size/2, b.y+ b.height/2 - drawFont.Size);
            }

            // Draws ball
            foreach (Ball b in balls)
            {
                if (krabbyPatty) 
                {
                    int i = rand.Next(0, 2);
                    e.Graphics.FillEllipse(krabby[i], b.x, b.y, b.size, b.size); 
                }
                else { e.Graphics.FillEllipse(ballBrush, b.x, b.y, b.size, b.size); }
                e.Graphics.DrawEllipse(borderPen, b.x, b.y, b.size, b.size);
            }
            // Draw Power Ups
            foreach (PowerUp p in powerUps)
            {
                e.Graphics.FillRectangle(p.brush, p.x, p.y, powerUpSize, powerUpSize);
            }
            //draws lifes
            if (lives == 3) { e.Graphics.DrawImage(jellyFish, 152, 552, 51, 67); }
            if (lives >= 2) { e.Graphics.DrawImage(jellyFish, 84, 552, 51, 67); }
            if (lives >= 1) { e.Graphics.DrawImage(jellyFish, 12, 552, 51, 67); }

            if (paddle.wumbo) { e.Graphics.DrawArc(wumboPen, 20, 510, 30, 30, 360, arcLength * paddle.wumboTime / totalWumbo); }
            if (krabbyPatty) { e.Graphics.DrawArc(krabbyPen, 92, 510, 30, 30, 360, arcLength * krabbyTime / totalKrabby); }
        }

        public void MakePowerUp(float x, float y)
        {
            if (rnd.Next(1, 6) == 5)
            {
                PowerUp powerUp = new PowerUp(rnd.Next(0, 3), x, y);
                powerUps.Add(powerUp);
            }
        }

        private void ReadXml()
        {
            XmlReader reader = XmlReader.Create($"Levels/lvl{level}.xml");
            while (reader.Read())
            {
                Block b = new Block();
                reader.ReadToFollowing("brick");
                b.x = Convert.ToInt32(reader.GetAttribute("x"));
                b.y = Convert.ToInt32(reader.GetAttribute("y"));
                b.width = Convert.ToInt32(reader.GetAttribute("width"));
                b.height = Convert.ToInt32(reader.GetAttribute("height"));
                b.hp = Convert.ToInt32(reader.GetAttribute("value"));
                string colour = reader.GetAttribute("colour");

                if (colour != null)
                {
                    b.brush = new SolidBrush(Color.FromName(colour));
                    blocks.Add(b);
                }
            }
            reader.Close();
        }

        private void backMedia_MediaEnded(object sender, EventArgs e)
        {
            backMedia.Stop();
            backMedia.Play();
        }
    }
}