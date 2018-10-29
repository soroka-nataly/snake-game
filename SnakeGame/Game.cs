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
        public const Int32 WidthField = 30;
        public const Int32 HeightField = 30;
        private const Int32 HeightPanel = 60;
        private const Int32 StartLenth = 4;

        public static RuntimeContext context = new RuntimeContext();
        public GameStats gameStats;
        private RenderWindow mainWindow;
        private Snake snake;
        private Field field;
        private TextPanel panel;
        private TimeCounter timeCounter;
        private Direction SnakeDirection = Direction.Up;

        public void Run()
        {
            Init();

            while (mainWindow.IsOpen())
            {
                mainWindow.DispatchEvents();
                Draw();
                UpdateAll(timeCounter.getTimeElapsed(context.IsGameRun));
                mainWindow.Display();
            }
        }

        private void Init()
        {
            mainWindow = new RenderWindow(new VideoMode(WidthField * FieldCellSize, HeightField * FieldCellSize + HeightPanel), "Snake Game");
            mainWindow.Closed += MainWindow_Closed;
            mainWindow.Resized += OnResized;
            mainWindow.LostFocus += SetPauseOrStop;
            mainWindow.GainedFocus += OnAllEvents;
            mainWindow.KeyPressed += OnKeyPressed;
            mainWindow.KeyReleased += OnKeyReleased;
            

            InitGame();
        }

        private void Draw()
        {
            mainWindow.Clear();
            mainWindow.Draw(field);
            mainWindow.Draw(snake);
            mainWindow.Draw(panel);
            if (context.IsGamePause)
            {
                DrawPauseScreen();
            }
            if (context.IsGameStop)
            {
                DrawFinalScreen();
            }
        }

        private void DrawPauseScreen()
        {
            panel.DrawPausePanel(mainWindow);
        }

        private void DrawFinalScreen()
        {
            panel.DrawFinalPanel(mainWindow);
        }

        private void OnKeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.Up:
                case Keyboard.Key.W:
                    snake.NewDirection = Direction.Up;
                    break;
                case Keyboard.Key.Down:
                case Keyboard.Key.S:
                    snake.NewDirection = Direction.Down;
                    break;
                case Keyboard.Key.Left:
                case Keyboard.Key.A:
                    snake.NewDirection = Direction.Left;
                    break;
                case Keyboard.Key.Right:
                case Keyboard.Key.D:
                    snake.NewDirection = Direction.Right;
                    break;
                case Keyboard.Key.Return:
                    if (snake.IsDead)
                        InitGame();
                    break;
            }

        }

        private void OnKeyReleased(object sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.Space:
                    context.IsGameRun = !context.IsGameRun;
                    break;
            }
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
            context.IsGameRun = true;

            gameStats.OnUpdateSubscribe(UpdateSnakeSpeed);
        }

        private void UpdateSnakeSpeed ()
        {
            snake.UpdateSnakeSpeed(gameStats.speed);
        }

        private void UpdateAll(float dt)
        {
            snake.Update(dt);
        }

        private void OnAllEvents(object sender, EventArgs e)
        {
            mainWindow.KeyPressed += OnKeyPressed;
            mainWindow.KeyReleased += OnKeyReleased;
        }

        private void SetPauseOrStop(object sender, EventArgs e)
        {
            if (context.IsGameRun)
                context.IsGameRun = false;
            mainWindow.KeyPressed -= OnKeyPressed;
            mainWindow.KeyReleased -= OnKeyReleased;
        }

        private void OnResized(object sender, SizeEventArgs e)
        {
            var view = mainWindow.GetView();
            view.Size = new Vector2f(e.Width, e.Height);
            mainWindow.SetView(view);
        }

        private static void MainWindow_Closed(object sender, EventArgs e) => ((Window)sender).Close();

    }
}
