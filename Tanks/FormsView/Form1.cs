using Entities;
using GameLogic;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;

            this.Paint += new PaintEventHandler(Form1_Paint);
        }
        private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Entity[,] map = MapGenerator.Generate(40, 20, 20);
            Bitmap wallImg = new Bitmap("D:\\test.png");

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] != null)
                    {
                        e.Graphics.DrawImage(wallImg, 20 * i, 20 * j, 20, 20);
                    }
                }
            }
            //e.Graphics.DrawImage(wallImg, 100, 10, 20, 20);
        }
    }
}
