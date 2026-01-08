using SnakeOnline.Snake.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnline.Rendering
{
    public class GameRenderer
    {
        private const int TileSize = 20;

        public void Render(Graphics g, GameState state)
        {
            if (state == null) return;

            if (state.FoodPosition != null)
            {
                g.FillRectangle(Brushes.Red,
                    state.FoodPosition.X * TileSize,
                    state.FoodPosition.Y * TileSize,
                    TileSize - 1, TileSize - 1);
            }

            // 3. Vykreslenie všetkých hadov v hre
            foreach (var snake in state.Snakes)
            {
                // Ak je had mŕtvy, vykreslíme ho šedou farbou
                Brush snakeBrush = snake.IsAlive ? Brushes.Green : Brushes.Gray;

                foreach (var part in snake.Body)
                {
                    g.FillRectangle(snakeBrush,
                        part.X * TileSize,
                        part.Y * TileSize,
                        TileSize - 1, TileSize - 1);
                }
            }

        }
    }
}
