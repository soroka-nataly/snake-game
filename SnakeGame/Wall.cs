﻿using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;

namespace SnakeGame
{
    class Wall : IFieldElement, Drawable
    {
        public bool IsPermeate => false;
        Color color = new Color(200, 200, 200);
        Drawable drawableElement;

        public Wall(Vector2i coordinate)
        {
            drawableElement = new RectangleShape(new Vector2f(Game.FieldCellSize, Game.FieldCellSize))
            {
                Position = Field.GetPositionFromCoordinate(coordinate),
                FillColor = color
            };
        }

        public void Draw(RenderTarget window, RenderStates states)
        {
            drawableElement.Draw(window, states);
        }

    }
}
