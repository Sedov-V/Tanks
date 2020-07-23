using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Entities
{
    public class Bullet : Entity
    {
        public Point pos;

        private Direction dir;

        private int speed;

        private Entity creator;

        public Point Pos => pos;

        public Direction Dir => dir;

        public int Speed => speed;

        public Entity Creator => creator;

        public Bullet(Entity creator)
        {
            pos = creator.Pos;
            dir = creator.Dir;
            this.creator = creator;
            speed = 4;
        }

        public void Update()
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
                    pos.X += speed;
                    break;
            }
        }
        public void ChangeDirection(Direction dir)
        {
            this.dir = dir;
        }
    }
}
