using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;
using System.Threading;

namespace SnakeGame
{
    sealed class Game
    {
        public const byte FieldCellSize = 16;
        const Int32 WidthField = 30;
        const Int32 HeightField = 30;
        const Int32 HeightPanel = 60;
        const Int32 StartLenth = 4;
        Direction SnakeDirection = Direction.Up;

        public static RuntimeContext context = new RuntimeContext();

        RenderWindow mainWindow;
        Snake snake;
        Field field;
        TextPanel panel;
        TimeCounter timeCounter;
        //Timer runGameTimer;
        //Int32 stepSpeed = 200;

        public GameStats gameStats;

        private void Draw()
        {
            mainWindow.Clear();
            mainWindow.Draw(field);
            mainWindow.Draw(snake);
            mainWindow.Draw(panel);
        }

        private void ProcessInput()
        {
            if (snake.IsDead)
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Return))
                {
                    InitGame();
                }
            }
            else
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
                {
                    snake.NewDirection = Direction.Up;
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
                {
                    snake.NewDirection = Direction.Down;
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
                {
                    snake.NewDirection = Direction.Left;
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
                {
                    snake.NewDirection = Direction.Right;
                }
            }
        }

        public void RunGame()
        {
            Init();            
            
            while (mainWindow.IsOpen())
            {
                mainWindow.DispatchEvents();
                ProcessInput();
                UpdateAll(timeCounter.getTimeElapsed());
                Draw();
                mainWindow.Display();
            }
        }

        private void Init()
        {
            mainWindow = new RenderWindow(new VideoMode(WidthField * FieldCellSize, HeightField * FieldCellSize + HeightPanel), "Snake Game");
            mainWindow.Closed += MainWindow_Closed;
            InitGame();
        }

        private void InitGame()
        {
            field = new Field(WidthField, HeightField);
            field.NewFood();
            SnakeDirection = Direction.Up;
            var startCoordinate = new Vector2i(WidthField / 2, HeightField / 2); //нужно ли ограничение на четность клеток поля???
            gameStats = new GameStats();
            snake = new Snake(StartLenth, startCoordinate, SnakeDirection, field);

            snake.OnEatFoodSubscribe(gameStats.EatOneFood);
            snake.OnEatFoodSubscribe(field.NewFood);

            panel = new TextPanel(HeightField * FieldCellSize, HeightPanel, WidthField * FieldCellSize, gameStats);

            timeCounter = new TimeCounter();
            //TimerCallback timerCb = new TimerCallback(SetUpdateEnabled);
            //this.runGameTimer = new Timer(timerCb, null, 0, stepSpeed); //избавиться от таймера
        }

        private void SetUpdateEnabled(object obj)
        {
            context.CurrentDisplayStatus = RuntimeContext.DisplayStatuses.updateDisplay;
        }

        private void UpdateAll(float dt)
        {
            snake.Update(dt);

        }

        private static void MainWindow_Closed(object sender, EventArgs e)
        {
            ((Window)sender).Close();
        }

    }
}
