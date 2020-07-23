using Controller;
using Entities;
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
        private Game game = new Game();

        private EntityView kolobokView;
        private EntityView wallsView;
        private EntityView tankskView;
        private EntityView appleView;
        private EntityView bulletsView;

        public MainForm()
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            /*
            for (int i = 0; i < game.tanks.Count; i++)
            {
                dataGridView1.Rows.Add();
            }
            */
            pictureBox.Paint += new PaintEventHandler(pictureBox_Paint);
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            game.Update();

            appleView = new EntityView(e.Graphics, game.apple, new Bitmap(Properties.Resources.apple), 20);
            appleView.Draw();

            kolobokView = new EntityView(e.Graphics, game.kolobok, new Bitmap(Properties.Resources.kolobok), 20);
            kolobokView.Draw();

            tankskView = new EntityView(e.Graphics, game.tanks.Cast<Entity>().ToList(), new Bitmap(Properties.Resources.tank), 20);
            tankskView.Draw();

            wallsView = new EntityView(e.Graphics, game.walls.Cast<Entity>().ToList(), new Bitmap(Properties.Resources.wall), 20);
            wallsView.Draw();

            bulletsView = new EntityView(e.Graphics, game.bullets.Cast<Entity>().ToList(), new Bitmap(Properties.Resources.bullet), 20);
            bulletsView.Draw();

            /*
            for (int i = 0; i < game.tanks.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = i;
                dataGridView1.Rows[i].Cells[1].Value = game.tanks[i].Pos;
                dataGridView1.Rows[i].Cells[2].Value = game.tanks[i].Dir;
            }
            */
        }


        private void mainTimer_Tick(object sender, EventArgs e)
        {
            Refresh();
        }
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            KolobokController.KeyDown(game, e.KeyCode);
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            KolobokController.KeyUp(game, e.KeyCode);
        }
    }
}
