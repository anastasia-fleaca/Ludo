using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ludo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.BackgroundImage = Image.FromFile("imagini/LUDO.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.WindowState = FormWindowState.Maximized;
            this.ActiveControl = null;
            button1.TabStop = false;
            this.TopMost = true;
        }
        private void RepozitioneazaButon()
        {
            float relX = 0.11f;
            float relY = 0.56f;
            float relWidth = 0.25f;
            float relHeight = 0.13f;

            int newWidth = (int)(this.ClientSize.Width * relWidth);
            int newHeight = (int)(this.ClientSize.Height * relHeight);
            int newX = (int)(this.ClientSize.Width * relX);
            int newY = (int)(this.ClientSize.Height * relY);

            button1.Size = new Size(newWidth, newHeight);
            button1.Location = new Point(newX, newY);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            RepozitioneazaButon();
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            RepozitioneazaButon();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
