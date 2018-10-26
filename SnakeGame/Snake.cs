using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;


namespace SnakeGame
{
    enum snakeState { live, dead, ate }

    sealed class Snake : Drawable
    {
        public Color color = new Color(150, 200, 50);
        List<SnakePart> body = new List<SnakePart>();
        Field field;
        public Direction moveDirection;

        readonly static Vector2f snakeCellSize = new Vector2f(Game.FieldCellSize - 2, Game.FieldCellSize - 2);

        public SnakePart Head { get { return body.First(); } }

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
            //init in field
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

        public void Die()
        {
            this.ChangeColor(new Color(150, 150, 150));
        }

        public void ChangeDirection(Direction newDirection)
        {
            if (this.moveDirection == newDirection || Field.InvertDirection(this.moveDirection) == newDirection)
                return;

            this.moveDirection = newDirection;
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



        /*private Vector2f GetPositionOffset(Direction direction, bool inversion)
        {
            Vector2f offset = new Vector2f(0, 0); //для чего создавать новый???
            if (inversion)
                direction = InvertDirection(direction);

            switch (direction)
            {
                case Direction.Down:
                    offset = new Vector2f(0f, 16.0f);
                    break;
                case Direction.Left:
                    offset = new Vector2f(-16.0f, 0f);
                    break;
                case Direction.Up:
                    offset = new Vector2f(0f, -16.0f);
                    break;
                case Direction.Right:
                    offset = new Vector2f(16.0f, 0f);
                    break;
            }
            return offset;
        }   */

        public void Draw(RenderTarget window, RenderStates states)
        {
            foreach (SnakePart part in this.body)
            {
                window.Draw(part);
            }
        }
    }

    class SnakePart : RectangleShape, IFieldElement
    {
        public Vector2i Coordinate { get; set; }
        public bool IsPermeate => false;

        public SnakePart(Vector2f size)
        {
            this.Size = size;
        }
    }
}
