using SnakeOnline.Snake.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnline.Snake.Core.Network
{
    [Serializable]
    public class InputPacket
    {
        public int PlayerId { get; set; }
        public Direction ChosenDirection { get; set; }
    }
}
