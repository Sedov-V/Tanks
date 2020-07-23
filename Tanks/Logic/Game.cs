using Entities;
using GameLogic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Game
    {
        private Random random = new Random();

        private int width;

        private int height;

        private int entitySize;

        public Kolobok kolobok;

        public List<Tank> tanks;

        public List<Bullet> bullets;

        public Apple apple;

        public List<Wall> walls;

        public Game() : this(600, 400, 20, 5)
        {
        }

        public Game(int width, int height, int entitySize, int tankCount)
        {
            this.width = width;
            this.height = height;
            this.entitySize = entitySize;

            kolobok = new Kolobok(new Point(entitySize, entitySize));
            walls = Generator.GenerateMap(width, height, entitySize, 10);
            tanks = Generator.GenerateTanks(width, height, entitySize, tankCount, walls);
            bullets = new List<Bullet>();

            foreach (Tank tank in tanks)
                tank.ChangeDirection(RandomDirection(tank.Pos));

            apple = Generator.GenerateApple(width, height, entitySize, walls);
        }

        public void Update()
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                Bullet bullet = bullets[i];

                bullet.Update();

                if (bullet.Creator.GetType() == typeof(Kolobok))
                {
                    for (int j = 0; j < tanks.Count; j++)
                    {
                        Tank tank = tanks[j];

                        if (CheckCollision(bullet.Pos, tank.Pos, entitySize / 2))
                        {
                            tanks.Remove(tank);
                            bullets.Remove(bullet);

                            List<Tank> newTanks = Generator.GenerateTanks(width, height, entitySize, random.Next(1, 3), walls);
                            foreach (Tank t in newTanks)
                                t.ChangeDirection(RandomDirection(t.Pos));
                            tanks.AddRange(newTanks);

                            break;
                        }
                    }
                }
                else
                {
                    if (CheckCollision(bullet.Pos, kolobok.Pos, entitySize / 2))
                    {
                        // GG
                        bullets.Remove(bullet);
                        break;
                    }
                }

                for (int j = 0; j < bullets.Count; j++)
                {
                    if (i != j)
                    {
                        Bullet bullet2 = bullets[j];

                        if (CheckCollision(bullet.Pos, bullet2.Pos, entitySize / 2))
                        {
                            bullets.Remove(bullet);
                            bullets.Remove(bullet2);
                            break;
                        }
                    }
                }

                for (int j = 0; j < walls.Count; j++)
                {
                    Wall wall = walls[j];

                    if (CheckCollision(bullet.Pos, wall.Pos, entitySize / 2))
                    {
                        walls.Remove(wall);
                        bullets.Remove(bullet);
                        break;
                    }
                }

                if (bullet.Pos.X < 0 ||
                    bullet.Pos.X > width - entitySize ||
                    bullet.Pos.Y < 0 ||
                    bullet.Pos.Y > height - entitySize)
                {
                    bullets.Remove(bullet);
                }
            }

            kolobok.Update();

            foreach (Tank tank in tanks)
                tank.Update();

            for (int i = 0; i < walls.Count; i++)
            {
                Wall wall = walls[i];

                if (CheckCollision(wall.Pos, kolobok.Pos, entitySize - 4))
                {
                    Direction dir = kolobok.Dir;
                    switch (dir)
                    {
                        case Direction.Up:
                            kolobok.pos.Y = wall.Pos.Y + entitySize;
                            break;
                        case Direction.Down:
                            kolobok.pos.Y = wall.Pos.Y - entitySize;
                            break;
                        case Direction.Left:
                            kolobok.pos.X = wall.Pos.X + entitySize;
                            break;
                        case Direction.Right:
                            kolobok.pos.X = wall.Pos.X - entitySize;
                            break;
                        default:
                            break;
                    }
                    break;
                }

            }

            if (CheckCollision(apple.Pos, kolobok.Pos, entitySize - 4))
            {
                apple = Generator.GenerateApple(width, height, entitySize, walls);
            }

            if (kolobok.Pos.X < 0 ||
                kolobok.Pos.X > width - entitySize ||
                kolobok.Pos.Y < 0 ||
                kolobok.Pos.Y > height - entitySize)
            {
                Direction dir = kolobok.Dir;
                switch (dir)
                {
                    case Direction.Up:
                        kolobok.pos.Y = 0;
                        break;
                    case Direction.Down:
                        kolobok.pos.Y = height - entitySize;
                        break;
                    case Direction.Left:
                        kolobok.pos.X = 0;
                        break;
                    case Direction.Right:
                        kolobok.pos.X = width - entitySize;
                        break;
                    default:
                        break;
                }
            }

            for (int i = 0; i < tanks.Count; i++)
            {
                Tank tank = tanks[i];

                for (int j = 0; j < tanks.Count; j++)
                {
                    if (i != j)
                    {
                        Tank tank2 = tanks[j];

                        if (CheckCollision(tank.Pos, tank2.Pos, entitySize - 1))
                        {
                            tank.ChangeDirection(InvertDirection(tank.Dir));
                        }
                    }
                }

                if (CheckCollision(tank.Pos, kolobok.Pos, entitySize - 2))
                {
                    //GG
                }

                List<Direction> directions = AllDirections(tank.Pos);
                directions.Remove(InvertDirection(tank.Dir));
                if (tank.Pos.X % 20 == 0 && tank.Pos.Y % 20 == 0)
                {
                    tank.ChangeDirection((directions.Count > 0) ? directions[random.Next(directions.Count)] : InvertDirection(tank.Dir));
                }

                if (random.Next(1000) < 3)
                {
                    bullets.Add(tank.Fire());
                }

                if (tank.Pos.X < 0 ||
                    tank.Pos.X > width - entitySize ||
                    tank.Pos.Y < 0 ||
                    tank.Pos.Y > height - entitySize)
                {
                    tank.ChangeDirection(InvertDirection(tank.Dir));
                }
            }

        }

        private Direction RandomDirection(Point pos)
        {

            List<Direction> directions = AllDirections(pos);
            return (directions.Count > 0) ? directions[random.Next(directions.Count)] : Direction.None;
        }

        private List<Direction> AllDirections(Point pos)
        {

            List<Direction> directions = new List<Direction>() { Direction.Up, Direction.Down, Direction.Left, Direction.Right };

            Point posUp = new Point(pos.X, pos.Y - entitySize);
            Point posDown = new Point(pos.X, pos.Y + entitySize);
            Point posLeft = new Point(pos.X - entitySize, pos.Y);
            Point posRight = new Point(pos.X + entitySize, pos.Y);

            foreach (Wall wall in walls)
            {
                if (CheckCollision(posUp, wall.Pos, 2))
                    directions.Remove(Direction.Up);
                if (CheckCollision(posDown, wall.Pos, 2))
                    directions.Remove(Direction.Down);
                if (CheckCollision(posLeft, wall.Pos, 2))
                    directions.Remove(Direction.Left);
                if (CheckCollision(posRight, wall.Pos, 2))
                    directions.Remove(Direction.Right);
            }

            return directions;
        }

        private bool CheckCollision(Point pos1, Point pos2, int size)
        {
            pos1 = new Point(pos1.X + entitySize / 2, pos1.Y + entitySize / 2);
            pos2 = new Point(pos2.X + entitySize / 2, pos2.Y + entitySize / 2);

            return (Math.Abs(pos1.X - pos2.X) <= (size - 0)) &&
                   (Math.Abs(pos1.Y - pos2.Y) <= (size - 0));
        }

        private Direction InvertDirection(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                default:
                    return Direction.None;
            }
        }
    }
}
