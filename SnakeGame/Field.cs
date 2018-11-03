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
            CreateLevels();
            countLevels = Levels.Count;
        }

        public bool IsCellFree(int x, int y)
        {
            return (Data[x, y] == null);
        }

        public Vector2i GetSize()
        {
            return new Vector2i(Data.GetLength(0), Data.GetLength(1));
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
                                currentLevel[x, y] = new Wall(new Vector2i(x + 1, y + 1));
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
            var fieldLevel = level % countLevels;
            round = level / countLevels;
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

        public static Vector2f GetPositionFromCoordinate(Vector2i coordinate)
        {
            var x = (float)coordinate.X * Game.FieldCellSize;
            var y = (float)coordinate.Y * Game.FieldCellSize;

            var position = new Vector2f(x, y);
            return position;
        }

        public IFieldElement GetCellElement(int x, int y)
        {
            return Data[x, y];
        }

        public void UpdateCells(IEnumerable<IFieldElement> elements)
        {
            foreach (var elm in elements)
            {
                UpdateCell(elm);
            }
        }

        public void UpdateCell(IFieldElement element)
        {
            Data[element.Coordinate.X, element.Coordinate.Y] = element;
        }

        public void ClearCell(Vector2i coordinate)
        {
            Data[coordinate.X, coordinate.Y] = null;
        }

        public void Draw(RenderTarget window, RenderStates states)
        {
            foreach (var obj in Data)
            {
                var drawable = obj as Drawable;
                if (drawable != null)
                {
                    drawable.Draw(window, states);
                }
            }
        }
    }
}
