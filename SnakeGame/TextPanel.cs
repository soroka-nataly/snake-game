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
        public static readonly Font font = new Font(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "cour.ttf")); 

        private int height;
        private int width;
        private int top;
        private GameStats gameStats;
        private List<Text> textForDraw = new List<Text>();
        private List<Drawable> pausePanel = new List<Drawable>();
        private List<Drawable> finalPanel = new List<Drawable>();

        public TextPanel(int top, int height, int width, GameStats gameStats)
        {
            this.gameStats = gameStats;
            this.height = height;
            this.width = width;
            this.top = top;

            gameStats.OnUpdateSubscribe(UpdatePanelView);

            UpdatePanelView();
            CreatePausePanel();
            CreateFinalPanel();
        }

        private void CreatePausePanel()
        {
            var overflow = new RectangleShape(new Vector2f(Game.WidthField * Game.FieldCellSize, Game.HeightField * Game.FieldCellSize))
            {
                FillColor = new Color(150, 150, 150, 150)
            };
            pausePanel.Add(overflow);

            var pauseText = new Text("Pause", font)
            {
                CharacterSize = 80,
                Color = Color.White,
                Style = Text.Styles.Bold
            };
            var pauseTextRect = pauseText.GetGlobalBounds();
            pauseText.Origin = new Vector2f(pauseTextRect.Width / 2, pauseText.CharacterSize / 2);
            pauseText.Position = new Vector2f((float)(Game.WidthField * Game.FieldCellSize / 2), (float)(Game.HeightField * Game.FieldCellSize / 2 - 30 ));
            pausePanel.Add(pauseText);            
        }

        private void CreateFinalPanel()
        {
            var overflow = new RectangleShape(new Vector2f(Game.WidthField * Game.FieldCellSize, Game.HeightField * Game.FieldCellSize))
            {
                FillColor = new Color(225, 150, 150, 150)
            };
            finalPanel.Add(overflow);

            var finalTextA = new Text("You died", font)
            {
                CharacterSize = 80,
                Color = Color.Red,
                Style = Text.Styles.Bold
            };
            finalTextA.Origin = new Vector2f(finalTextA.GetGlobalBounds().Width / 2, finalTextA.CharacterSize / 2);
            finalTextA.Position = new Vector2f((float)(Game.WidthField * Game.FieldCellSize / 2), (float)(Game.HeightField * Game.FieldCellSize / 2 - 80));
            finalPanel.Add(finalTextA);

            var finalTextB = new Text("Press Enter for new game", font)
            {
                CharacterSize = 30,
                Color = Color.White,
                Style = Text.Styles.Bold
            };
            finalTextB.Origin = new Vector2f(finalTextB.GetGlobalBounds().Width / 2, finalTextB.CharacterSize / 2);
            finalTextB.Position = new Vector2f((float)(Game.WidthField * Game.FieldCellSize / 2), (float)(Game.HeightField * Game.FieldCellSize / 2));
            finalPanel.Add(finalTextB);
        }

        public void Draw(RenderTarget window, RenderStates states)
        {
            foreach (var text in textForDraw)
            {
                text.Draw(window, states);
            }
        }

        public void DrawPausePanel (RenderTarget window)
        {
            foreach (var elem in pausePanel)
            {
                window.Draw(elem);
            }
        }

        public void DrawFinalPanel(RenderTarget window)
        {
            foreach (var elem in finalPanel)
            {
                window.Draw(elem);
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
