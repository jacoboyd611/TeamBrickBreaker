using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreaker
{
    class PowerUp
    {
        public string type;
        public double x, y;

        public PowerUp(string _type, double _x, double _y)
        {
            type = _type;
            x = _x;
            y = _y;
        }
    }
}
