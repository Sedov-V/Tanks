using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GameLogic
{
    public static class MapGenerator
    {
        public static Entity[,] Generate(int width, int height, int pathCount)
        {
            if ((width < 5) || (height < 5))
                new Exception("Incorrect map size");

            Entity[,] arr = new Entity[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    arr[i, j] = ((i == 1 && j > 0 && j < height - 1) ||
                                 (i == width - 2 && j > 0 && j < height - 1) ||
                                 (j == 1 && i > 0 && i < width - 1) ||
                                 (j == height - 2 && i > 0 && i < width - 1)) ? null : new Wall();
                }
            }

            while (pathCount > 0)
            {
                int x = (new Random()).Next(1, width - 1);
                int y = (new Random()).Next(1, height - 1);

                if (arr[x, y] == null)
                {
                    int dir = (new Random()).Next(4);
                    if (dir == 0)
                    {
                        if ((arr[x, y - 1] == null) || (y == 1))
                        {
                            dir++;
                        }
                        for (int i = y - 1; i > 1; i--)
                        {
                            if (arr[x, i] == null)
                                break;
                            arr[x, i] = null;
                        }
                    }
                    if (dir == 1)
                    {
                        if ((arr[x + 1, y] == null) || (x == width - 1))
                        {
                            dir++;
                        }
                        for (int i = x + 1; i < width - 1; i++)
                        {
                            if (arr[i, y] == null)
                                break;
                            arr[i, y] = null;
                        }
                    }
                    if (dir == 2)
                    {
                        if ((arr[x, y + 1] == null) || (y == height - 1))
                        {
                            dir++;
                        }
                        for (int i = y + 1; i < height - 1; i++)
                        {
                            if (arr[x, i] == null)
                                break;
                            arr[x, i] = null;
                        }
                    }
                    if (dir == 3)
                    {
                        if ((arr[x - 1, y] == null) || (x == 1))
                        {
                            pathCount++;
                        }
                        for (int i = x - 1; i > 1; i--)
                        {
                            if (arr[i, y] == null)
                                break;
                            arr[i, y] = null;
                        }
                    }
                    pathCount--;
                }
            }

            return arr;
        }
    }
}
