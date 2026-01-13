using System.Drawing;
using SnakeOnline.Snake.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace SnakeOnline.Rendering
{
    public class GameRenderer
    {
        private readonly int _tileSize;

        public GameRenderer(int tileSize)
        {
            _tileSize = tileSize;
        }

        public void Render(Graphics g, GameState state)
        {
            if (state == null) return;
            g.Clear(Color.Black);

            // 1. VYKRESLENIE MRIEŽKY (Prázdne políčka)
            // Použijeme pero s nižšou priehľadnosťou (napr. 50), aby čiara nebola príliš rušivá
            using (Pen gridPen = new Pen(Color.FromArgb(10, Color.White), 1))
            {
                for (int x = 0; x < state.GridWidth; x++)
                {
                    for (int y = 0; y < state.GridHeight; y++)
                    {
                        g.DrawRectangle(gridPen,
                            x * _tileSize,
                            y * _tileSize,
                            _tileSize,
                            _tileSize);
                    }
                }
            }

            // 2. Vykreslenie všetkých jedál
            foreach (var food in state.ActiveFoods)
            {
                using (Brush foodBrush = new SolidBrush(food.Color))
                {
                    g.FillRectangle(foodBrush,
                        food.Position.X * _tileSize + 1, // +1 aby sme neprekryli mriežku
                        food.Position.Y * _tileSize + 1,
                        _tileSize - 1,
                        _tileSize - 1);
                }
            }

            // 3. Vykreslenie všetkých hadov
            foreach (var snake in state.Snakes)
            {
                Brush snakeBrush = snake.IsAlive ? Brushes.LimeGreen : Brushes.Gray;

                foreach (var part in snake.Body)
                {
                    g.FillRectangle(snakeBrush,
                        part.X * _tileSize + 1,
                        part.Y * _tileSize + 1,
                        _tileSize - 1,
                        _tileSize - 1);
                }
            }
        }
    }
}