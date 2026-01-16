using SnakeOnline.Snake.Core.Network;
using SnakeOnline.Snake.Server;
using SnakeOnline.Snake.Client;
using SnakeOnline.Snake.Core.Models;

namespace SnakeOnline.Snake.Core.Logic
{
    public class MatchManager
    {
        private readonly GameEngine _engine;
        private readonly SnakeServer _server;
        private readonly SnakeClient _client;
        private readonly bool _isHost;
        private readonly bool _isClient;
        private bool _waitingForPlayer;

        public bool IsWaiting => _waitingForPlayer;

        public event Action<int> OnRequestSpeedChange;

        public GameState CurrentState
        {
            get
            {
                return _engine.CurrentState;
            }
        }

        public MatchManager(GameEngine engine, bool isHost, bool isClient, string ipAddress)
        {
            _engine = engine;
            _isHost = isHost;
            _isClient = isClient;

            _engine.OnTimerIntervalChanged += HandleEngineSpeedChange;

            if (_isHost)
            {
                _server = new SnakeServer();
                _waitingForPlayer = true;
            }

            else if (_isClient)
            {
                _client = new SnakeClient(ipAddress);
                _client.Connect();
                _waitingForPlayer = false;
            }
            else
            {
                _waitingForPlayer = false;
            }
        }

        private void HandleEngineSpeedChange(int newInterval)
        {
            if (OnRequestSpeedChange != null)
            {
                OnRequestSpeedChange.Invoke(newInterval);
            }
        }

        public void Update()
        {
            if (_isClient)
            {
                UpdateAsClient();
            }
            else
            {
                UpdateAsHostOrSolo();
            }
        }

        private void UpdateAsClient()
        {
            if (_client == null) return;

            GameStatePacket packet = _client.ReceiveWorldState();
            if (packet != null)
            {
                _engine.UpdateLocalStateFromServer(packet);
            }

            InputPacket input = new InputPacket();
            input.ChosenDirection = _engine.CurrentState.Snakes[0].CurrentDirection;
            _client.SendInput(input);
        }

        private void UpdateAsHostOrSolo()
        {
            if (_isHost && _server != null)
            {
                InputPacket enemyInput = _server.ReceiveInput();

                if (enemyInput != null)
                {
                    _waitingForPlayer = false;

                    if (_engine.CurrentState.Snakes.Count > 1)
                    {
                        _engine.CurrentState.Snakes[1].ChangeDirection(enemyInput.ChosenDirection);
                    }
                }

                if (_waitingForPlayer)
                {
                    return;
                }
            }

            _engine.Update();

            if (_isHost && _server != null)
            {
                GameStatePacket packet = _engine.CreatePacketFromState();
                _server.SendWorldState(packet);
            }
        }

        public void HandleLocalInput(Keys key)
        {
            _engine.HandleInput(key);
        }
    }
}