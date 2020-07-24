using Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    class Map
    {
        public int entitySize;
        public bool[,] map;

        public Map(int width, int height, int entitySize)
        {
            map = new bool[width / entitySize + 1, height / entitySize + 1];
            this.entitySize = entitySize;
        }

        public void AddBarier(Entity entity)
        {
            map[entity.Pos.X / entitySize, entity.Pos.Y / entitySize] = true;
        }

        public void RemoveBarier(Entity entity)
        {
            map[entity.Pos.X / entitySize, entity.Pos.Y / entitySize] = false;
        }
        public IEnumerable<Point> GetBarier(Entity entity)
        {
            Point pos = new Point(entity.Pos.X / entitySize, entity.Pos.Y / entitySize);
            for (int i = -1; i < 3; i++)
            {
                for (int j = -1; j < 3; j++)
                {
                    if (pos.X + i >= 0 && pos.X + i < map.GetLength(0) &&
                        pos.Y + j >= 0 && pos.Y + j < map.GetLength(1) &&
                        map[pos.X + i, pos.Y + j])
                        yield return new Point((pos.X + i) * entitySize, (pos.Y + j) * entitySize);
                }

            }
        }
        public IEnumerable<Point> GetBarier(Point pos)
        {
            pos = new Point(pos.X / entitySize, pos.Y / entitySize);
            for (int i = -1; i < 3; i++)
            {
                for (int j = -1; j < 3; j++)
                {
                    if (pos.X + i >= 0 && pos.X + i < map.GetLength(0) &&
                        pos.Y + j >= 0 && pos.Y + j < map.GetLength(1) &&
                        map[pos.X + i, pos.Y + j])
                        yield return new Point((pos.X + i) * entitySize, (pos.Y + j) * entitySize);
                }

            }
        }
    }
}
