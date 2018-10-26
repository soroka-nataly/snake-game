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
        Timer runGameTimer;
        Int32 stepSpeed = 200;

        public GameStats gameStats;

        public void RunGame()
        {
            mainWindow = new RenderWindow(new VideoMode(WidthField * FieldCellSize, HeightField * FieldCellSize + HeightPanel), "Snake Game");
            mainWindow.Closed += MainWindow_Closed;
            InitGame();

            
            
            while (mainWindow.IsOpen())
            {
                mainWindow.DispatchEvents();
                mainWindow.Clear();
                mainWindow.Draw(field);
                mainWindow.Draw(snake);
                mainWindow.Draw(panel);


                if (context.CurrentDisplayStatus == RuntimeContext.DisplayStatuses.stopGame)
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
                        SnakeDirection = Direction.Up;
                    }
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
                    {
                        SnakeDirection = Direction.Down;
                    }
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
                    {
                        SnakeDirection = Direction.Left;
                    }
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
                    {
                        SnakeDirection = Direction.Right;
                    }

                    if (context.CurrentDisplayStatus == RuntimeContext.DisplayStatuses.updateDisplay)
                    {
                        snake.ChangeDirection(SnakeDirection);
                        context.CurrentDisplayStatus = RuntimeContext.DisplayStatuses.pauseUpdating;
                        UpdateAll();
                    }
                }
                mainWindow.Display();

            }
        }

        private void InitGame()
        {
            field = new Field(WidthField, HeightField);
            field.NewFood();
            SnakeDirection = Direction.Up;
            var startCoordinate = new Vector2i(WidthField / 2, HeightField / 2); //нужно ли ограничение на четность клеток поля???
            snake = new Snake(StartLenth, startCoordinate, SnakeDirection, field);
            gameStats = new GameStats();
            panel = new TextPanel((uint)(HeightField * FieldCellSize), (uint)HeightPanel, (uint)(WidthField * FieldCellSize), gameStats);
            

            TimerCallback timerCb = new TimerCallback(SetUpdateEnabled);
            this.runGameTimer = new Timer(timerCb, null, 0, stepSpeed); //избавиться от таймера
        }

        private void SetUpdateEnabled(object obj)
        {
            context.CurrentDisplayStatus = RuntimeContext.DisplayStatuses.updateDisplay;
        }

        private void UpdateAll()
        {
            Vector2i newHeadCoordinate = snake.Head.Coordinate + Field.getCoordinateOffset(snake.moveDirection, false);
            if (field.Data[newHeadCoordinate.X, newHeadCoordinate.Y] == null || field.Data[newHeadCoordinate.X, newHeadCoordinate.Y].IsPermeate)
            {
                if ((field.Data[newHeadCoordinate.X, newHeadCoordinate.Y] is Food))
                {
                    gameStats.EatOneFood(); //надо ли как-то объединить???
                    this.stepSpeed = gameStats.speed;
                    panel.UpdatePanelView();
                    field.NewFood();
                    snake.DoNextStep(newHeadCoordinate, false);
                }
                else
                {
                    snake.DoNextStep(newHeadCoordinate, true);
                }
            }
            else
            {
                runGameTimer.Dispose();
                context.CurrentDisplayStatus = RuntimeContext.DisplayStatuses.stopGame;
                this.snake.Die();
            }

        }

        private static void MainWindow_Closed(object sender, EventArgs e)
        {
            ((Window)sender).Close();
        }

    }
}
