using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Wall : Entity
    {
        private Point pos;
        public Point Pos => pos;
        public Direction Dir => Direction.None;
        public Wall(int x, int y)
        {
            pos.X = x;
            pos.Y = y;
        }

        public void Update()
        {

        }
    }
}
