using Entities;
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Controller
{
    public static class KolobokController
    {
        private static List<Direction> directions = new List<Direction>() { Direction.Up, Direction.Down, Direction.Left, Direction.Right, Direction.None };
        public static void KeyDown(Game game, Keys key)
        {
            Direction dir;
            if ((dir = DirectionFromKey(key)) != Direction.None)
            {
                directions.Remove(dir);
                directions.Add(dir);
            }
            game.kolobok.ChangeDirection(directions.Last());

            if (key == Keys.Space)
            {
                if (game.kolobok.canFire)
                    game.bullets.Add(game.kolobok.Fire());
                game.kolobok.canFire = false;
            }
        }

        public static void KeyUp(Game game, Keys key)
        {
            Direction dir;
            if ((dir = DirectionFromKey(key)) != Direction.None)
            {
                directions.Remove(dir);
                directions.Insert(0, dir);
            }
            game.kolobok.ChangeDirection(directions.Last());

            if (key == Keys.Space)
                game.kolobok.canFire = true;
        }

        private static Direction DirectionFromKey(Keys key)
        {
            switch (key)
            {
                case Keys.Left:
                case Keys.A:
                    return Direction.Left;
                case Keys.Up:
                case Keys.W:
                    return Direction.Up;
                case Keys.Right:
                case Keys.D:
                    return Direction.Right;
                case Keys.Down:
                case Keys.S:
                    return Direction.Down;
                default:
                    return Direction.None;
            }
        }
    }
}
