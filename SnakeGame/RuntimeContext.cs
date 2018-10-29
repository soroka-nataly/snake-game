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
        public enum GameStatus { Run, Stop, Pause };
        private GameStatus CurrentStatus;

        public bool IsGameRun
        {
            get { return CurrentStatus == GameStatus.Run; }
            set
            {
                if (value)
                    CurrentStatus = GameStatus.Run;
                else
                    CurrentStatus = GameStatus.Pause;
            }
        }

        public bool IsGamePause
        {
            get { return CurrentStatus == GameStatus.Pause; }
        }

        public bool IsGameStop
        {
            get { return CurrentStatus == GameStatus.Stop; }
            set
            {
                if (value)
                    CurrentStatus = GameStatus.Stop;
                else
                    CurrentStatus = GameStatus.Run;
            }
        }

        public RuntimeContext()
        {
            CurrentStatus = GameStatus.Run;
        }
    }
}
