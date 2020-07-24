using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Explosion : Entity
    {
        private int timer;

        private Point pos;
        public Point Pos => pos;
        public Direction Dir => Direction.None;
        public int Timer => timer;

        public Explosion()
        {
            this.pos = Point.Empty;
            timer = 0;
        }

        public Explosion(Point pos)
        {
            this.pos = pos;
            timer = 0;
        }

        public Explosion(int x, int y)
        {
            pos.X = x;
            pos.Y = y;
        }

        public void Update(double dTime)
        {
            timer++;
        }

        public void ChangeDirection(Direction dir) { }

        public void ChangePosition(Point pos)
        {
            this.pos = pos;
        }
    }
}
