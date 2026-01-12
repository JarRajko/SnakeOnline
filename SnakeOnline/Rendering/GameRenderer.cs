using System.Drawing;
using SnakeOnline.Snake.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace SnakeOnline.Rendering
{
    public class GameRenderer
    {
        private const int TileSize = 20;

        public void Render(Graphics g, GameState state)
        {
            // 1. Základná ochrana a vyčistenie plochy
            if (state == null) return;
            g.Clear(Color.Black);

            // 2. Vykreslenie VŠETKÝCH jedál zo zoznamu
            // Starý blok s FoodPosition sme úplne vymazali
            foreach (var food in state.ActiveFoods)
            {
                // Každé jedlo si nesie informáciu o svojej farbe
                using (Brush foodBrush = new SolidBrush(food.Color))
                {
                    g.FillRectangle(foodBrush,
                        food.Position.X * TileSize,
                        food.Position.Y * TileSize,
                        TileSize - 1,
                        TileSize - 1);
                }
            }

            // 3. Vykreslenie všetkých hadov v hre
            foreach (var snake in state.Snakes)
            {
                // Ak je had mŕtvy, vykreslíme ho šedou farbou, inak zelenou
                Brush snakeBrush = snake.IsAlive ? Brushes.LimeGreen : Brushes.Gray;

                foreach (var part in snake.Body)
                {
                    g.FillRectangle(snakeBrush,
                        part.X * TileSize,
                        part.Y * TileSize,
                        TileSize - 1,
                        TileSize - 1);
                }
            }

            // Sem môžeš neskôr doplniť DrawUI(g, state); pre skóre
        }
    }
}