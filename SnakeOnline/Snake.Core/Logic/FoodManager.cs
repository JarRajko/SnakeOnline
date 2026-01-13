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
            // 1. Manažér nájde voľné miesto (jeho zodpovednosť)
            Position pos = GetRandomAvailablePosition(snakes);

            // 2. Vytvoríme inštancie a priradíme im pozíciu cez inicializátor { Position = pos }
            var foodTypes = new List<Food>
    {
        new Apple { Position = pos },
        new Plum { Position = pos },
        new Pear { Position = pos }
    };

            // 3. Výber podľa šancí (Weighted Random)
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

            return new Apple { Position = pos };
        }

        private Position GetRandomAvailablePosition(List<Models.Snake> snakes)
        {
            Position pos;
            bool isInvalid;

            do
            {
                isInvalid = false;
                // Vygenerujeme náhodné súradnice v rámci mriežky
                pos = new Position(_rand.Next(0, _gridWidth), _rand.Next(0, _gridHeight));

                // Skontrolujeme, či na tejto pozícii nestojí nejaký had
                foreach (var snake in snakes)
                {
                    if (snake.Body.Any(part => part.X == pos.X && part.Y == pos.Y))
                    {
                        isInvalid = true;
                        break;
                    }
                }

            } while (isInvalid); // Opakujeme, kým nenájdeme prázdne miesto

            return pos;
        }
    }

}
