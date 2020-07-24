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
        public delegate void GameEvent(int score);
        public event GameEvent GameOver;

        private bool inGame = true;

        private Random random = new Random();

        private int width;

        private int height;

        private int entitySize;

        private int speed;

        private int score;

        private Map map;

        public Kolobok kolobok;

        public List<Tank> tanks;

        public List<Bullet> bullets;

        public List<Explosion> explosions;

        public List<Apple> apples;

        public List<Wall> walls;

        public List<Water> waters;

        public int Score => score;

        public Game() : this(600, 400, 20, 5, 5, 1)
        {
        }

        public Game(int width, int height, int entitySize, int tankCount, int appleCount, int speed)
        {
            this.width = width;
            this.height = height;
            this.entitySize = entitySize;
            this.speed = speed;

            kolobok = new Kolobok(new Point(entitySize, entitySize), speed);

            map = new Map(width, height, entitySize);

            walls = Generator.GenerateMap(width, height, entitySize, height * width / 10000);
            foreach (var wall in walls)
            {
                map.AddBarier(wall);
            }
            waters = Generator.GenerateWater(width, height, entitySize, ref walls);
            foreach (var water in waters)
            {
                map.AddBarier(water);
            }

            List<Entity> borders = walls.Cast<Entity>().ToList();
            borders.AddRange(waters);
            borders.Add(kolobok);
            tanks = Generator.GenerateEntity<Tank, Entity>(width, height, entitySize, tankCount, borders);
            foreach (Tank tank in tanks)
            {
                tank.ChangeDirection(RandomDirection(tank.Pos));
                tank.ChangeSpeed(speed);
            }

            apples = Generator.GenerateEntity<Apple, Entity>(width, height, entitySize, appleCount, borders);

            bullets = new List<Bullet>();
            explosions = new List<Explosion>();
        }

        public void Update(double dTime)
        {
            if (inGame)
            {
                for (int i = 0; i < bullets.Count; i++)
                {
                    Bullet bullet = bullets[i];

                    bullet.Update(dTime);

                    if (bullet.Creator is Kolobok)
                    {
                        for (int j = 0; j < tanks.Count; j++)
                        {
                            Tank tank = tanks[j];

                            if (CheckCollision(bullet.Pos, tank.Pos, entitySize / 2))
                            {
                                AddScore(10);

                                explosions.Add(new Explosion(tank.Pos));

                                tanks.Remove(tank);
                                bullets.Remove(bullet);

                                List<Entity> borders = walls.Cast<Entity>().ToList();
                                borders.AddRange(waters);
                                borders.AddRange(tanks);
                                borders.Add(kolobok);

                                List<Tank> newTanks = Generator.GenerateEntity<Tank, Entity>(width, height, entitySize, random.Next(1, 3), borders);
                                foreach (Tank newTank in newTanks)
                                {
                                    newTank.ChangeDirection(RandomDirection(newTank.Pos));
                                    newTank.ChangeSpeed(speed);
                                }
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
                            explosions.Add(new Explosion(kolobok.Pos));

                            inGame = false;

                            GameOver?.Invoke(Score);

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
                            explosions.Add(new Explosion(wall.Pos));

                            map.RemoveBarier(wall);

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

                kolobok.Update(dTime);

                foreach (Tank tank in tanks)
                    tank.Update(dTime);

                foreach (var wallPos in map.GetBarier(kolobok))
                {
                    if (CheckCollision(wallPos, kolobok.Pos, entitySize - 6))
                    {
                        Direction dir = kolobok.Dir;
                        switch (dir)
                        {
                            case Direction.Up:
                                kolobok.pos.Y = wallPos.Y + entitySize;
                                break;
                            case Direction.Down:
                                kolobok.pos.Y = wallPos.Y - entitySize;
                                break;
                            case Direction.Left:
                                kolobok.pos.X = wallPos.X + entitySize;
                                break;
                            case Direction.Right:
                                kolobok.pos.X = wallPos.X - entitySize;
                                break;
                            default:
                                break;
                        }
                        kolobok.ChangeDirection(InvertDirection(kolobok.Dir));
                        break;
                    }

                }

                for (int i = 0; i < apples.Count; i++)
                {
                    Apple apple = apples[i];

                    if (CheckCollision(apple.Pos, kolobok.Pos, entitySize - 4))
                    {
                        AddScore(100);

                        apples.Remove(apple);

                        List<Entity> borders = walls.Cast<Entity>().ToList();
                        borders.AddRange(waters);
                        borders.Add(kolobok);
                        apples.AddRange(Generator.GenerateEntity<Apple, Entity>(width, height, entitySize, 1, borders));
                    }

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
                    kolobok.ChangeDirection(InvertDirection(kolobok.Dir));
                }

                for (int i = 0; i < tanks.Count; i++)
                {
                    Tank tank = tanks[i];

                    bool hasCollision = false;

                    for (int j = 0; j < tanks.Count; j++)
                    {
                        if (i != j)
                        {
                            Tank tank2 = tanks[j];

                            if (CheckCollision(tank.Pos, tank2.Pos, entitySize - 2))
                            {
                                hasCollision = true;
                                tank.ChangeDirection(InvertDirection(tank.Dir));
                            }
                        }
                    }

                    if (CheckCollision(tank.Pos, kolobok.Pos, entitySize - 2))
                    {
                        // GG
                        explosions.Add(new Explosion(kolobok.Pos));

                        inGame = false;

                        GameOver?.Invoke(Score);
                    }

                    if (!hasCollision)
                    {
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

                for (int i = 0; i < explosions.Count; i++)
                {
                    explosions[i].Update(dTime);
                    if (explosions[i].Timer > 20)
                        explosions.RemoveAt(i);
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

            foreach (var wallPos in map.GetBarier(pos))
            {
                if (CheckCollision(posUp, wallPos, 2))
                    directions.Remove(Direction.Up);
                if (CheckCollision(posDown, wallPos, 2))
                    directions.Remove(Direction.Down);
                if (CheckCollision(posLeft, wallPos, 2))
                    directions.Remove(Direction.Left);
                if (CheckCollision(posRight, wallPos, 2))
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
        private void AddScore(int value)
        {
            score += value;
        }

        public List<Entity> AllEntity()
        {
            List<Entity> entities = new List<Entity>();
            entities.Add(kolobok);
            entities.AddRange(tanks);
            entities.AddRange(bullets);
            entities.AddRange(apples);
            entities.AddRange(walls);
            entities.AddRange(waters);
            return entities;
        }
    }
}
