using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Ludo
{
    public partial class Tabla : Form
    {
        private AnimatieZar diceAnimator;
        private AfisareTabla graphicsManager;

        private string[] playerNames;
        private int currentPlayerIndex = 0;
        private DateTime lastClickTime = DateTime.MinValue;
        private const int doubleClickThreshold = 500;
        private List<PathSquare> pawnPath;
        public Tabla(string p1, string p2, string p3, string p4)
        {
            InitializeComponent();

            this.BackgroundImage = Image.FromFile("imagini/TABLA.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.ActiveControl = null;

            playerNames = new string[] { p1, p2, p3, p4 };

            diceAnimator = new AnimatieZar(timer1, pictureBox1, panel1);
            diceAnimator.InitializeUI(this);
            graphicsManager = new AfisareTabla(this, tableLayoutPanel1, tableLayoutPanel2, tableLayoutPanel3, tableLayoutPanel4, tableLayoutPanel5);

            diceAnimator.DiceRollCompleted += result =>
            {
                EndTurn();
            };
            graphicsManager.DeseneazaTabla();
            graphicsManager.PozitioneazaLabeluriSiButon(label1, label2, label3, label4, button1, p1, p2, p3, p4);

            HighlightCurrentPlayer();
        }
        public class PathSquare
        {
            public TableLayoutPanel Panel { get; set; }
            public int Row { get; set; }
            public int Column { get; set; }

            public PathSquare(TableLayoutPanel panel, int row, int column)
            {
                Panel = panel;
                Row = row;
                Column = column;
            }
        }

        private void EndTurn()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % playerNames.Length;
            HighlightCurrentPlayer();
        }

        private void HighlightCurrentPlayer()
        {
            label1.Font = new Font(label1.Font.FontFamily, 28, FontStyle.Regular);
            label2.Font = new Font(label2.Font.FontFamily, 28, FontStyle.Regular);
            label3.Font = new Font(label3.Font.FontFamily, 28, FontStyle.Regular);
            label4.Font = new Font(label4.Font.FontFamily, 28, FontStyle.Regular);

            switch (currentPlayerIndex)
            {
                case 0:
                    label1.Font = new Font(label1.Font.FontFamily, 35, FontStyle.Underline);
                    break;
                case 1:
                    label2.Font = new Font(label2.Font.FontFamily, 35, FontStyle.Underline);
                    break;
                case 2:
                    label3.Font = new Font(label3.Font.FontFamily, 35, FontStyle.Underline);
                    break;
                case 3:
                    label4.Font = new Font(label4.Font.FontFamily, 35, FontStyle.Underline);
                    break;
            }
        }

        private void ButtonRollDice_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            if ((now - lastClickTime).TotalMilliseconds <= doubleClickThreshold)
            {
                diceAnimator.SkipAnimation();
            }
            else
            {
                diceAnimator.Start();
            }
            lastClickTime = now;
        }


        private void Tabla_Load(object sender, EventArgs e)
        {
            graphicsManager.RepozitioneazaTabla();
            pawnPath = graphicsManager.GetPawnPath();
        }
    }
}

