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

        public override void ApplyEffect(SnakeOnline.Snake.Core.Models.Snake snake, GameEngine engine)
        {
            // Ochrana pred crashom
            if (snake == null || engine == null) return;

            // Efekt hrušky:
            snake.Grow(); // Had stále rastie po zjední

            // Spomalenie: nastavíme dlhší interval (napr. 250ms namiesto 150ms)
            // Hráč bude spomalený na 5 sekúnd
            engine.ApplySpeedBuff(5.0f, 250f);
        }
    }
}
