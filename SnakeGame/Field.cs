using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;

namespace SnakeGame
{
    public enum Element { empty, wall, food }

    class Field : Drawable
    {
        public IFieldElement[,] Data;
        List<Drawable> DrawbleField = new List<Drawable>();

        public Field(int width, int height)
        {
            Data = new IFieldElement[width, height];

            for (int i = 0; i <= width - 1; i++)
            {
                for (int j = 0; j <= height - 1; j++)
                {
                    var wall = new Wall(new Vector2i(i, j));
                    if (i == 0 || j == 0 || i == width - 1 || j == height - 1)
                        Data[i, j] = wall;
                    else
                        Data[i, j] = null;
                }
            }
        }

        public void NewFood()
        {
            Random random = new Random();
            while (true)
            {
                var x = random.Next(1, Data.GetLength(0) - 1);
                var y = random.Next(1, Data.GetLength(1) - 1);
                if (Data[x, y] == null)
                {
                    Data[x, y] = new Food(new Vector2i(x, y));
                    return;
                }
            }
        }

        public static Vector2f GetPositionFromCoordinate(Vector2i coordinate)
        {
            var x = (float)coordinate.X * Game.FieldCellSize;
            var y = (float)coordinate.Y * Game.FieldCellSize;

            var position = new Vector2f(x, y);
            return position;
        }

        public static Vector2i getCoordinateOffset(Direction direction, bool inversion)
        {
            Vector2i offset = new Vector2i(0, 0);
            if (inversion)
                direction = InvertDirection(direction);

            switch (direction)
            {
                case Direction.Down:
                    offset = new Vector2i(0, 1);
                    break;
                case Direction.Left:
                    offset = new Vector2i(-1, 0);
                    break;
                case Direction.Up:
                    offset = new Vector2i(0, -1);
                    break;
                case Direction.Right:
                    offset = new Vector2i(1, 0);
                    break;
            }
            return offset;
        }

        public static Direction InvertDirection(Direction direction)
        {
            var newDirection = Direction.Up;
            switch (direction)
            {
                case Direction.Down:
                    newDirection = Direction.Up;
                    break;
                case Direction.Left:
                    newDirection = Direction.Right;
                    break;
                case Direction.Up:
                    newDirection = Direction.Down;
                    break;
                case Direction.Right:
                    newDirection = Direction.Left;
                    break;
            }
            return newDirection;
        }

        public void Draw(RenderTarget window, RenderStates states)
        {
            foreach (var obj in Data)
            {
                var drawable = obj as Drawable;
                if (drawable != null)
                {
                    window.Draw(drawable);
                }
            }
        }

    }
}
