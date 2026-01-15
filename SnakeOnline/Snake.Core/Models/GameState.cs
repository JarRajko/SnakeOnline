using SnakeOnline.Snake.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnline.Snake.Core.Models
{
    public delegate void SpeedChangedHandler(int newSpeed);

    public class GameState
    {
        public event SpeedChangedHandler OnSpeedChanged;
        private int _currentSpeed;

        public List<Snake> Snakes { get; init; } = new List<Snake>();
        public List<Models.Food> ActiveFoods { get; set; } = new List<Models.Food>();
        public bool IsGameOver { get; set; }
        public int Score { get; set; } = 0;
        public int GridWidth { get; init; } = 30;
        public int GridHeight { get; init; } = 30;

        public int CurrentSpeed
        {
            get => _currentSpeed;
            set
            {
                if (_currentSpeed != value)
                {
                    _currentSpeed = value;
                    OnSpeedChanged?.Invoke(_currentSpeed);
                }
            }
        }

    }
}
