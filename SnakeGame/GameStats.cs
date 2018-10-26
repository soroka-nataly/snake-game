using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;

namespace SnakeGame
{
    class GameStats
    {
        public int foodCount;
        public float speed;
        int level;
        int score;

        private Action onUpdated = delegate { };


        public void OnUpdateSubscribe(Action onUpdated)
        {
            this.onUpdated += onUpdated;
        }


        public GameStats()
        {
            foodCount = 0;
            level = 1;
            score = 0;
            speed = 1.0f;
        }

        public void EatOneFood()
        {
            foodCount++;
            if (foodCount % 5 == 0)
            {
                level++;
                speed = speed - ((level - 1) * 50);
            }
            score += level;
            onUpdated?.Invoke();
        }

        public Dictionary<string, int> GetGameStats()
        {
            var stats = new Dictionary<string, int>
            {
                { "Food", foodCount },
                { "Level", level },
                { "Score", score },
                //{ "Speed", speed }


            };
            return stats;
        }
    }
}
