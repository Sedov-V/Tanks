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
        private bool canMove = false;
        public bool canFire = true;

        public Point pos;

        private Direction dir;

        private int speed;
        public Point Pos => pos;
        public Direction Dir => dir;
        public int Speed => speed;

        public Kolobok()
        {
            speed = 1;
            pos = Point.Empty;
            dir = Direction.None;
        }

        public Kolobok(Point pos)
        {
            speed = 1;
            this.pos = pos;
            dir = Direction.None;
        }

        public void Update()
        {
            if (canMove)
            {
                switch (dir)
                {
                    case Direction.Up:
                        pos.Y -= speed;
                        break;
                    case Direction.Down:
                        pos.Y += speed;
                        break;
                    case Direction.Left:
                        pos.X -= speed;
                        break;
                    case Direction.Right:
                        pos.X += speed;
                        break;
                    default:
                        break;
                }
            }
        }

        public void ChangeDirection(Direction dir)
        {
            if (canMove = dir != Direction.None)
            {
                this.dir = dir;
            }
        }
        public Bullet Fire()
        {
            return new Bullet(this);
        }
    }
}
