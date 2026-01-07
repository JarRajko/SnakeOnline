using SnakeOnline.Snake.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnline.Input
{
    public class InputHandler
    {
        public Direction? GetDirection(Keys key)
        {
            if (key == Keys.W) return Direction.UP;
            else if (key == Keys.A) return Direction.LEFT;
            else if (key == Keys.S) return Direction.DOWN;
            else if (key == Keys.D) return Direction.RIGHT;
            return null;
        }
    }
}
