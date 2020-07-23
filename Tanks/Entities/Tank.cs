﻿using System;
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
        public Point pos;
        private Direction dir;
        private int speed;
        public Point Pos => pos;
        public Direction Dir => dir;
        public int Speed => speed;

        public Tank()
        {
            speed = 1;
        }
        public Tank(Point pos)
        {
            this.pos = pos;
            speed = 1;
        }
        public Tank(int x, int y)
        {
            pos.X = x;
            pos.Y = y;
            speed = 1;
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
                    break;
            }
        }
        public void ChangeDirection(Direction dir)
        {
            this.dir = dir;
        }

        public Bullet Fire()
        {
            return new Bullet(this);
        }
    }
}
