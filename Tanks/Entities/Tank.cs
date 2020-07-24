using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Entities
{
    public class Tank : Entity
    {
        private Point pos;
        private Direction dir;
        private int speed;

        public Point Pos => pos;
        public Direction Dir => dir;
        public int Speed => speed;

        public Tank()
        {
            this.pos = new Point(20, 20);
            this.speed = 1;
        }

        public Tank(Point pos, int speed)
        {
            this.pos = pos;
            this.speed = speed;
        }

        public Tank(int x, int y, int speed)
        {
            this.pos.X = x;
            this.pos.Y = y;
            this.speed = speed;
        }

        public void Update(double dTime)
        {
            switch (dir)
            {
                case Direction.Up:
                    pos.Y -= (int)(speed * dTime);
                    break;
                case Direction.Down:
                    pos.Y += (int)(speed * dTime);
                    break;
                case Direction.Left:
                    pos.X -= (int)(speed * dTime);
                    break;
                case Direction.Right:
                    pos.X += (int)(speed * dTime);
                    break;
                default:
                    break;
            }
        }

        public void ChangeDirection(Direction dir)
        {
            this.dir = dir;
        }

        public void ChangePosition(Point pos)
        {
            this.pos = pos;
        }

        public void ChangeSpeed(int speed)
        {
            this.speed = speed;
        }

        public Bullet Fire()
        {
            return new Bullet(this);
        }
    }
}
