using Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GameLogic
{
    public static class Generator
    {
        static Random random = new Random();

        public static List<Wall> GenerateMap(int width, int height, int blockSize, int pathCount)
        {
            if ((width < 5) || (height < 5))
                new Exception("Incorrect map size");

            width = (int)Math.Ceiling((double)width / blockSize);
            height = (int)Math.Ceiling((double)height / blockSize);

            int[,] arr = new int[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    arr[i, j] = ((i == 1 && j > 0 && j < height - 1) ||
                                 (i == width - 2 && j > 0 && j < height - 1) ||
                                 (j == 1 && i > 0 && i < width - 1) ||
                                 (j == height - 2 && i > 0 && i < width - 1)) ? 0 : 1;
                }
            }

            while (pathCount > 0)
            {
                int x = random.Next(1, width - 1);
                int y = random.Next(1, height - 1);

                if (arr[x, y] == 0)
                {
                    List<Direction> directions = new List<Direction>();
                    if ((arr[x, y - 1] != 0) && (y > 1))
                        directions.Add(Direction.Up);
                    if ((arr[x, y + 1] != 0) && (y < height - 2))
                        directions.Add(Direction.Down);
                    if ((arr[x - 1, y] != 0) && (x > 1))
                        directions.Add(Direction.Left);
                    if ((arr[x + 1, y] != 0) && (x < width - 2))
                        directions.Add(Direction.Right);

                    if (directions.Count > 0)
                    {
                        Direction dir = directions[random.Next(directions.Count)];

                        if (dir == Direction.Up)
                        {
                            for (int i = y - 1; i > 1; i--)
                            {
                                if (arr[x, i] == 0)
                                    break;
                                arr[x, i] = 0;
                            }
                        }
                        if (dir == Direction.Right)
                        {
                            for (int i = x + 1; i < width - 1; i++)
                            {
                                if (arr[i, y] == 0)
                                    break;
                                arr[i, y] = 0;
                            }
                        }
                        if (dir == Direction.Down)
                        {
                            for (int i = y + 1; i < height - 1; i++)
                            {
                                if (arr[x, i] == 0)
                                    break;
                                arr[x, i] = 0;
                            }
                        }
                        if (dir == Direction.Left)
                        {
                            for (int i = x - 1; i > 1; i--)
                            {
                                if (arr[i, y] == 0)
                                    break;
                                arr[i, y] = 0;
                            }
                        }
                        pathCount--;
                    }
                }
            }

            List<Wall> walls = new List<Wall>();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (arr[i, j] == 1)
                        walls.Add(new Wall(i * blockSize, j * blockSize));
                }
            }

            return walls;
        }

        public static List<Water> GenerateWater(int width, int height, int blockSize, ref List<Wall> borders)
        {
            List<Water> water = new List<Water>();

            width = (int)Math.Ceiling((double)width / blockSize);
            height = (int)Math.Ceiling((double)height / blockSize);

            int[,] arr = new int[width, height];

            Point pos = new Point(random.Next(width / 4, width - width / 4), random.Next(height / 4, height - height / 4));

            int[] dir = { random.Next(-1, 2), random.Next(-1, 2) };

            if (dir[0] == 0) dir[0] = 1;
            if (dir[1] == 0) dir[1] = 1;

            while (pos.X > 1 && pos.X < width - 1 && pos.Y > 1 && pos.Y < height - 1)
            {
                arr[pos.X, pos.Y] = 1;

                arr[pos.X - 1, pos.Y] = 1;
                arr[pos.X + 1, pos.Y] = 1;
                arr[pos.X, pos.Y - 1] = 1;
                arr[pos.X, pos.Y + 1] = 1;

                pos.X += dir[0];
                pos.Y += dir[1];
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (arr[i, j] == 1)
                    {

                        for (int k = 0; k < borders.Count(); k++)
                        {
                            Wall wall = borders[k];
                            if (wall.Pos.X == i * blockSize && wall.Pos.Y == j * blockSize)
                            {
                                borders.RemoveAt(k);
                            }
                        }

                        water.Add(new Water(i * blockSize, j * blockSize));
                    }
                }
            }

            return water;
        }

        public static List<U> GenerateEntity<U, V>(int width, int height, int blockSize, int amount, List<V> borders)
        where U : Entity, new()
        where V : Entity
        {
            List<U> entities = new List<U>();

            List<Point> free = FreeSpace<V>(width, height, blockSize, borders);

            for (int i = 0; i < amount; i++)
            {
                int randomValue = random.Next(free.Count);
                U entity = new U();
                entity.ChangePosition(free[randomValue]);
                entities.Add(entity);
                free.RemoveAt(randomValue);
            }
            return entities;
        }

        private static List<Point> FreeSpace(int[,] walls, int blockSize)
        {
            List<Point> space = new List<Point>();
            for (int i = 0; i < walls.GetLength(0); i++)
            {
                for (int j = 0; j < walls.GetLength(1); j++)
                {
                    if (walls[i, j] == 0)
                        space.Add(new Point(i * blockSize, j * blockSize));
                }
            }
            return space;
        }

        private static List<Point> FreeSpace<T>(int width, int height, int blockSize, List<T> borders) where T : Entity
        {
            /*
            width = (int)Math.Ceiling((double)width / blockSize);
            height = (int)Math.Ceiling((double)height / blockSize);

            int[,] arr = new int[width, height];
            foreach (T border in borders)
            {
                arr[border.Pos.X / blockSize, border.Pos.Y / blockSize] = 1;
                if (border.Pos.X % blockSize != 0)
                {
                    arr[border.Pos.X / blockSize + 1, border.Pos.Y / blockSize] = 1;
                }
                if (border.Pos.Y % blockSize != 0)
                {
                    arr[border.Pos.X / blockSize, border.Pos.Y / blockSize + 1] = 1;
                }
            }
            */
            return FreeSpace(ToArray<T>(width, height, blockSize, borders), blockSize);
        }
        private static int[,] ToArray<T>(int width, int height, int blockSize, List<T> borders) where T : Entity
        {
            width = (int)Math.Ceiling((double)width / blockSize);
            height = (int)Math.Ceiling((double)height / blockSize);

            int[,] arr = new int[width, height];
            foreach (T border in borders)
            {
                arr[border.Pos.X / blockSize, border.Pos.Y / blockSize] = 1;
                if (border.Pos.X % blockSize != 0 && border.Pos.X / blockSize + 1 < width)
                {
                    arr[border.Pos.X / blockSize + 1, border.Pos.Y / blockSize] = 1;
                }
                if (border.Pos.Y % blockSize != 0 && border.Pos.Y / blockSize + 1 < height)
                {
                    arr[border.Pos.X / blockSize, border.Pos.Y / blockSize + 1] = 1;
                }
            }

            return arr;
        }
    }
}
