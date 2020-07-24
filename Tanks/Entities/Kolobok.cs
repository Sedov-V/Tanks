using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Kolobok : Entity
    {
        public bool canFire = true;

        public Point pos;
        private Direction dir;
        private int speed;

        public Point Pos => pos;
        public Direction Dir => dir;
        public int Speed => speed;

        public Kolobok()
        {
            pos = Point.Empty;
            dir = Direction.None;
            speed = 1;
        }

        public Kolobok(Point pos, int speed)
        {
            this.pos = pos;
            this.dir = Direction.Right;
            this.speed = speed;
        }

        public Kolobok(int x, int y, int speed)
        {
            this.pos.X = x;
            this.pos.Y = y;
            this.dir = Direction.Right;
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
