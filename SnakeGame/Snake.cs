using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;


namespace SnakeGame
{
    sealed class Snake : Drawable
    {
        enum SnakeState { Alive, Wait, Dead }
        readonly static Vector2f snakeCellSize = new Vector2f(Game.FieldCellSize - 2, Game.FieldCellSize - 2);

        public Color color = new Color(150, 200, 50);

        private List<SnakePart> body = new List<SnakePart>();
        private SnakeState state = SnakeState.Alive;
        private Field field;
        private Direction moveDirection;
        private Direction newDirection;
        private float timer = 0.0f;
        private float speed = 0.25f;
        private float startSpeed;
        private bool FoodWasEaten = false;
        private Action onEatFood = delegate { };
        private Action onLevelComplete = delegate { };

        public bool IsDead => state == SnakeState.Dead;
        public bool IsWait => state == SnakeState.Wait;
        //private SnakePart Head { get { return body.First(); } }

        public Direction NewDirection
        {
            get { return newDirection; }
            set
            {
                if (moveDirection != value && moveDirection.Invert() != value)
                {
                    newDirection = value;
                }
            }
        }

        public bool Waiting
        {
            get => state == SnakeState.Wait;
            set
            {
                if (value)
                    state = SnakeState.Wait;
                else
                    state = SnakeState.Alive;
            }
        }

        public Snake(int length, Vector2i startCoordinate, Direction moveDirection, Field field)
        {
            startSpeed = speed;
            this.field = field;
            this.moveDirection = moveDirection;
            //var partPosition = Field.GetPositionFromCoordinate(startCoordinate) + new Vector2f(2.0f, 2.0f);
            var partCoordinate = startCoordinate;
            for (int i = 0; i < length; i++)
            {
                var bodyPart = NewPart(partCoordinate, this.color);
                this.body.Add(bodyPart);
                partCoordinate = partCoordinate + moveDirection.GetInverseVector();

                //moveDirection.Next(partCoordinate);
            }
            field.UpdateCells(body);
            body.First().FillColor = Color.Red;

        }

        public void ChangeColor(Color color)
        {
            var tail = body.Skip(1);
            foreach (SnakePart part in tail)
            {
                part.FillColor = color;
            }
        }

        public void Update(float dt, bool IsGameRun)
        {

            if (IsGameRun)
            {
                timer += dt;
                while (timer > speed)
                {
                    if (IsWait)
                    {
                        onLevelComplete?.Invoke();
                    }
                    else if (!IsDead)
                    {
                        DoNextStep();
                    }
                    timer -= speed;
                }
            }
        }

        private void DoNextStep()
        {
            moveDirection = newDirection;
            Vector2i newHeadCoordinate = body.First().Coordinate + moveDirection.GetVector();
            var currentElement = field.GetCellElement(newHeadCoordinate.X, newHeadCoordinate.Y);
            if (currentElement == null || currentElement.IsPermeate)
            {
                body.First().FillColor = this.color;
                var newHead = NewPart(newHeadCoordinate, Color.Red);
                body.Insert(0, newHead);
                if (currentElement is Food)
                {
                    UpdateSnakeCells(body.First());
                    EatOneFood();
                }
                else
                {
                    UpdateSnakeCells(body.First(), body.Last());
                    body.Remove(body.Last());
                }
            }
            else
            {
                Die();
            }
        }

        private void UpdateSnakeCells(SnakePart newHead, SnakePart deleteTail = null)
        {
            field.UpdateCell(newHead);
            if (deleteTail != null)
            {
                field.ClearCell(deleteTail.Coordinate);
            }
        }

        internal void OnCompleteLevelSubscribe(Action onLevelComplete)
        {
            this.onLevelComplete += onLevelComplete;
        }

        public void Win()
        {
            state = SnakeState.Wait;
        }

        public void OnEatFoodSubscribe(Action onEatFood)
        {
            this.onEatFood += onEatFood;
        }

        private void EatOneFood()
        {
            onEatFood?.Invoke();
        }

        public void Die()
        {
            state = SnakeState.Dead;
            this.ChangeColor(new Color(150, 150, 150));
            Game.context.IsGameStop = true;
        }

        public void IncreaseSpeed()
        {
            //speed = startSpeed * (float)Math.Pow(0.85f, speedLevel - 1);
            speed *= 0.85f;
        }

        private SnakePart NewPart(Vector2i coordinate, Color color)
        {
            var partPosition = Field.GetPositionFromCoordinate(coordinate) + new Vector2f(1.0f, 1.0f);
            var bodyPart = new SnakePart(snakeCellSize)
            {
                FillColor = color,
                Position = partPosition,
                Coordinate = coordinate
            };
            return bodyPart;
        }

        public void Draw(RenderTarget window, RenderStates states)
        {
            foreach (SnakePart part in this.body)
            {
                part.Draw(window, states);
            }
        }
    }
}
