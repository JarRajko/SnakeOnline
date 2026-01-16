using SnakeOnline.Rendering;
using SnakeOnline.Snake.Core.Logic;

namespace SnakeOnline
{
    public partial class MainForm : Form
    {
        private readonly GameRenderer _renderer;
        private MatchManager _matchManager;
        private System.Windows.Forms.Timer gameTimer = new System.Windows.Forms.Timer();

        public MainForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.KeyPreview = true;

            _renderer = new GameRenderer(16);
            gameTimer.Tick += OnGameTimer_Tick;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            _matchManager?.HandleLocalInput(e.KeyCode);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (gameTimer.Enabled && _matchManager != null)
            {
                _renderer.Render(e.Graphics, _matchManager.CurrentState);
            }
        }

        private void OnGameTimer_Tick(object sender, EventArgs e)
        {
            _matchManager.Update();
            this.Invalidate();
        }

        private void btnHost_Click(object sender, EventArgs e) => StartGame(true, false);
        private void btnSolo_Click_1(object sender, EventArgs e) => StartGame(false, false);
        private void btnJoin_Click_1(object sender, EventArgs e) => StartGame(false, true);

        private void StartGame(bool isHost, bool isClient)
        {
            GameEngine engine = new GameEngine(isHost || isClient);
            _matchManager = new MatchManager(engine, isHost, isClient, txtIpAddress.Text);
            _matchManager.OnRequestSpeedChange += HandleSpeedChange;
            HideMenu();

            SetupWindowSize(_matchManager.CurrentState.GridWidth, _matchManager.CurrentState.GridHeight);

            gameTimer.Interval = _matchManager.CurrentState.CurrentSpeed;
            gameTimer.Start();
            this.Focus();
        }

        private void HideMenu()
        {
            btnSolo.Visible = false;
            btnJoin.Visible = false;
            btnHost.Visible = false;
            txtIpAddress.Visible = false;
            if (gamePanel != null) gamePanel.Visible = false;
        }

        private void SetupWindowSize(int width, int height)
        {
            int tileSize = 16;
            this.ClientSize = new Size(width * tileSize, height * tileSize);
        }

        private void HandleSpeedChange(int newInterval)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => gameTimer.Interval = newInterval));
            }
            else
            {
                gameTimer.Interval = newInterval;
            }
        }
    }
}