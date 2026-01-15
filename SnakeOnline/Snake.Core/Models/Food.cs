using SnakeOnline.Snake.Core.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnline.Snake.Core.Models
{
    public abstract class Food
    {
        protected Food(Position pos)
        { 
            Position = pos;
        }

        public Position Position { get; set; }
        public abstract Color Color { get; }

        public abstract int SpawnChance { get; }

        public abstract void ApplyEffect(SnakeOnline.Snake.Core.Models.Snake snake, GameState state);
    }
}
