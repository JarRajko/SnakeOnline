using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnline.Snake.Core.Models
{
    public class GameState
    {
        public List<Snake> Snakes { get; init; } = new List<Snake>();

        public Position FoodPosition { get; set; }

        public bool IsGameOver { get; set; }

        public int GridWidth { get; init; } = 30;
        public int GridHeight { get; init; } = 30;
    }
}
