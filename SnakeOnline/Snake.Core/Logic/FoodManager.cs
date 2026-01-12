using SnakeOnline.Snake.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace SnakeOnline.Snake.Core.Logic
{
    public class FoodManager
    {
        private readonly Random _rand = new Random();
        private int _gridWidth;
        private int _gridHeight;

        public FoodManager(int gridWidth, int gridHeight)
        {
            this._gridWidth = gridWidth;
            this._gridHeight = gridHeight;
        }

        internal void DistributeFood(List<Food> activeFoods, List<Models.Snake> snakes, int targetCount)
        {
            while (activeFoods.Count < targetCount)
            {
                activeFoods.Add(CreateRandomFood(snakes));
            }
        }

        private Food CreateRandomFood(List<SnakeOnline.Snake.Core.Models.Snake> snakes)
        {
            Position pos;
            bool collision;

            do
            {
                collision = false;
                pos = new Position(_rand.Next(0, _gridWidth), _rand.Next(0, _gridHeight));

                // Kontrola, aby sa jedlo neobjavilo na tele hada
                if (snakes.Any(s => s.Body.Any(p => p.X == pos.X && p.Y == pos.Y)))
                    collision = true;

            } while (collision);

            // Logika náhodného výberu typu jedla TODO
            if (_rand.Next(0, 100) < 20)
                return new Plum { Position = pos };

            return new Apple { Position = pos };
        }
    }

}
