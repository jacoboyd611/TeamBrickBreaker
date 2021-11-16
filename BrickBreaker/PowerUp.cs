using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BrickBreaker
{
    class PowerUp
    {
       
        public string type;
        public float x, y;
        public Color colour;

        public PowerUp(int rand, float _x, float _y)
        {

            string[] powerNames = new string[] { "scatterShot", "wumbo", "krabbyPatty" };
            string power = powerNames[rand];
            #region colourSelection
            Color powerColour;
            if (power == "scatterShot")
            {
                powerColour = Color.Blue;
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

            type = power;
            x = _x;
            y = _y;
            colour = powerColour;
        }

       public void Move()
        {
            y += 5;
        }

        public bool PaddleCollision(Paddle p)
        {
            
            Rectangle powerRec = new Rectangle(Convert.ToInt32(x), Convert.ToInt32(y), 20, 20);
            Rectangle paddleRec = new Rectangle(p.x, p.y, p.width, p.height);

            if (powerRec.IntersectsWith(paddleRec))
            {
                
                return true;
            }

            return false;
        }
    }
}
