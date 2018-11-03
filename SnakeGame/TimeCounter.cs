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

        public float getTimeElapsed()
        {
            return (float)TimeSpan.FromTicks(getElapsedTicks()).TotalSeconds;
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
