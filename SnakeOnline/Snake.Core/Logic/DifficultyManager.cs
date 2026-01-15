using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnline.Snake.Core.Logic
{
    public static class DifficultyManager
    {
        private const int StandardInterval = 250;

        public static int CalculateInterval(int score)
        {
            return StandardInterval;
        }
    }
}
