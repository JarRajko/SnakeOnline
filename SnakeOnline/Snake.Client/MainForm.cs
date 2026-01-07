using Microsoft.VisualBasic;
using SnakeOnline.Rendering;
using SnakeOnline.Snake.Core.Logic;

namespace SnakeOnline
{
    public partial class MainForm : Form
    {
        private readonly GameEngine _engine;
        private readonly GameRenderer _renderer;
        public MainForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            _engine = new GameEngine();
            _renderer = new GameRenderer();

            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 100;//ms
            timer.Start();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S)
            {

            }
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
    
}
