using SnakeOnline.Snake.Core.Enum;
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
        public List<Models.Food> ActiveFoods { get; set; } = new List<Models.Food>();

        public bool IsGameOver { get; set; }

        public int Score { get; set; } = 0;
        public int GridWidth { get; init; } = 30;
        public int GridHeight { get; init; } = 30;
    }
}
