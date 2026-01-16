using SnakeOnline.Input;
using SnakeOnline.Snake.Core.Enum;
using SnakeOnline.Snake.Core.Models;
using SnakeOnline.Snake.Core.Network;

namespace SnakeOnline.Snake.Core.Logic
{
    public class GameEngine
    {
        private readonly FoodManager _foodManager;
        private const float BoostSpeed = 80f;
        private int _currentInterval = 1000;//150;
        private int activeFoods = 5;
        private bool _hasChangedDirectionThisTick = false;
        private readonly InputHandler _inputHandler = new InputHandler();

        public event Action<int> OnTimerIntervalChanged;
        public GameState CurrentState { get; private set; }

        public GameEngine(bool isMultiplayer)
        {
            CurrentState = new GameState();
            CurrentState.CurrentSpeed = _currentInterval;

            var player1 = new Models.Snake(new Position(5, 5), Direction.RIGHT,Color.Green);
            CurrentState.Snakes.Add(player1);

            if (isMultiplayer)
            {
                var player2 = new Models.Snake(new Position(15, 15), Direction.UP, Color.Red);
                CurrentState.Snakes.Add(player2);
            }

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

            if (CurrentState.Snakes.Count > 0)
            {
                packet.Score = CurrentState.Snakes[0].Score;
            }

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
                        SnakeOnline.Snake.Core.Enum.Direction.NONE, Color.Gold);
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
            _currentInterval = newInterval;
            OnTimerIntervalChanged?.Invoke(newInterval);
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
                if (CurrentState.Snakes.Count > 0)
                {
                    if (CurrentState.Snakes[0].ChangeDirection(newDir))
                    {
                        _hasChangedDirectionThisTick = true;
                    }
                }
            }
        }

        private void MoveSnakes()
        {
            foreach (var snake in CurrentState.Snakes)
            {
                if (!snake.IsAlive) continue;

                snake.Move();

                Food eatenFood = null;
                foreach (var food in CurrentState.ActiveFoods)
                {
                    if (food.Position.X == snake.Body[0].X && food.Position.Y == snake.Body[0].Y)
                    {
                        eatenFood = food;
                        break;
                    }
                }

                if (eatenFood != null)
                {
                    eatenFood.ApplyEffect(snake, CurrentState);
                    CurrentState.ActiveFoods.Remove(eatenFood);

                    if (snake == CurrentState.Snakes[0])
                    {
                        CurrentState.Score += 10;
                    }

                    ChangeTimerInterval(CurrentState.CurrentSpeed);
                    _foodManager.DistributeFood(CurrentState.ActiveFoods, CurrentState.Snakes, activeFoods);
                }

                CheckCollisions(snake);
            }
        }

        private void CheckCollisions(SnakeOnline.Snake.Core.Models.Snake currentSnake)
        {
            Position head = currentSnake.Body[0];

            if (IsOutOfBounds(head))
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
    }
}