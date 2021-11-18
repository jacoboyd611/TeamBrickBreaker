using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BrickBreaker
{
    public class Ball
    {
        public int x, y, xSpeed, ySpeed, size;
        public Color colour;

        public static Random rand = new Random();

        public Ball(int _x, int _y, int _xSpeed, int _ySpeed, int _ballSize)
        {
            x = _x;
            y = _y;
            xSpeed = _xSpeed;
            ySpeed = _ySpeed;
            size = _ballSize;

        }

        public void Move()
        {
            x = x + xSpeed;
            y = y + ySpeed;
        }

        public bool BlockCollision(Block b)
        {
            Rectangle blockRec = new Rectangle(b.x, b.y, b.width, b.height);
            Rectangle ballRec = new Rectangle(x, y, size, size);

            if (ballRec.IntersectsWith(blockRec) && y >= b.y + b.height - 5 || ballRec.IntersectsWith(blockRec) && y <= b.y + 5)
            {
                ySpeed *= -1;
            }
            else if (ballRec.IntersectsWith(blockRec) && x >= b.x + b.width - 5 || ballRec.IntersectsWith(blockRec) && x <= b.x + 5)
            {
                xSpeed *= -1;
            }

            return blockRec.IntersectsWith(ballRec);
        }

        public void PaddleCollision(Paddle p, bool left, bool right)
        {
            Rectangle ballRec = new Rectangle(x, y, size, size);
            Rectangle paddleRec = new Rectangle(p.x, p.y, p.width, p.height);

            if (ballRec.IntersectsWith(paddleRec) && ySpeed >= 0)
            {
                ySpeed *= -1;
                if (left) { xSpeed = -Math.Abs(xSpeed); }
                if (right) { xSpeed = Math.Abs(xSpeed); }             
            }
            else if (ballRec.IntersectsWith(paddleRec) && ySpeed <= 0)
            {
                var ballPosition = x - p.x;
                var hitPercent = (ballPosition / (p.width - size)) - .5;
                xSpeed = (int)(hitPercent * 10);
                ySpeed *= -1;
            }
        }

        public void WallCollision(UserControl UC)
        {
            // Collision with left wall
            if (x <= 0)
            {
                xSpeed *= -1;
            }
            // Collision with right wall
            if (x >= (UC.Width - size))
            {
                xSpeed *= -1;
            }
            // Collision with top wall
            if (y <= 2)
            {
                ySpeed *= -1;
            }
        }

        public bool BottomCollision(UserControl UC)
        {
            Boolean didCollide = false;

            if (y >= UC.Height)
            {
                didCollide = true;
            }

            return didCollide;
        }
    }
}
