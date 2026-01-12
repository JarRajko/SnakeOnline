using SnakeOnline.Snake.Core.Enum;
using SnakeOnline.Snake.Core.Models;
using SnakeClass = SnakeOnline.Snake.Core.Models.Snake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeOnline.Input;

namespace SnakeOnline.Snake.Core.Logic
{

    public class GameEngine
    {
        private readonly FoodManager _foodManager;
        private float _speedBuffTimer = 0; // Zostávajúci čas buffu v sekundách
        private const float NormalSpeed = 150f;
        private const float BoostSpeed = 80f;
        private int _currentInterval = 150; // Základná rýchlosť


        public event Action<int> OnSpeedChanged;
        // Definujeme udalosť, ktorú bude MainForm počúvať
        public event Action<int> OnTimerIntervalChanged;
        public GameState CurrentState { get; private set; }
        public GameEngine() 
        {
            CurrentState = new GameState();
            var playerSnake = new Models.Snake(new Position(5, 5), Direction.RIGHT);
            CurrentState.Snakes.Add(playerSnake);
            _foodManager = new FoodManager(CurrentState.GridWidth, CurrentState.GridHeight);

            // Na začiatku vygenerujeme napr. 3 jedlá
            _foodManager.DistributeFood(CurrentState.ActiveFoods, CurrentState.Snakes, 3);
        }

        // Samotná metóda, ktorú volajú jedlá alebo Update
        public void ChangeTimerInterval(int newInterval)
        {
            _currentInterval = newInterval; // Uložíme si novú rýchlosť
            OnTimerIntervalChanged?.Invoke(newInterval);
        }

        public void Update()
        {
            // 1. Ak je hra na konci, nič ďalšie nerobíme
            if (CurrentState.IsGameOver) return;

            // 2. Spracovanie aktívneho buffu (odpočítavanie času)
            if (_speedBuffTimer > 0)
            {
                // Odpočítame reálny čas, ktorý prešiel. 
                // Keďže metóda Update sa volá v každom tiku timeru, 
                // odpočítame dĺžku tohto intervalu (v milisekundách prepočítaných na sekundy).
                _speedBuffTimer -= (float)_currentInterval / 1000f;

                // Ak čas vypršal, vrátime rýchlosť do normálu
                if (_speedBuffTimer <= 0)
                {
                    _speedBuffTimer = 0;
                    ChangeTimerInterval(150); // Návrat na základných 150ms
                }
            }

            // 3. Pohyb a interakcia (jedenie)
            // Túto logiku máš pravdepodobne v MoveSnakes(), tak ju zavoláme
            MoveSnakes();

            // 4. Doplnenie jedla (ak nejaké zmizlo po zjedení v MoveSnakes)
            _foodManager.DistributeFood(CurrentState.ActiveFoods, CurrentState.Snakes, 3);
        }

        public void HandleInput(Keys key)
        {
            var inputHandler = new InputHandler();
            Direction? newDir = inputHandler.GetDirection(key);

            if (newDir.HasValue)
            {
                // Tu povieme prvému hadovi (hráčovi), aby zmenil smer
                // Musíš mať v triede Snake metódu ChangeDirection
                CurrentState.Snakes[0].ChangeDirection(newDir.Value);
            }
        }

        public void ApplySpeedBuff(float durationInSeconds, float newInterval)
        {
            _speedBuffTimer = durationInSeconds;
            ChangeTimerInterval((int)newInterval);
        }

        private void MoveSnakes()
        {
            foreach (var snake in CurrentState.Snakes)
            {
                // 1. Pohneme len hadmi, ktorí ešte žijú
                if (!snake.IsAlive) continue;

                snake.Move();

                // 2. Skontrolujeme, či hlava hada narazila na nejaké jedlo
                // Používame LINQ FirstOrDefault na prehľadanie zoznamu ActiveFoods
                var eatenFood = CurrentState.ActiveFoods.FirstOrDefault(f =>
                    f.Position.X == snake.Body[0].X && f.Position.Y == snake.Body[0].Y);

                if (eatenFood != null)
                {
                    // Jedlo samo vykoná svoj efekt (rast, zmena rýchlosti...)
                    eatenFood.ApplyEffect(snake, this);

                    // Odstránime ho, aby ho nemohol zjesť niekto iný
                    CurrentState.ActiveFoods.Remove(eatenFood);

                    // Zvýšime skóre v GameState
                    CurrentState.Score += 10;
                }

                // 3. Po pohybe skontrolujeme, či had nenarazil do steny alebo seba
                CheckCollisions(snake);
            }

            // 4. Až keď sa pohnú všetci hadi, manažér doplní jedlo na plochu do počtu 3
            _foodManager.DistributeFood(CurrentState.ActiveFoods, CurrentState.Snakes, 3);
        }

        private void HandleSpeedEffect(Models.Snake snake, int speed)
        {
            // Základná rýchlosť je napr. 150ms
            // Ak chceme zrýchliť, zmenšíme interval (napr. na 100ms)
            // Ak chceme spomaliť, zväčšíme ho (napr. na 200ms)

            int newInterval = 150;

            if (CurrentState.Score % 50 == 0) // Každých 50 bodov zrýchlime
            {
                newInterval = Math.Max(50, 150 - (CurrentState.Score / 10 * 5));
                OnSpeedChanged?.Invoke(newInterval);
            }
        }

        private Food HasEatenFood(Models.Snake snake)
        {
            var head = snake.Body[0];
            // Hľadáme v zozname ActiveFoods, či je nejaké jedlo na súradniciach hlavy
            return CurrentState.ActiveFoods.FirstOrDefault(f => f.Position.X == head.X && f.Position.Y == head.Y);
        }


        private void CheckCollisions(SnakeClass snake)
        {
            Position head = snake.Body[0];

            if (head.X < 0 || head.X >= CurrentState.GridWidth ||
                head.Y < 0 || head.Y >= CurrentState.GridHeight)
            {
                snake.IsAlive = false;
                CurrentState.IsGameOver = true;
                return;
            }

            for (int i = 1; i < snake.Body.Count; i++)
            {
                if (head.X == snake.Body[i].X && head.Y == snake.Body[i].Y)
                {
                    snake.IsAlive = false;
                    CurrentState.IsGameOver = true;
                    return;
                }
            }
        }

       

    }
}
