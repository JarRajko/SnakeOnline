using SnakeOnline.Snake.Core.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnline.Snake.Core.Models;
public class Plum : Food
{

    public Plum(Position pos) : base(pos)
    {
    }
    public override Color Color => Color.Blue;

    public override int SpawnChance => 50;

   public override void ApplyEffect(Snake snake, GameState state)
    {
        state.CurrentSpeed = 80;
        snake.Grow();
    }



}

