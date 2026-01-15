using SnakeOnline.Snake.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnline.Snake.Core.Network
{
    [Serializable]
    public class GameStatePacket
    {
        public List<List<Position>> AllSnakesPositions { get; set; } = new List<List<Position>>();
        public List<Position> FoodPositions { get; set; } = new List<Position>();
        public int Score { get; set; }
        public bool IsGameOver { get; set; }
    }
}
