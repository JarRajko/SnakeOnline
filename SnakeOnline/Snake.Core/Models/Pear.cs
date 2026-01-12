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
        public override Color Color => Color.Yellow;

        public override int SpawnChance => 50;

        public override void ApplyEffect(Snake snake, GameEngine engine)
        {
            throw new NotImplementedException();
        }
    }
}
