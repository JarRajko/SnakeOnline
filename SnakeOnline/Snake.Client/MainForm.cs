using Microsoft.VisualBasic;
using SnakeOnline.Input;
using SnakeOnline.Rendering;
using SnakeOnline.Snake.Client;
using SnakeOnline.Snake.Core.Logic;
using SnakeOnline.Snake.Core.Network;
using SnakeOnline.Snake.Core.Enum;
using SnakeOnline.Snake.Server;

namespace SnakeOnline
{
    public partial class MainForm : Form
    {
        private readonly GameEngine _engine;
        private readonly GameRenderer _renderer;
        private readonly InputHandler _inputHandler;

        private bool _isMultiplayer = false;
        private SnakeServer _server;
        private SnakeClient _client;
        private bool _isHost;
        private bool _isClient;
        private System.Windows.Forms.Timer gameTimer = new System.Windows.Forms.Timer();

        public MainForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            _engine = new GameEngine(false);
            _renderer = new GameRenderer(16);
            _inputHandler = new InputHandler();

            gameTimer.Interval = 100;
            gameTimer.Tick += OnGameTimer_Tick;

            this.KeyPreview = true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            _engine.HandleInput(e.KeyCode);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (gameTimer.Enabled)
            {
                _renderer.Render(e.Graphics, _engine.CurrentState);
            }
        }

        private void OnGameTimer_Tick(object sender, EventArgs e)
        {
            if (_isClient)
            {
                var state = _client.ReceiveWorldState();
                if (state != null)
                {
                    _engine.UpdateLocalStateFromServer(state);
                }
            }
            else
            {
                if (_isHost)
                {
                    var input = _server.ReceiveInput();
                    if (input != null && _engine.CurrentState.Snakes.Count > 1)
                    {
                        _engine.CurrentState.Snakes[1].ChangeDirection(input.ChosenDirection);
                    }
                }

                _engine.Update();

                ChangeTimerInterval(_engine.CurrentState.CurrentSpeed);

                if (_isHost)
                {
                    var packet = _engine.CreatePacketFromState();
                    _server.SendWorldState(packet);
                }
            }

            this.Invalidate();
        }

        private void btnHost_Click(object sender, EventArgs e)
        {
            _server = new SnakeServer();
            StartGame(true, false);
        }

        private void btnSolo_Click_1(object sender, EventArgs e)
        {
            StartGame(false, false);
        }

        private void btnJoin_Click_1(object sender, EventArgs e)
        {
            _client = new SnakeClient(txtIpAddress.Text);
            StartGame(false, true);
        }

        private void StartGame(bool isHost, bool isClient)
        {
            _isHost = isHost;
            _isClient = isClient;

            if (gamePanel != null) gamePanel.Visible = false;
            btnSolo.Visible = false;
            btnJoin.Visible = false;
            btnHost.Visible = false;
            txtIpAddress.Visible = false;
            gamePanel.Visible = false;

            _engine.CurrentState.OnSpeedChanged += (newSpeed) => {
                this.Invoke((MethodInvoker)delegate {
                    gameTimer.Interval = newSpeed;
                });
            };

            int tileSize = 16;
            this.ClientSize = new Size(
                _engine.CurrentState.GridWidth * tileSize,
                _engine.CurrentState.GridHeight * tileSize
            );

            gameTimer.Start();
            this.Focus();
        }

        public void ChangeTimerInterval(int newInterval)
        {
            if (newInterval > 0)
            {
                gameTimer.Interval = newInterval;
            }
        }

    }

}
