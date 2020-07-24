using Entities;
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
    public partial class Information : Form
    {
        Game game;
        public Information(Game game)
        {
            this.game = game;
            InitializeComponent();
        }

        private void Information_Load(object sender, EventArgs e)
        {
            /*
            dataGridView1.DataSource = game.AllEntity();

            dataGridView1.Columns[0].DataPropertyName = "GetType().Name";
            /*
            dataGridView1.Rows.Add();
            for (int i = 0; i < game.tanks.Count; i++)
                dataGridView1.Rows.Add();
            for (int i = 0; i < game.bullets.Count; i++)
                dataGridView1.Rows.Add();
            for (int i = 0; i < game.apples.Count; i++)
                dataGridView1.Rows.Add();

            for (int i = 0; i < game.walls.Count; i++)
                dataGridView1.Rows.Add();
            for (int i = 0; i < game.waters.Count; i++)
                dataGridView1.Rows.Add();

            WriteInfo(game.kolobok, 0);
            int writePos = 1;

            for (int i = 0; i < game.tanks.Count; i++)
            {
                WriteInfo(game.tanks[i], writePos);
                writePos++;
            }
            for (int i = 0; i < game.bullets.Count; i++)
            {
                WriteInfo(game.bullets[i], writePos);
                writePos++;
            }
            for (int i = 0; i < game.apples.Count; i++)
            {
                WriteInfo(game.apples[i], writePos);
                writePos++;
            }

            for (int i = 0; i < game.walls.Count; i++)
            {
                WriteInfo(game.walls[i], writePos);
                writePos++;
            }
            for (int i = 0; i < game.waters.Count; i++)
            {
                WriteInfo(game.waters[i], writePos);
                writePos++;
            }
            dataGridView1.Update();
            dataGridView1.Refresh();
            */
        }

        private void WriteInfo(Entity entity, int row)
        {
            dataGridView1.Rows[row].Cells[0].Value = entity.GetType().Name;
            dataGridView1.Rows[row].Cells[1].Value = entity.Pos.X;
            dataGridView1.Rows[row].Cells[2].Value = entity.Pos.Y;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            dataGridView1.RowCount = game.AllEntity().Count;

            for (int i = 0; i < game.AllEntity().Count; i++)
            {
                WriteInfo(game.AllEntity()[i], i);
            }

            //dataGridView1.Update();
            dataGridView1.Refresh();
        }
    }
}
