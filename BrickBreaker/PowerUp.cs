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

        public PowerUp(string _type, float _x, float _y, Color _colour)
        {
            type = _type;
            x = _x;
            y = _y;
            colour = _colour;
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
