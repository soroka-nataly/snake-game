using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;
using SimpleJSON;
using System.IO;
using System.Reflection;

namespace SnakeGame
{
    class Field : Drawable
    {
        public IFieldElement[,] Data;
        //private List<Drawable> DrawbleField = new List<Drawable>();
        private Dictionary<int, IFieldElement[,]> Levels = new Dictionary<int, IFieldElement[,]>();
        private int countLevels;
        private int width;
        private int height;
        private int round = 0;

        public Field(int newWidth, int newHeight)
        {
            width = newWidth;
            height = newHeight;
            Data = new IFieldElement[width, height];

            for (int i = 0; i <= width - 1; i++)
            {
                for (int j = 0; j <= height - 1; j++)
                {
                    var wall = new Wall(new Vector2i(i, j));
                    if (i == 0 || j == 0 || i == width - 1 || j == height - 1)
                        Data[i, j] = wall;
                }
            }
            NewFood();
            CreateLevels();
            countLevels = Levels.Count;
        }

        private void CreateLevels()
        {
            string levelsString = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "levels.txt"));
            try
            {
                var levels = JSONNode.Parse(levelsString).AsArray;

                foreach (JSONNode level in levels)
                {
                    var id = level["id"].AsInt;
                    var currentLevel = new IFieldElement[width - 2, height - 2];
                    int y = 0;
                    foreach (JSONNode imageStr in level["level"].AsArray)
                    {
                        int x = 0;
                        foreach (var imagePxl in (string)imageStr)
                        {
                            if (imagePxl == '1')
                            {
                                currentLevel[x, y] = new Wall(new Vector2i(x, y));
                            }
                            x++;
                        }
                        y++;
                    }
                    Levels.Add(id, currentLevel);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public int Update(int level)
        {
            var fieldLevel = level - countLevels * round;
            if (fieldLevel - 1 == countLevels) round++;
            if (Levels.TryGetValue(fieldLevel, out IFieldElement[,] imageLevel))
            {
                Clear();
                for (int i = 0; i < width - 1; i++)
                {
                    for (int j = 0; j < height - 1; j++)
                    {
                        if (Data[i + 1, j + 1] == null && imageLevel[i, j] != null)
                            Data[i + 1, j + 1] = imageLevel[i, j];
                    }
                }
            }
            return round;
        }

        public void Clear()
        {
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
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

        public IFieldElement HasSomethingHere(int x, int y)
        {
            var element = Data[x, y];
            return element;
        }

        public void CreateSnake(List<SnakePart> snake)
        {
            foreach (SnakePart part in snake)
            {
                Data[part.Coordinate.X, part.Coordinate.Y] = part;
            }
        }

        public void UpdateSnake(SnakePart newHead, SnakePart deleteTail = null)
        {
            Data[newHead.Coordinate.X, newHead.Coordinate.Y] = newHead;
            if (deleteTail != null)
                Data[deleteTail.Coordinate.X, deleteTail.Coordinate.Y] = null;
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
