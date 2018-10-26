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
        public int speed;
        int level;
        int score;


        public GameStats()
        {
            foodCount = 0;
            level = 1;
            score = 0;
            speed = 700;
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
