using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;


namespace SnakeGame
{
    enum SnakeState { Alive, Dead }

    sealed class Snake : Drawable
    {
        readonly static Vector2f snakeCellSize = new Vector2f(Game.FieldCellSize - 2, Game.FieldCellSize - 2);

        public Color color = new Color(150, 200, 50);
        public bool IsDead => snakeState == SnakeState.Dead;
        private List<SnakePart> body = new List<SnakePart>();
        private SnakeState snakeState = SnakeState.Alive;
        private Field field;
        private Direction moveDirection;
        private Direction newDirection;
        private SnakePart Head { get { return body.First(); } }
        private float timer = 0.0f;
        private float tickTime = 0.6f;
        private Action onEatFood = delegate { };

        public Direction NewDirection
        {
            get
            {
                return newDirection;
            }
            set
            {
                if (moveDirection != value && Field.InvertDirection(moveDirection) != value)
                {
                    newDirection = value;
                }
            }

        }
        
        public Snake(byte length, Vector2i startCoordinate, Direction moveDirection, Field field)
        {
            this.field = field;
            this.moveDirection = moveDirection;
            var partPosition = Field.GetPositionFromCoordinate(startCoordinate) + new Vector2f(1.0f, 1.0f);
            var partCoordinate = startCoordinate;
            for (int i = 0; i < length; i++)
            {
                var bodyPart = NewPart(partCoordinate, this.color);
                this.body.Add(bodyPart);
                partCoordinate = partCoordinate + Field.getCoordinateOffset(moveDirection, true);
            }
            foreach (SnakePart part in body)
            {
                field.Data[part.Coordinate.X, part.Coordinate.Y] = part;
            }
            body.First().FillColor = Color.Red;
            
        }

        public void ChangeColor(Color color)
        {
            foreach (SnakePart part in this.body)
            {
                part.FillColor = color;
            }
            body[0].FillColor = Color.Red;
        }

        public void Update(float dt)
        {
            timer += dt;
            while (timer > tickTime)
            {
                if (!IsDead)
                {
                     NextStep();
                }
                timer -= tickTime;
            }

        }

        public void DoNextStep(Vector2i newHeadCoordinate, bool hungry)
        {
            if (hungry)
            {
                field.Data[body.Last().Coordinate.X, body.Last().Coordinate.Y] = null;
                body.Remove(body.Last());
            }
            body.First().FillColor = this.color;
            var newHead = NewPart(newHeadCoordinate, Color.Red);
            body.Insert(0, newHead);
            field.Data[newHead.Coordinate.X, newHead.Coordinate.Y] = newHead;
        }

        private void NextStep()
        {
            moveDirection = newDirection;
            Vector2i newHeadCoordinate = Head.Coordinate + Field.getCoordinateOffset(moveDirection, false);
            if (field.Data[newHeadCoordinate.X, newHeadCoordinate.Y] == null || field.Data[newHeadCoordinate.X, newHeadCoordinate.Y].IsPermeate)
            {
                if ((field.Data[newHeadCoordinate.X, newHeadCoordinate.Y] is Food))
                {
                    EatOneFood();
                    DoNextStep(newHeadCoordinate, false);
                }
                else
                {
                    DoNextStep(newHeadCoordinate, true);
                }
            }
            else
            {
                Die();
            }
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
            snakeState = SnakeState.Dead;
            this.ChangeColor(new Color(150, 150, 150));
            Game.context.IsGameStop = true;
        }

        public void UpdateSnakeSpeed(float newSpeed)
        {
            tickTime = newSpeed;
        }

        private SnakePart NewPart(Vector2i coordinate, Color color)
        {
            var partPosition = Field.GetPositionFromCoordinate(coordinate);
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
