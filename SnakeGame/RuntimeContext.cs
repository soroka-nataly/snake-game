using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public enum Direction { Up, Right, Down, Left }
    class RuntimeContext
    {
        public enum DisplayStatuses { updateDisplay, stopGame, pauseUpdating};
        public DisplayStatuses CurrentDisplayStatus;

        public RuntimeContext()
        {
            CurrentDisplayStatus = DisplayStatuses.pauseUpdating;
        }
    }
}
