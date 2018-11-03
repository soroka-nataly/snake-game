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
        public const int WidthField = 29;
        public const int HeightField = 29;
        private const int HeightPanel = 60;
        private const int StartLenth = 4;

        public static RuntimeContext context = new RuntimeContext();
        public GameStats gameStats;
        private RenderWindow mainWindow;
        private GameRules rules;
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
                UpdateAll(timeCounter.getTimeElapsed());
                Draw();
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
            //mainWindow.Draw(snake);
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
                case Keyboard.Key.C:
                    snake.Waiting = !snake.Waiting;
                    break;
            }
        }

        private void InitGame()
        {
            gameStats = new GameStats(2, 1); //надо с этим что-то делать..
            panel = new TextPanel(HeightField * FieldCellSize, HeightPanel, WidthField * FieldCellSize, gameStats);
            field = new Field(WidthField, HeightField);
            rules = new GameRules(field);

            InitSnake(StartLenth);
            SnakeDirection = Direction.Up;

            timeCounter = new TimeCounter();
            context.IsGameRun = true;
        }

        private void InitSnake(int length)
        {
            snake = new Snake(length, new Vector2i(WidthField / 2, HeightField / 2), SnakeDirection, field);
            rules.SetSnake(snake);

            snake.OnEatFoodSubscribe(gameStats.EatOneFood);
            snake.OnCompleteLevelSubscribe(UpdateFieldLevel);
            gameStats.OnSpeedUpdateSubscription(snake.IncreaseSpeed);
            gameStats.OnLevelUpdateSubscription(snake.Win);
            gameStats.OnLevelUpdateSubscription(rules.Win);

            rules.StartGame();
        }

        private void UpdateFieldLevel()
        {
            var currentRound = field.Update(gameStats.level);
            InitSnake(StartLenth + currentRound);
            //else
            //    snake.IncreaseSpeed(gameStats.speedLevel);
        }

        private void UpdateAll(float dt)
        {
            snake.Update(dt, context.IsGameRun);
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
