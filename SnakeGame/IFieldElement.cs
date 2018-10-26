using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;

namespace SnakeGame
{
    interface IFieldElement
    {
        bool IsPermeate { get; }
        

    }
}
