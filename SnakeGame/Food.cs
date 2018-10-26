using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;

namespace SnakeGame
{
    class Food : IFieldElement, Drawable
    {
        public bool IsPermeate => true;
        Color color = new Color(255, 104, 0);
        Drawable drawableElement;

        public Food(Vector2i coordinate)
        {
            drawableElement = new CircleShape(Game.FieldCellSize / 2 - 1)
            {
                Position = Field.GetPositionFromCoordinate(coordinate),
                FillColor = color
            };
        }

        public void Draw(RenderTarget window, RenderStates states)
        {
            window.Draw(drawableElement);
        }
    }
}
