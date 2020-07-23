using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Apple : Entity
    {
        private Point pos;
        public Point Pos => pos;

        public Apple(Point pos)
        {
            this.pos = pos;
        }
        public Direction Dir => Direction.None;

        public Apple(int x, int y)
        {
            pos.X = x;
            pos.Y = y;
        }

        public void Create()
        {

        }
        public void Update()
        {

        }

        public void OnCollision(Entity entity)
        {
            throw new NotImplementedException();
        }

    }
}
