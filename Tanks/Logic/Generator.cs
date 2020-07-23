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

        public static List<Tank> GenerateTanks(int width, int height, int blockSize, int tankCount, List<Wall> walls)
        {
            List<Tank> tanks = new List<Tank>();

            List<Point> free = freeSpace(width, height, blockSize, walls);

            for (int i = 0; i < tankCount; i++)
            {
                int randomValue = random.Next(free.Count);
                tanks.Add(new Tank(free[randomValue]));
                free.RemoveAt(randomValue);
            }
            return tanks;
        }

        public static Apple GenerateApple(int width, int height, int blockSize, List<Wall> walls)
        {
            List<Point> free = freeSpace(width, height, blockSize, walls);

            return new Apple(free[random.Next(free.Count)]);
        }

        private static List<Point> freeSpace(int[,] walls, int blockSize)
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

        private static List<Point> freeSpace(int width, int height, int blockSize, List<Wall> walls)
        {
            width = (int)Math.Ceiling((double)width / blockSize);
            height = (int)Math.Ceiling((double)height / blockSize);

            int[,] arr = new int[width, height];
            foreach (Wall wall in walls)
                arr[wall.Pos.X / blockSize, wall.Pos.Y / blockSize] = 1;

            return freeSpace(arr, blockSize);
        }
    }
}
