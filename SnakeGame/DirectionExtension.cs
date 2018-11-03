using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;

namespace SnakeGame
{
    public static class DirectionExtension
    {
        public static Vector2i GetVector(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Down:
                    return new Vector2i(0, 1);
                case Direction.Left:
                    return new Vector2i(-1, 0);
                case Direction.Up:
                    return new Vector2i(0, -1);
                case Direction.Right:
                    return new Vector2i(1, 0);
            }
            return new Vector2i(0, 0);
        }

        public static Vector2i GetInverseVector(this Direction direction)
        {
            return -1 * direction.GetVector();
        }

        public static Direction Invert(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Up:
                    return Direction.Down;
                case Direction.Right:
                    return Direction.Left;
            }
            return Direction.Up;
        }
    }
}
