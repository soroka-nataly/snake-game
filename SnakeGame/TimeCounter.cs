using System;

namespace SnakeGame
{
    class TimeCounter
    {
        private long lastTicks;

        public TimeCounter()
        {
            lastTicks = DateTime.Now.Ticks;
        }

        public float getTimeElapsed(bool IsGameRun)
        {
            if (IsGameRun)
                return (float)TimeSpan.FromTicks(getElapsedTicks()).TotalSeconds;
            else
                getElapsedTicks();
                return 0.0f;
        }

        private long getElapsedTicks()
        {
            var ticks = DateTime.Now.Ticks;
            var esclaped = ticks - lastTicks;
            lastTicks = ticks;
            return esclaped;
        }
    }
}
