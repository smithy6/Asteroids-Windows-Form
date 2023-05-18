using System;
using System.Windows.Forms;
using System.Threading;

namespace Asteroids {
    public partial class Form1 : Form {
        // Game variable
        public Game game = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public Form1() {
            InitializeComponent();
            game = new Game(pictureBox1.Width, pictureBox1.Height);
        }

        /// <summary>
        /// Function that updates the Frame each tick
        /// </summary>
        public void UpdateFrame() {
            if(pictureBox1.Image!=null)
                pictureBox1.Image.Dispose();
            pictureBox1.Image = game.GetFrame();
        }

        /// <summary>
        /// Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e) {
            // Começa o jogo
            game.PlayGame();
            // Desabilitar e atualizar componentes do form para que fiquem consistentes com o jogo
            label1.Text = "Level: " + game.Level;
            timer1.Interval = 40;
            button1.Visible = false;
            button1.Enabled = false;
            label3.Visible = false;
            label3.Enabled = false;
            timer1.Start();
            // Give time for stopwatch to allow it to shoot
            Thread.Sleep(1000);
        }
        
        /// <summary>
        /// Tick event controller
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e) {
            // Calculate game state
            GameState state = game.Logic();
            // Update score after logic
            label2.Text = "Score: " + game.Score;
            // Update current frame
            UpdateFrame();
            // Verify game state
            if(state!= GameState.Running) {
                if(state == GameState.Won) {
                    label1.Text = "";
                    label2.Text = "You won this level, get ready for the next one!";
                    if(++game.Level == 6) {
                        label1.Text = "";
                        label2.Text = "You finished the game!";
                    }
                } else {
                    label2.Text = "You've lost the game. Try again!";
                    label1.Text = "";
                    game.Level = 1;
                }
                pictureBox1.Image = null;
                timer1.Stop();
                button1.Visible = true;
                button1.Enabled = true;
            }
        }
    }
}
