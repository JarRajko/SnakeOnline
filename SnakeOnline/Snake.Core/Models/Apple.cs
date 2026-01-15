using SnakeOnline.Snake.Core.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnline.Snake.Core.Models
{
    public class Apple : Food
    {

        public Apple(Position pos) : base(pos)
        {

        }
        public override Color Color => Color.Red;

        public override int SpawnChance => 80;
        public override void ApplyEffect(Snake snake, GameState state)
        {
            snake.Grow();
        }
    }
}
