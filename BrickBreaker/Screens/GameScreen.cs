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
        SolidBrush ballBrush = new SolidBrush(Color.White);
        SolidBrush blockBrush = new SolidBrush(Color.Red);
        SolidBrush powerUpBrush;

        //PowerUp list 
        List<PowerUp> powerUps = new List<PowerUp>();
        int powerUpSize = 20;
        

        //random
        Random rnd = new Random();


        #endregion

        public GameScreen()
        {
            InitializeComponent();
            ReadXml();
            OnStart();
        }


        public void OnStart()
        {
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
            ball.Move();

            // Check for collision with top and side walls
            ball.WallCollision(this);

            // Check for ball hitting bottom of screen
            if (ball.BottomCollision(this))
            {
                lives--;

                // Moves the ball back to origin
                ball.x = ((paddle.x - (ball.size / 2)) + (paddle.width / 2));
                ball.y = (this.Height - paddle.height) - 85;

                if (lives == 0)
                {
                    gameTimer.Enabled = false;
                    OnEnd();
                }
            }

            // Check for collision of ball with paddle, (incl. paddle movement)
            ball.PaddleCollision(paddle);

            // Check if ball has collided with any blocks
            foreach (Block b in blocks)
            {


                if (ball.BlockCollision(b))
                {

                    //5% chance to make power up when block breaks
                    MakePowerUp(b.x, b.y);

                    blocks.Remove(b);

                    if (blocks.Count == 0)
                    {
                        gameTimer.Enabled = false;
                        OnEnd();
                    }

                    break;
                }
            }
            // power up move
            foreach  (PowerUp p in powerUps)
            {
                p.Move();
            }

            //power up collision

            for(int i = 0; i < powerUps.Count(); i++)
            {
                if (powerUps[i].PaddleCollision(paddle))
                {
                    powerUps.Remove(powerUps[i]);
                }
            }

            //redraw the screen
            Refresh();
        }

        public void OnEnd()
        {
            // Goes to the game over screen
            Form form = this.FindForm();
            MenuScreen ps = new MenuScreen();
            
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
                e.Graphics.FillRectangle(blockBrush, b.x, b.y, b.width, b.height);
            }

            // Draws ball
            e.Graphics.FillRectangle(ballBrush, ball.x, ball.y, ball.size, ball.size);

            // Draw Power Ups
            foreach (PowerUp p in powerUps)
            {
                powerUpBrush = new SolidBrush(p.colour);

                e.Graphics.FillRectangle(powerUpBrush, p.x, p.y, powerUpSize, powerUpSize);
            }
        }

        public void MakePowerUp(float x, float y)
        {
            if (rnd.Next(1, 6) == 5)
            {
                string[] powerNames = new string[] { "scatterShot", "wumbo", "krabbyPatty" };
                string power = powerNames[rnd.Next(0, powerNames.Length)];
                #region colourSelection
                Color powerColour;
                if (power == "scatterShot")
                {
                     powerColour = Color.Tan;
                }
                else if (power == "wumbo")
                {
                    powerColour = Color.Gold;
                }
                else
                {
                    powerColour = Color.White;
                }
                #endregion

                PowerUp powerUp = new PowerUp(power, x, y, powerColour);
                powerUps.Add(powerUp);
            }
        }

        private void ReadXml()
        {
            XmlReader reader = XmlReader.Create($"Resources/test1.xml");
            while (reader.Read())
            {
                Block b = new Block();
                reader.ReadToFollowing("brick");
                b.x = Convert.ToInt32(reader.GetAttribute("x"));
                b.y = Convert.ToInt32(reader.GetAttribute("y"));
                b.width = Convert.ToInt32(reader.GetAttribute("width"));
                b.height = Convert.ToInt32(reader.GetAttribute("height"));
                blocks.Add(b);
            }
        }
    }
}
