using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;

namespace SnakeGame
{
    internal class GameRules
    {
        private Field field;
        private Snake snake;

        public GameRules(Field field)
        {
            this.field = field;
        }


        public void SetSnake(Snake snake)
        {
            this.snake = snake;
            this.snake.OnEatFoodSubscribe(EatFood);
        }

        private void EatFood()
        {
            NewFood();
        }

        private void ClearAllFood()
        {
            var size = field.GetSize();
            for (int y = 0; y < size.Y; ++y)
            {
                for (int x = 0; x < size.X; ++x)
                {
                    var element = field.GetCellElement(x, y);
                    if (element is Food)
                    {
                        field.ClearCell(new Vector2i(x, y));
                    }
                }
            }
        }

        public void NewFood()
        {
            Random random = new Random();
            while (true)
            {
                var size = field.GetSize();
                var x = random.Next(1, size.X - 1);
                var y = random.Next(1, size.Y - 1);
                if (field.IsCellFree(x, y))
                {
                    field.UpdateCell(new Food(new Vector2i(x, y)));
                    return;
                }
            }
        }

        public void StartGame()
        {
            NewFood();
        }

        public void Win()
        {
            ClearAllFood();
        }
    }
}
