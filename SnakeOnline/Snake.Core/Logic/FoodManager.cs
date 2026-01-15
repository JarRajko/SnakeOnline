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
            Position pos = GetRandomAvailablePosition(snakes);
            var foodTypes = new List<Food>
    {
        new Apple(pos),
        new Plum(pos),
        new Pear(pos)
    };

            int totalWeight = foodTypes.Sum(f => f.SpawnChance);
            int roll = _rand.Next(0, totalWeight);

            int currentWeight = 0;
            foreach (var food in foodTypes)
            {
                currentWeight += food.SpawnChance;
                if (roll < currentWeight)
                {
                    return food;
                }
            }
            return new Apple(pos);
        }

        private Position GetRandomAvailablePosition(List<Models.Snake> snakes)
        {
            Position pos;
            bool isInvalid;

            do
            {
                isInvalid = false;
                pos = new Position(_rand.Next(0, _gridWidth), _rand.Next(0, _gridHeight));

                foreach (var snake in snakes)
                {
                    if (snake.Body.Any(part => part.X == pos.X && part.Y == pos.Y))
                    {
                        isInvalid = true;
                        break;
                    }
                }

            } while (isInvalid);
            return pos;
        }
    }

}
