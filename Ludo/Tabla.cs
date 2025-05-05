using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Ludo
{
    public partial class Tabla : Form
    {
        private AnimatieZar animatorZar;
        private Timer cronometruDoubleClick;
        private MutarePioni mutariPioni;
        private AfisareTabla managerGrafica;

        private bool esteRotireZar = false;
        private bool mutareInAsteptare = false;
        private string[] numeJucatori;
        private int indexJucatorCurent = 0;
        private DateTime ultimaOraClick = DateTime.MinValue;
        private const int pragDoubleClick = 1000;

        public Tabla(string p1, string p2, string p3, string p4)
        {
            InitializeComponent();
            cronometruDoubleClick = new Timer();
            cronometruDoubleClick.Interval = pragDoubleClick;
            cronometruDoubleClick.Tick += CronometruDoubleClick_Tick;
            button1.EnabledChanged += button1_EnabledChanged;

            this.BackgroundImage = Image.FromFile("imagini/TABLA.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.ActiveControl = null;

            button1.BackColor = Color.White;
            button1.ForeColor = Color.Black;
            numeJucatori = new string[] { p1, p2, p3, p4 };

            animatorZar = new AnimatieZar(timer1, pictureBox1, panel1);
            animatorZar.InitializeazaUI(this);
            managerGrafica = new AfisareTabla(this, tableLayoutPanel1, tableLayoutPanel2, tableLayoutPanel3, tableLayoutPanel4, tableLayoutPanel5);
            mutariPioni = new MutarePioni(this, managerGrafica);

            animatorZar.AruncareZarCompletata += rezultat =>
            {
                esteRotireZar = false;
                button1.Enabled = false;

                string culoareJucatorCurent = ObtineCuloareJucatorCurent();
                var pioniMutabili = mutariPioni.PioniMutabili(culoareJucatorCurent, rezultat);

                if (pioniMutabili.Count > 0)
                {
                    ActiveazaSelectareaPionilor(pioniMutabili);
                    mutareInAsteptare = true;
                    button1.Enabled = false;
                }
                else
                {
                    string culoare = ObtineCuloareJucatorCurent();
                    bool arePioniAcasa = mutariPioni.ArePioniInCasa(culoare);
                    var mutabili = mutariPioni.PioniMutabili(culoare, rezultat);

                    if (rezultat == 6 && (mutabili.Count > 0 || arePioniAcasa))
                    {
                        button1.Enabled = true;
                    }
                    else
                    {
                        button1.Enabled = false;
                        IncheieTura();
                    }
                }
            };

            managerGrafica.DeseneazaTabla();
            managerGrafica.PozitioneazaEticheteSiButon(label1, label2, label3, label4, button1, p1, p2, p3, p4);
            EvidentiazaJucatorulCurent();
        }

        public class CasutaDrum
        {
            public TableLayoutPanel Panel { get; set; }
            public int Row { get; set; }
            public int Column { get; set; }
            public string Culoare { get; set; }

            public CasutaDrum(TableLayoutPanel panel, int row, int column, string culoare = "")
            {
                Panel = panel;
                Row = row;
                Column = column;
                Culoare = culoare;
            }
        }

        public void AfiseazaClasament(List<string> castigatori)
        {
            var clasament = new Leaderboard(castigatori[0], castigatori[1], castigatori[2], castigatori[3]);
            clasament.Show();
            this.Close();
        }

        private string ObtineCuloareJucatorCurent()
        {
            switch (indexJucatorCurent)
            {
                case 0: return "G";
                case 1: return "R";
                case 2: return "Y";
                case 3: return "B";
                default: return "G";
            }
        }

        private void IncheieTura()
        {
            indexJucatorCurent = (indexJucatorCurent + 1) % numeJucatori.Length;
            EvidentiazaJucatorulCurent();
            button1.Enabled = true;
        }

        private void EvidentiazaJucatorulCurent()
        {
            label1.Font = new Font(label1.Font.FontFamily, 28, FontStyle.Regular);
            label2.Font = new Font(label2.Font.FontFamily, 28, FontStyle.Regular);
            label3.Font = new Font(label3.Font.FontFamily, 28, FontStyle.Regular);
            label4.Font = new Font(label4.Font.FontFamily, 28, FontStyle.Regular);

            switch (indexJucatorCurent)
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

        private void ButtonZar_Click(object sender, EventArgs e)
        {
            DateTime acum = DateTime.Now;

            if (esteRotireZar)
            {
                if ((acum - ultimaOraClick).TotalMilliseconds <= pragDoubleClick)
                {
                    animatorZar.SkipAnimatie();
                    cronometruDoubleClick.Stop();
                }
            }
            else
            {
                esteRotireZar = true;
                mutareInAsteptare = false;
                button1.Enabled = true;
                animatorZar.Start();
                cronometruDoubleClick.Start();
            }
            ultimaOraClick = acum;
        }

        private void CronometruDoubleClick_Tick(object sender, EventArgs e)
        {
            cronometruDoubleClick.Stop();
            if (esteRotireZar)
            {
                button1.Enabled = false;
            }
        }

        private void ActiveazaSelectareaPionilor(List<PictureBox> pioniMutabili)
        {
            foreach (var pion in pioniMutabili)
            {
                pion.Cursor = Cursors.Hand;
                pion.Click += Pion_Click;
            }
        }

        private void DezactiveazaSelectareaPionilor()
        {
            foreach (var pioniCuloare in managerGrafica.pioniColorati.Values)
            {
                foreach (var pion in pioniCuloare)
                {
                    pion.Cursor = Cursors.Default;
                    pion.Click -= Pion_Click;
                }
            }
        }

        private void Pion_Click(object sender, EventArgs e)
        {
            if (sender is PictureBox pion)
            {
                int pasi = animatorZar.UltimaValoareAruncata;
                mutariPioni.ProceseazaClickPion(pion, pasi);

                DezactiveazaSelectareaPionilor();
                mutareInAsteptare = false;

                if (pasi == 6)
                {
                    string culoare = ObtineCuloareJucatorCurent();
                    bool areMutari = mutariPioni.PioniMutabili(culoare, 6).Count > 0 || mutariPioni.ArePioniInCasa(culoare);
                    button1.Enabled = areMutari;

                    if (!areMutari) IncheieTura();
                }
                else
                {
                    IncheieTura();
                }
            }
        }

        private void button1_EnabledChanged(object sender, EventArgs e)
        {
            if (button1.Enabled)
            {
                button1.ForeColor = Color.Black;
            }
            else
            {
                button1.ForeColor = Color.FromArgb(220, 220, 220);
            }
        }

        private void Tabla_Load(object sender, EventArgs e)
        {
            managerGrafica.RepoziționeazăTabla();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            IncheieTura();
        }
    }
}
