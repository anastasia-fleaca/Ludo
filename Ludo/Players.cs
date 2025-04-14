using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ludo
{
    public partial class Players : Form
    {
        public Players()
        {
            InitializeComponent();
            this.BackgroundImage = Image.FromFile("imagini/PLAYERS.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.ActiveControl = null;
            float relY = 1.6f;
            float relX1 = 0.25f;
            float relX2 = 0.83f;
            float relX3 = 1.42f;
            float relX4 = 1.95f;
            int newX1 = (int)(this.ClientSize.Width * relX1);
            int newX2 = (int)(this.ClientSize.Width * relX2);
            int newX3 = (int)(this.ClientSize.Width * relX3);
            int newX4 = (int)(this.ClientSize.Width * relX4);
            int newY = (int)(this.ClientSize.Height * relY);
            textBox1.Location = new Point(newX1, newY);
            textBox2.Location = new Point(newX2, newY);
            textBox3.Location = new Point(newX3, newY);
            textBox4.Location = new Point(newX4, newY);
            float relXB = 1.15f;
            float relYB = 1.9f;
            float relWidth = 0.25f;
            float relHeight = 0.13f;

            int newWidth = (int)(this.ClientSize.Width * relWidth);
            int newHeight = (int)(this.ClientSize.Height * relHeight);
            int newXB = (int)(this.ClientSize.Width * relXB);
            int newYB = (int)(this.ClientSize.Height * relYB);

            button1.Size = new Size(newWidth, newHeight);
            button1.Location = new Point(newXB, newYB);
        }
        public string player1, player2, player3, player4;
        public void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 0&& textBox2.Text.Length != 0 && textBox3.Text.Length != 0 && textBox4.Text.Length != 0)
            {
                this.Hide();
                player1=textBox1.Text;
                player2=textBox2.Text;
                player3=textBox3.Text;
                player4=textBox4.Text;
                Tabla tabla = new Tabla(player1, player2, player3, player4);
                tabla.Show();
            }
            else
            {
                MessageBox.Show("Please make sure all the players chose a username");
            }
        }
    }
}
