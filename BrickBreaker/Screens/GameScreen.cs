/*  Created by: 
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

namespace BrickBreaker
{
    public partial class GameScreen : UserControl
    {
        int level = 2;
        #region global values

        //player1 button control keys - DO NOT CHANGE
        Boolean leftArrowDown, rightArrowDown;

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
        Pen ballBorder = new Pen(Color.Black);
        SolidBrush blockBrush = new SolidBrush(Color.Red);
        SolidBrush powerUpBrush;


        //PowerUp list 
        List<PowerUp> powerUps = new List<PowerUp>();
        int powerUpSize = 20;


        //random
        Random rnd = new Random();

        public static List<Ball> balls = new List<Ball>();
        public static Image[] image = new Image[] { Properties.Resources.krustyKrab, Properties.Resources.spongebobHouse, Properties.Resources.squidwardsHouse, Properties.Resources.patricksHouse, Properties.Resources.jellyfishFields, Properties.Resources.gooLagoon, Properties.Resources.bikini_bottom, Properties.Resources.gloveWorld };


        #endregion

        public GameScreen()
        {
            InitializeComponent();
            ReadXml();
            OnStart();
        }

        public void OnStart()
        {
            balls.Clear();
            BackgroundImage = image[0];
            //set life counter
            lives = 3;

            //set all button presses to false.
            leftArrowDown = rightArrowDown = false;

            // setup starting paddle values and create paddle object
            int paddleWidth = 80;
            int paddleHeight = 20;
            int paddleX = ((this.Width / 2) - (paddleWidth / 2));
            int paddleY = (this.Height - paddleHeight) - 60;
            int paddleSpeed = 8;
            paddle = new Paddle(paddleX, paddleY, paddleWidth, paddleHeight, paddleSpeed, Color.White);

            // setup starting ball values
            int ballX = this.Width / 2 - 10;
            int ballY = this.Height - paddle.height - 80;

            // Creates a new ball
            int xSpeed = 6;
            int ySpeed = 6;
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

            // Move ball
            foreach (Ball b in balls)
            {
                b.Move();
            }

            // Check for collision with top and side walls
            foreach (Ball b in balls)
            {
                b.WallCollision(this);
            }

            // Check for ball hitting bottom of screen
            for (int i = 0; i < balls.Count(); i++)
            {
                if (balls[i].BottomCollision(this))
                {
                    // Moves the ball back to origin
                    if (balls.Count() == 1)
                    {
                        ball.x = ((paddle.x - (ball.size / 2)) + (paddle.width / 2));
                        ball.y = (this.Height - paddle.height) - 85;
                    }

                    if (balls.Count() == 1)
                    {
                        lives--;
                    } 
                    else
                    {
                        balls.Remove(balls[i]);
                    }

                    if (lives == 0)
                    {
                        gameTimer.Enabled = false;
                        OnEnd();
                    }
                }
            }

            // Check for collision of ball with paddle, (incl. paddle movement)
            foreach (Ball b in balls)
            {
                b.PaddleCollision(paddle, leftArrowDown, rightArrowDown);
            }
            // Check if ball has collided with any blocks
            for (int i = 0; i < blocks.Count(); i++)
            {
                foreach (Ball b in balls)
                {
                    if (b.BlockCollision(blocks[i]))
                    {
                        //5% chance to make power up when block breaks
                        MakePowerUp(blocks[i].x, blocks[i].y);

                        blocks.Remove(blocks[i]);

                        if (blocks.Count == 0)
                        {
                            gameTimer.Enabled = false;
                            OnEnd();
                        }
                        break;
                    }
                }
            }
            // power up move
            foreach (PowerUp p in powerUps)
            {
                p.Move();
            }

            //power up collision

            for (int i = 0; i < powerUps.Count(); i++)
            {
                if (powerUps[i].PaddleCollision(paddle))
                {
                    if (powerUps[i].type == "scatterShot")
                    {
                        int xSpeed = 6;
                        int ySpeed = 6;
                        int ballSize = 20;
                        Ball ball = new Ball(paddle.x, paddle.y, xSpeed, ySpeed, ballSize);
                        balls.Add(ball);

                    }
                    powerUps.Remove(powerUps[i]);
                }
            }
            Refresh();
        }

        public void OnEnd()
        {
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
            paddleBrush.Color = paddle.colour;
            e.Graphics.FillRectangle(paddleBrush, paddle.x, paddle.y, paddle.width, paddle.height);

            // Draws blocks
            foreach (Block b in blocks)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromName(b.colour)), b.x, b.y, b.width, b.height);
            }

            // Draws ball
            foreach (Ball b in balls)
            {
                e.Graphics.FillEllipse(ballBrush, b.x, b.y, b.size, b.size);
                e.Graphics.DrawEllipse(ballBorder, b.x, b.y, b.size, b.size);
            }
            // Draw Power Ups
            foreach (PowerUp p in powerUps)
            {
                powerUpBrush = new SolidBrush(p.colour);
                e.Graphics.FillRectangle(powerUpBrush, p.x, p.y, powerUpSize, powerUpSize);
            }
            //draws lifes
            if (lives == 3) { e.Graphics.DrawImage(Properties.Resources.jellyfish, 152, 552, 51, 67); }
            if (lives >= 2) { e.Graphics.DrawImage(Properties.Resources.jellyfish, 84, 552, 51, 67); }
            if (lives>=1) { e.Graphics.DrawImage(Properties.Resources.jellyfish, 12, 552, 51, 67); }
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
                b.colour = reader.GetAttribute("colour");
                if (b.colour != null) { blocks.Add(b); }
            }
            reader.Close();
        }
    }
}

