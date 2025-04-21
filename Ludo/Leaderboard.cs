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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Ludo
{
    public partial class Leaderboard : Form
    {
        public Leaderboard(string p1, string p2, string p3, string p4)
        {
            InitializeComponent();
            this.BackgroundImage = Image.FromFile("imagini/PODIUM.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.ActiveControl = null;
            float relX1 = 0.36f;
            float relX2 = 1f;
            float relX3 = 1.55f;
            float relX4 = 2.05f;
            float relY1 = 0.94f;
            float relY2 = 0.92f;
            float relY3 = 0.915f;
            float relY4 = 0.67f;
            int newX1 = (int)(this.ClientSize.Width * relX1);
            int newX2 = (int)(this.ClientSize.Width * relX2);
            int newX3 = (int)(this.ClientSize.Width * relX3);
            int newX4 = (int)(this.ClientSize.Width * relX4);
            int newY1 = (int)(this.ClientSize.Width * relY1);
            int newY2 = (int)(this.ClientSize.Width * relY2);
            int newY3 = (int)(this.ClientSize.Width * relY3);
            int newY4 = (int)(this.ClientSize.Width * relY4);
            label1.Location = new Point(newX1, newY1);
            label2.Location = new Point(newX2, newY2);
            label3.Location = new Point(newX3, newY3);
            label4.Location = new Point(newX4, newY4);
            label1.Text = p1;
            label2.Text = p2;
            label3.Text = p3;
            label4.Text = p4;
            label1.Font = new Font("Franklin Gothic Heavy", 30, FontStyle.Bold); 
            label2.Font = new Font("Franklin Gothic Heavy", 26, FontStyle.Bold); 
            label3.Font = new Font("Franklin Gothic Heavy", 22, FontStyle.Bold);
            label4.Font = new Font("Franklin Gothic Heavy", 18, FontStyle.Bold); 
        }
    }
}
