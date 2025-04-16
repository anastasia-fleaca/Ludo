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
        private AnimatieZar diceAnimator;
        AfisareTabla graphicsManager;

        public Tabla(string p1, string p2, string p3, string p4)
        {
            InitializeComponent();
            this.BackgroundImage = Image.FromFile("imagini/TABLA.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.ActiveControl = null;
            diceAnimator = new AnimatieZar(timer1, pictureBox1, panel1);
            diceAnimator.InitializeUI(this);
            graphicsManager = new AfisareTabla(this, tableLayoutPanel1, tableLayoutPanel2, tableLayoutPanel3, tableLayoutPanel4, tableLayoutPanel5);
            graphicsManager.DeseneazaTabla();
            graphicsManager.PozitioneazaLabeluriSiButon(label1, label2, label3, label4, button1, p1, p2, p3, p4);

        }
        private void ButtonRollDice_Click(object sender, EventArgs e)
        {
            diceAnimator.Start();
            this.ActiveControl = null;
        }
        private void Tabla_Load(object sender, EventArgs e)
        {
            graphicsManager.RepozitioneazaTabla();
        }
    }
}
