using SnakeOnline.Snake.Core.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnline.Snake.Core.Models;
public class Plum : Food
{


    public override Color Color => Color.Blue;

    public override int SpawnChance => 50;

    public override void ApplyEffect(Snake snake, GameEngine engine)
    {
        // OCHRANA: Ak náhodou engine alebo snake neexistuje, metóda skončí a hra nespadne
        if (snake == null || engine == null) return;

        snake.Grow();
        engine.ApplySpeedBuff(5.0f, 80f); // Buff na 5 sekúnd
    }



}

