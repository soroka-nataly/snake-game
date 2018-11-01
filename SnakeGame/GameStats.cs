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
        private int score;
        private Action onUpdated = delegate { };
        private Action onLevelUpdated = delegate { };
        private Action onSpeedUpdated = delegate { };

        //public float speed { get; set; }
        public int level { get; set; }
        public int speedLevel { get; set; }
        public int updateSpeedPeriod { get; set; }
        public int updateLevelPeriod { get; set; }

        public void OnUpdateSubscription(Action onUpdated)
        {
            this.onUpdated += onUpdated;
        }

        public void OnLevelUpdateSubscription(Action onUpdated)
        {
            this.onLevelUpdated += onUpdated;
        }

        public void OnSpeedUpdateSubscription(Action onUpdated)
        {
            this.onSpeedUpdated += onUpdated;
        }

        public GameStats(int updateSpeedPeriod, int updateLevelPeriod)
        {
            foodCount = 0;
            level = 1;
            score = 0;
            //speed = 0.6f;

            this.updateSpeedPeriod = updateSpeedPeriod;
            this.updateLevelPeriod = updateLevelPeriod;
        }

        public void EatOneFood()
        {
            foodCount++;
            if (foodCount % updateSpeedPeriod == 0)
            {
                speedLevel++;
                if (speedLevel % updateLevelPeriod == 0)
                {
                    level++;
                    onLevelUpdated?.Invoke();

                }
                onSpeedUpdated?.Invoke();
                //speed *= 0.8f;
            }
            score += level;
            onUpdated?.Invoke();
        }

        public Dictionary<string, float> GetGameStats()
        {
            var stats = new Dictionary<string, float>
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
