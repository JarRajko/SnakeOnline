using Microsoft.VisualBasic;
using SnakeOnline.Input;
using SnakeOnline.Rendering;
using SnakeOnline.Snake.Core.Logic;

namespace SnakeOnline
{
    public partial class MainForm : Form
    {
        private readonly GameEngine _engine;
        private readonly GameRenderer _renderer;
        private readonly InputHandler _inputHandler;
        public MainForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            _engine = new GameEngine();
            _renderer = new GameRenderer();
            _inputHandler = new InputHandler();

            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 100;//ms
            timer.Tick += OnGameTimer_Tick;
            timer.Start();

            _engine.OnTimerIntervalChanged += (newInterval) =>
            {
                timer.Interval = newInterval;
            };

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            // Odovzdáme klávesu enginu, on už vie, ?o s tým
            _engine.HandleInput(e.KeyCode);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            _renderer.Render(e.Graphics, _engine.CurrentState);
        }

        private void OnGameTimer_Tick(object sender, EventArgs e)
        {
            _engine.Update();
            this.Invalidate(); //prekleslenie okna
        }
    }
    
}
