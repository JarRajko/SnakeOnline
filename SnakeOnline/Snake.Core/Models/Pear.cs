using SnakeOnline.Snake.Core.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnline.Snake.Core.Models
{
    public class Pear : Food
    {
        public Pear(Position pos) : base(pos)
        {
        }

        public override Color Color => Color.Yellow;

        public override int SpawnChance => 50;

        public override void ApplyEffect(SnakeOnline.Snake.Core.Models.Snake snake, GameState state)
        {
            if (snake == null || state == null) return;

            snake.Grow();
            state.CurrentSpeed = 250;
        }
    }
}
