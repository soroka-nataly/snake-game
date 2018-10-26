using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;
using System.IO;

namespace SnakeGame
{
    class TextPanel : Drawable
    {
        int height;
        int width;
        int top;
        GameStats gameStats;
        List<Text> textForDraw = new List<Text>();

        static readonly Font font = new Font(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "cour.ttf"));

        public TextPanel(int top, int height, int width, GameStats gameStats)
        {
            this.gameStats = gameStats;
            this.height = height;
            this.width = width;
            this.top = top;

            gameStats.OnUpdateSubscribe(UpdatePanelView);

            UpdatePanelView();
        }

        public void Draw(RenderTarget window, RenderStates states)
        {
            foreach (var text in textForDraw)
            {
                text.Draw(window, states);
            }
        }

        public void UpdatePanelView()
        {
            int numParam = 0;
            textForDraw.Clear();
            var stats = gameStats.GetGameStats();
            foreach (KeyValuePair<string, int> param in stats)
            {
                var text = CreateText(param.Key, param.Value, numParam, (stats.Count()));
                textForDraw.Add(text);
                numParam++;
            }
        }

        private Text CreateText(string textParam, int valueParam, int numParam, int countParams)
        {
            int lineHeight = height / ((countParams + 1)/ 2);
            var text = new Text(textParam + ": " + valueParam.ToString(), font)
            {
                CharacterSize = (uint)lineHeight - 2,
                Position = new Vector2f(numParam % 2 * (width / 2), (numParam / 2 * lineHeight) + top)
            };

            return text;
        }


    }
}
