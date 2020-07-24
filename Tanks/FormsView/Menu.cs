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
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxSize1.Text, out int width) &&
                width >= 200 &&
                int.TryParse(textBoxSize2.Text, out int height) &&
                width >= 200 &&
                int.TryParse(textBoxTanks.Text, out int tanks) &&
                int.TryParse(textBoxApples.Text, out int apples) &&
                int.TryParse(textBoxSpeed.Text, out int speed) &&
                speed >= 1)
            {
                this.Hide();
                MainForm main = new MainForm(width, height, tanks, apples, speed);
                main.ShowDialog();
                this.Close();
            }
            else
            {
                string message = string.Format("Incorrect data");
                string title = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
            }
        }
        private void KeyPress(object sender, KeyPressEventArgs e)
        {

            if (Char.IsDigit(e.KeyChar) | e.KeyChar == '\b') return;
            else
                e.Handled = true;
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
