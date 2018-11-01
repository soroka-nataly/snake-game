using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;

namespace SnakeGame
{
    class SnakePart : RectangleShape, IFieldElement
    {
        public Vector2i Coordinate { get; set; }
        public bool IsPermeate => false;

        public SnakePart(Vector2f size)
        {
            Size = size;
        }
    }
}
