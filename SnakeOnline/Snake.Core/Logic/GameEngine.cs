using SnakeOnline.Snake.Core.Enum;
using SnakeOnline.Snake.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnline.Snake.Core.Logic
{
    public class GameEngine
    {
        public GameState CurrentState { get; private set; }
        public GameEngine() 
        {
            CurrentState = new GameState();
            var playerSnake = new Models.Snake(new Position(5, 5), Direction.RIGHT);
            CurrentState.Snakes.Add(playerSnake);
            CurrentState.FoodPosition = new Position(10, 10);
        }

        public void Update()
        {
            if (!CurrentState.IsGameOver)
            {
               // MoveSnakes();
               // CheckCollisions();
            }
        }

        public void ChangeDirection(Direction newDirection)
        {
            //var snake = CurrentState.PlayerSnake;
            //if (IsOpposite(snake.CurrentDirection, newDirection)) return;

           // snake.SetDirection(newDirection);
        }

        private bool IsOpposite(Direction d1, Direction d2)
        {
            if (d1 == Direction.UP && d2 == Direction.DOWN) return true;
            if (d1 == Direction.DOWN && d2 == Direction.UP) return true;
            if (d1 == Direction.LEFT && d2 == Direction.RIGHT) return true;
            if (d1 == Direction.RIGHT && d2 == Direction.LEFT) return true;
            return false;

        }

    }
}
