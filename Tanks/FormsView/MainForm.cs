using Controller;
using Entities;
using FormsView.View;
using FormsView.Views;
using GameLogic;
using Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsView
{
    public partial class MainForm : Form
    {
        private Game game;

        private int blockSize = 20;

        private EntityView kolobokView;
        private FastEntityView wallsView;
        private EntityView watersView;
        private EntityView tankskView;
        private EntityView appleView;
        private EntityView bulletsView;
        private EntityView explosionsView;

        private int width;
        private int height;
        private int tanks;
        private int apples;
        private int speed;

        public MainForm(int width, int height, int tanks, int apples, int speed)
        {
            InitializeComponent();

            this.width = width;
            this.height = height;
            this.tanks = tanks;
            this.apples = apples;
            this.speed = speed;

            game = new Game(width, height, blockSize, tanks, apples, speed);

            game.GameOver += OnLoose;

            this.Width = width + 150;
            this.Height = height + 60;

            pictureBox.Height = height;
            pictureBox.Width = width;

            labelScore.Location = new Point(width + 20, 10);

            pictureInformation.Location = new Point(width + 20, height - 70);

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = null;

            pictureBox.Paint += new PaintEventHandler(pictureBox_Paint);

            lastTime = DateTime.Now;
        }


        DateTime lastTime;
        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            TimeSpan time = DateTime.Now - lastTime;
            lastTime = DateTime.Now;
            double dTime = time.TotalMilliseconds / 20;

            game.Update(1);

            watersView = new EntityView(e.Graphics, game.waters.Cast<Entity>().ToList(), new Bitmap(Properties.Resources.water), 20);
            watersView.Draw();

            appleView = new EntityView(e.Graphics, game.apples.Cast<Entity>().ToList(), new Bitmap(Properties.Resources.apple), 20);
            appleView.Draw();

            kolobokView = new EntityView(e.Graphics, game.kolobok, KolobokAnim(), 20);
            kolobokView.Draw();

            tankskView = new EntityView(e.Graphics, game.tanks.Cast<Entity>().ToList(), new Bitmap(Properties.Resources.tank), 20);
            tankskView.Draw();

            wallsView = new FastEntityView(e.Graphics, game.walls.Cast<Entity>().ToList(), new Bitmap(Properties.Resources.wall), 20);
            wallsView.Draw();           

            bulletsView = new EntityView(e.Graphics, game.bullets.Cast<Entity>().ToList(), new Bitmap(Properties.Resources.bullet), 20);
            bulletsView.Draw();

            explosionsView = new EntityView(e.Graphics, game.explosions.Cast<Entity>().ToList(), ExplosionAnim(), 20);
            explosionsView.Draw();

            labelScore.Text = string.Format("Score: {0}", game.Score);
        }

        private int frames = 0;

        private Bitmap KolobokAnim()
        {
            frames++;
            if (frames > 30)
                frames = 0;
            if (frames < 15)
                return new Bitmap(Properties.Resources.kolobok);
            else
                return new Bitmap(Properties.Resources.kolobok2);
        }
        private Bitmap ExplosionAnim()
        {
            if (frames < 2)
                return new Bitmap(Properties.Resources.explosion_0);
            else if (frames < 4)
                return new Bitmap(Properties.Resources.explosion_1);
            else if (frames < 6)
                return new Bitmap(Properties.Resources.explosion_2);
            else if (frames < 8)
                return new Bitmap(Properties.Resources.explosion_3);
            else if (frames < 10)
                return new Bitmap(Properties.Resources.explosion_4);
            else if (frames < 12)
                return new Bitmap(Properties.Resources.explosion_5);
            else if (frames < 14)
                return new Bitmap(Properties.Resources.explosion_6);
            else if (frames < 16)
                return new Bitmap(Properties.Resources.explosion_7);
            else if (frames < 18)
                return new Bitmap(Properties.Resources.explosion_8);
            else if (frames < 20)
                return new Bitmap(Properties.Resources.explosion_9);
            else
                return new Bitmap(16, 16);
        }

        private void OnLoose(int score)
        {
            string message = string.Format("You score: {0}. Want to try again?", score);
            string title = "Game over";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                game = new Game(width, height, blockSize, tanks, apples, speed);
                game.GameOver += OnLoose;
            }
            else
            {
                this.Close();
            }
        }

        private void mainTimer_Tick(object sender, EventArgs e)
        {
            Refresh();
        }
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            this.ActiveControl = null;
            KolobokController.KeyDown(game, e.KeyCode);
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            KolobokController.KeyUp(game, e.KeyCode);
        }

        private void pictureInformation_Click(object sender, EventArgs e)
        {
            Information info = new Information(game);
            info.Show();
        }
    }
}
