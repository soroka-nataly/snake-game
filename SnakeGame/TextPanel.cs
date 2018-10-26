using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;
using System.IO;
using System.Text.RegularExpressions;

namespace SnakeGame
{
    class TextPanel : Drawable
    {
        uint height;
        uint width;
        uint top;
        GameStats gameStats;
        Dictionary<string, int> stats;
        List<Text> textForDraw = new List<Text>();

        Font font = new Font(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "cour.ttf"));

        public TextPanel(uint top, uint height, uint width, GameStats gameStats)
        {
            this.gameStats = gameStats;
            this.stats = this.gameStats.GetGameStats();
            this.height = height;
            this.width = width;
            this.top = top;

            UpdatePanelView();
        }

        public void Draw(RenderTarget window, RenderStates states)
        {
            foreach (var text in textForDraw)
            {
                window.Draw(text);
            }
        }

        public void UpdatePanelView()
        {
            stats = gameStats.GetGameStats();
            int numParam = 0;
            textForDraw.Clear();
            foreach (KeyValuePair<string, int> param in stats)
            {
                var text = CreateText(param.Key, param.Value, numParam);
                textForDraw.Add(text);
                numParam++;
            }
        }

        private Text CreateText(string textParam, int valueParam, int numParam)
        {
            uint lineHeight = height / (((uint)stats.Count() + 1)/ 2);
            var text = new Text(textParam + ": " + valueParam.ToString(), font)
            {
                CharacterSize = lineHeight - 2,
                Position = new Vector2f(numParam % 2 * (width / 2), (numParam / 2 * lineHeight) + top)
            };

            return text;
        }


    }
}
