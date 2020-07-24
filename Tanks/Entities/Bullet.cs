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
        private Entity creator;

        public Point pos;
        private Direction dir;
        private int speed;

        public Entity Creator => creator;

        public Point Pos => pos;
        public Direction Dir => dir;
        public int Speed => speed;

        public Bullet(Entity creator)
        {
            this.pos = creator.Pos;
            this.dir = creator.Dir;
            this.creator = creator;
            if (creator is Kolobok)
                this.speed = ((Kolobok)creator).Speed * 4;
            if (creator is Tank)
                this.speed = ((Tank)creator).Speed * 4;
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

    }
}
