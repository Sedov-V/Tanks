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

        public Wall()
        {
            this.pos = Point.Empty;
        }

        public Wall(Point pos)
        {
            this.pos = pos;
        }

        public Wall(int x, int y)
        {
            pos.X = x;
            pos.Y = y;
        }

        public void Update(double dTime) { }

        public void ChangeDirection(Direction dir) { }

        public void ChangePosition(Point pos)
        {
            this.pos = pos;
        }
    }
}
