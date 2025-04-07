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
    public partial class Tabla : Form
    {
        public Tabla()
        {
            InitializeComponent();
            this.BackgroundImage = Image.FromFile("imagini/TABLA.png");
            this.BackgroundImageLayout = ImageLayout.Zoom;
            this.WindowState = FormWindowState.Maximized;
            this.ActiveControl = null;
            DeseneazaTabla();
        }
        private void DeseneazaTabla()
        {
            AddBackgroundImage(1, 1, "imagini/CASA.png",1);
            AddBackgroundImage(0, 0, "imagini/CASA_G.png",1);
            AddBackgroundImage(2, 0, "imagini/CASA_Y.png",1);
            AddBackgroundImage(0, 2, "imagini/CASA_R.png",1);
            AddBackgroundImage(2, 2, "imagini/CASA_B.png",1);
            AddBackgroundImage(1, 1, "imagini/YELLOW.png", 2);
            AddBackgroundImage(1, 2, "imagini/YELLOW.png", 2); 
            AddBackgroundImage(2, 1, "imagini/YELLOW.png", 2);
            AddBackgroundImage(1, 4, "imagini/YELLOW.png", 2);
            AddBackgroundImage(1, 3, "imagini/YELLOW.png", 2);
            AddBackgroundImage(1, 5, "imagini/YELLOW.png", 2);
            AddBackgroundImage(4, 2, "imagini/BLUE.png", 5);
            AddBackgroundImage(0, 1, "imagini/BLUE.png", 5);
            AddBackgroundImage(1, 1, "imagini/BLUE.png", 5);
            AddBackgroundImage(2, 1, "imagini/BLUE.png", 5);
            AddBackgroundImage(3, 1, "imagini/BLUE.png", 5);
            AddBackgroundImage(4, 1, "imagini/BLUE.png", 5);
            AddBackgroundImage(1, 1, "imagini/GREEN.png", 4);
            AddBackgroundImage(2, 1, "imagini/GREEN.png", 4);
            AddBackgroundImage(3, 1, "imagini/GREEN.png", 4);
            AddBackgroundImage(4, 1, "imagini/GREEN.png", 4);
            AddBackgroundImage(5, 1, "imagini/GREEN.png", 4);
            AddBackgroundImage(1, 0, "imagini/GREEN.png", 4);
            AddBackgroundImage(1, 1, "imagini/RED.png", 3);
            AddBackgroundImage(1, 2, "imagini/RED.png", 3);
            AddBackgroundImage(1, 3, "imagini/RED.png", 3);
            AddBackgroundImage(1, 4, "imagini/RED.png", 3);
            AddBackgroundImage(1, 0, "imagini/RED.png", 3);
            AddBackgroundImage(0, 4, "imagini/RED.png", 3);
        }
        private void AddBackgroundImage(int column, int row, string poza, int i)
        {
            PictureBox casa = new PictureBox();
            casa.Dock = DockStyle.Fill;
            casa.SizeMode = PictureBoxSizeMode.Zoom;
            casa.Margin = new Padding(0);
            casa.Paint += (s, e) =>
            {
                Color borderColor = Color.FromArgb(5a3731); 
                int thickness = 2; 
                using (Pen p = new Pen(borderColor, thickness))
                {
                    e.Graphics.DrawRectangle(p, thickness / 2, thickness / 2,
                        casa.Width - thickness, casa.Height - thickness);
                }
            };
            casa.Image = Image.FromFile(poza);
            switch (i)
            {
                case 1:
                    tableLayoutPanel1.Controls.Add(casa, column, row);
                    break;
                case 2:
                    tableLayoutPanel2.Controls.Add(casa, column, row);
                    break;
                case 3:
                    tableLayoutPanel3.Controls.Add(casa, column, row);
                    break;
                case 4:
                    tableLayoutPanel4.Controls.Add(casa, column, row);
                    break;
                case 5:
                    tableLayoutPanel5.Controls.Add(casa, column, row);
                    break;
            }
        }
        private void RepozitioneazaTabla()
        {
            float relSize = 0.69f;
            float relY = 0.2f;
            int maxWidth = (int)(this.ClientSize.Width * relSize);
            int maxHeight = (int)(this.ClientSize.Height * relSize);
            int squareSize = Math.Min(maxWidth, maxHeight);
            int newX = (this.ClientSize.Width - squareSize) / 2;
            int newY = (int)(this.ClientSize.Height * relY);
            tableLayoutPanel1.Size = new Size(squareSize, squareSize);
            tableLayoutPanel1.Location = new Point(newX, newY);
        }

        private void Tabla_Load(object sender, EventArgs e)
        {
            RepozitioneazaTabla();
        }

        private void Tabla_Resize(object sender, EventArgs e)
        {
            RepozitioneazaTabla();
        }
    }
}
