using SnakeOnline.Snake.Core.Enum;
using SnakeOnline.Snake.Core.Models;
using SnakeClass = SnakeOnline.Snake.Core.Models.Snake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeOnline.Input;
using SnakeOnline.Snake.Core.Network;

namespace SnakeOnline.Snake.Core.Logic
{

    public class GameEngine
    {
        private readonly FoodManager _foodManager;
        private const float BoostSpeed = 80f;
        private int _currentInterval = 150; private int activeFoods = 5;
        private bool _hasChangedDirectionThisTick = false;
        private readonly InputHandler _inputHandler = new InputHandler();

        public event Action<int> OnSpeedChanged;
        public event Action<int> OnTimerIntervalChanged;
        public GameState CurrentState { get; private set; }
        public GameEngine(bool isHost)
        {
            CurrentState = new GameState();
            CurrentState.CurrentSpeed = _currentInterval;

            var playerSnake = new Models.Snake(new Position(5, 5), Direction.RIGHT);

            CurrentState.Snakes.Add(playerSnake);

            _foodManager = new FoodManager(CurrentState.GridWidth, CurrentState.GridHeight);

            _foodManager.DistributeFood(CurrentState.ActiveFoods, CurrentState.Snakes, activeFoods);
        }

        public GameStatePacket CreatePacketFromState()
        {
            GameStatePacket packet = new GameStatePacket();
            packet.AllSnakesPositions = new List<List<Position>>();
            foreach (var snake in CurrentState.Snakes)
            {
                List<Position> snakeBody = new List<Position>();
                foreach (var segment in snake.Body)
                {
                    snakeBody.Add(segment);
                }
                packet.AllSnakesPositions.Add(snakeBody);
            }

            packet.FoodPositions = new List<Position>();
            foreach (var food in CurrentState.ActiveFoods)
            {
                packet.FoodPositions.Add(food.Position);
            }

            packet.Score = CurrentState.Snakes[0].Score;
            packet.IsGameOver = CurrentState.IsGameOver;

            return packet;
        }

        public void UpdateLocalStateFromServer(GameStatePacket packet)
        {
            CurrentState.IsGameOver = packet.IsGameOver;

            for (int i = 0; i < packet.AllSnakesPositions.Count; i++)
            {
                if (i >= CurrentState.Snakes.Count)
                {
                    var newSnake = new SnakeOnline.Snake.Core.Models.Snake(
                        packet.AllSnakesPositions[i][0],
                        SnakeOnline.Snake.Core.Enum.Direction.NONE);
                    CurrentState.Snakes.Add(newSnake);
                }

                CurrentState.Snakes[i].SyncBody(packet.AllSnakesPositions[i]);
            }

            CurrentState.ActiveFoods.Clear();
            foreach (var pos in packet.FoodPositions)
            {
                var food = new SnakeOnline.Snake.Core.Models.Apple(pos);
                CurrentState.ActiveFoods.Add(food);
            }
        }

        public void ChangeTimerInterval(int newInterval)
        {
            _currentInterval = newInterval; OnTimerIntervalChanged?.Invoke(newInterval);
        }

        public void Update()
        {
            if (CurrentState.IsGameOver) return;


            MoveSnakes();
            _hasChangedDirectionThisTick = false;
            _foodManager.DistributeFood(CurrentState.ActiveFoods, CurrentState.Snakes, activeFoods);
        }

        public void HandleInput(Keys key)
        {
            if (_hasChangedDirectionThisTick) return;

            Direction newDir = _inputHandler.GetDirection(key);

            if (newDir != Direction.NONE)
            {
                if (CurrentState.Snakes[0].ChangeDirection(newDir))
                {
                    _hasChangedDirectionThisTick = true;
                }
            }
        }


        private void MoveSnakes()
        {
            foreach (var snake in CurrentState.Snakes)
            {
                if (!snake.IsAlive) continue;

                snake.Move();

                var eatenFood = CurrentState.ActiveFoods.FirstOrDefault(f =>
                    f.Position.X == snake.Body[0].X && f.Position.Y == snake.Body[0].Y);

                if (eatenFood != null)
                {
                    eatenFood.ApplyEffect(snake, CurrentState);
                    CurrentState.ActiveFoods.Remove(eatenFood);
                    CurrentState.Score += 10;

                    ChangeTimerInterval(CurrentState.CurrentSpeed);

                    _foodManager.DistributeFood(CurrentState.ActiveFoods, CurrentState.Snakes, activeFoods);
                }

                CheckCollisions(snake);
            }

        }

        private Food HasEatenFood(Models.Snake snake)
        {
            var head = snake.Body[0];
            return CurrentState.ActiveFoods.FirstOrDefault(f => f.Position.X == head.X && f.Position.Y == head.Y);
        }

        private void CheckCollisions(SnakeClass currentSnake)
        {
            Position head = currentSnake.Body[0];

            if (head.X < 0 || head.X >= CurrentState.GridWidth ||
                head.Y < 0 || head.Y >= CurrentState.GridHeight)
            {
                currentSnake.IsAlive = false;
                CurrentState.IsGameOver = true;
                return;
            }

            foreach (var otherSnake in CurrentState.Snakes)
            {
                for (int i = 0; i < otherSnake.Body.Count; i++)
                {
                    if (currentSnake == otherSnake && i == 0) continue;

                    if (head.X == otherSnake.Body[i].X && head.Y == otherSnake.Body[i].Y)
                    {
                        if (i == 0)
                        {
                            currentSnake.IsAlive = false;
                            otherSnake.IsAlive = false;
                            CurrentState.IsGameOver = true;
                        }
                        else
                        {
                            otherSnake.CutAt(i);
                        }
                        return;
                    }
                }
            }
        }

        private bool IsOutOfBounds(Position head)
        {
            return head.X < 0 || head.X >= CurrentState.GridWidth ||
                   head.Y < 0 || head.Y >= CurrentState.GridHeight;
        }

        private void EndGame(SnakeClass snake)
        {
            snake.IsAlive = false;
            CurrentState.IsGameOver = true;
        }



    }
}
