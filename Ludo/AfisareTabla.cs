using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Ludo.Tabla;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Ludo
{
    public class AfisareTabla
    {
        private Form parinte;
        private TableLayoutPanel[] panouri;
        public Dictionary<string, List<PictureBox>> pioniColorati = new Dictionary<string, List<PictureBox>>();
        private Dictionary<string, List<PictureBox>> pioniEtichete = new Dictionary<string, List<PictureBox>>();
        public AfisareTabla(Form formular, params TableLayoutPanel[] panouriTabla)
        {
            parinte = formular;
            panouri = panouriTabla;
        }

        public void DeseneazaTabla()
        {
            AdaugaImagineFundal(1, 1, "imagini/CASA.png", 1);
            AdaugaImagineFundal(0, 0, "imagini/CASA_G.png", 1);
            AdaugaImagineFundal(2, 0, "imagini/CASA_Y.png", 1);
            AdaugaImagineFundal(0, 2, "imagini/CASA_R.png", 1);
            AdaugaImagineFundal(2, 2, "imagini/CASA_B.png", 1);
            AdaugaImagineFundal(1, 1, "imagini/YELLOW.png", 2);
            AdaugaImagineFundal(1, 2, "imagini/YELLOW.png", 2);
            AdaugaImagineFundal(2, 1, "imagini/STAR_Y.png", 2);
            AdaugaImagineFundal(1, 4, "imagini/YELLOW.png", 2);
            AdaugaImagineFundal(1, 3, "imagini/YELLOW.png", 2);
            AdaugaImagineFundal(1, 5, "imagini/YELLOW.png", 2);
            AdaugaImagineFundal(4, 2, "imagini/STAR_B.png", 5);
            AdaugaImagineFundal(0, 1, "imagini/BLUE.png", 5);
            AdaugaImagineFundal(1, 1, "imagini/BLUE.png", 5);
            AdaugaImagineFundal(2, 1, "imagini/BLUE.png", 5);
            AdaugaImagineFundal(3, 1, "imagini/BLUE.png", 5);
            AdaugaImagineFundal(4, 1, "imagini/BLUE.png", 5);
            AdaugaImagineFundal(1, 1, "imagini/GREEN.png", 4);
            AdaugaImagineFundal(2, 1, "imagini/GREEN.png", 4);
            AdaugaImagineFundal(3, 1, "imagini/GREEN.png", 4);
            AdaugaImagineFundal(4, 1, "imagini/GREEN.png", 4);
            AdaugaImagineFundal(5, 1, "imagini/GREEN.png", 4);
            AdaugaImagineFundal(1, 0, "imagini/STAR_G.png", 4);
            AdaugaImagineFundal(1, 1, "imagini/RED.png", 3);
            AdaugaImagineFundal(1, 2, "imagini/RED.png", 3);
            AdaugaImagineFundal(1, 3, "imagini/RED.png", 3);
            AdaugaImagineFundal(1, 4, "imagini/RED.png", 3);
            AdaugaImagineFundal(1, 0, "imagini/RED.png", 3);
            AdaugaImagineFundal(0, 4, "imagini/STAR_R.png", 3);
            AdaugaImagineFundal(1, 5, "imagini/ARROW_R.png", 3);
            AdaugaImagineFundal(5, 1, "imagini/ARROW_B.png", 5);
            AdaugaImagineFundal(0, 1, "imagini/ARROW_G.png", 4);
            AdaugaImagineFundal(1, 0, "imagini/ARROW_Y.png", 2);
            AdaugaImagineFundal(0, 2, "imagini/STAR.png", 2);
            AdaugaImagineFundal(3, 0, "imagini/STAR.png", 5);
            AdaugaImagineFundal(2, 2, "imagini/STAR.png", 4);
            AdaugaImagineFundal(2, 3, "imagini/STAR.png", 3);

            for (int i = 1; i < 5; i++)
            {
                TableLayoutPanel panou = panouri[i];
                for (int rand = 0; rand < panou.RowCount; rand++)
                {
                    for (int coloana = 0; coloana < panou.ColumnCount; coloana++)
                    {
                        Control c = panou.GetControlFromPosition(coloana, rand);
                        if (c == null)
                        {
                            AdaugaImagineFundal(coloana, rand, "imagini/WHITE.png", i + 1);
                        }
                    }
                }
            }
        }
        public List<CasutaDrum> ObtineCaleaPionului()
        {
            var cale = new List<CasutaDrum>();
            var p2 = panouri[1];
            var p5 = panouri[4];
            var p3 = panouri[2];
            var p4 = panouri[3];
            cale.Add(new CasutaDrum(p2, 5, 0));
            cale.Add(new CasutaDrum(p2, 4, 0));
            cale.Add(new CasutaDrum(p2, 3, 0));
            cale.Add(new CasutaDrum(p2, 2, 0, "", true));
            cale.Add(new CasutaDrum(p2, 1, 0));

            cale.Add(new CasutaDrum(p2, 0, 0));
            cale.Add(new CasutaDrum(p2, 0, 1));
            cale.Add(new CasutaDrum(p2, 0, 2));
            cale.Add(new CasutaDrum(p2, 1, 2, "Y"));
            cale.Add(new CasutaDrum(p2, 2, 2));
            cale.Add(new CasutaDrum(p2, 3, 2));
            cale.Add(new CasutaDrum(p2, 4, 2));
            cale.Add(new CasutaDrum(p2, 5, 2));
            cale.Add(new CasutaDrum(p5, 0, 0));
            cale.Add(new CasutaDrum(p5, 0, 1));
            cale.Add(new CasutaDrum(p5, 0, 2));
            cale.Add(new CasutaDrum(p5, 0, 3, "", true));
            cale.Add(new CasutaDrum(p5, 0, 4));
            cale.Add(new CasutaDrum(p5, 0, 5));
            cale.Add(new CasutaDrum(p5, 1, 5));
            cale.Add(new CasutaDrum(p5, 2, 5));
            cale.Add(new CasutaDrum(p5, 2, 4, "B"));
            cale.Add(new CasutaDrum(p5, 2, 3));
            cale.Add(new CasutaDrum(p5, 2, 2));
            cale.Add(new CasutaDrum(p5, 2, 1));
            cale.Add(new CasutaDrum(p5, 2, 0));
            cale.Add(new CasutaDrum(p3, 0, 2));
            cale.Add(new CasutaDrum(p3, 1, 2));
            cale.Add(new CasutaDrum(p3, 2, 2));
            cale.Add(new CasutaDrum(p3, 3, 2, "", true));
            cale.Add(new CasutaDrum(p3, 4, 2));
            cale.Add(new CasutaDrum(p3, 5, 2));
            cale.Add(new CasutaDrum(p3, 5, 1));
            cale.Add(new CasutaDrum(p3, 5, 0));
            cale.Add(new CasutaDrum(p3, 4, 0, "R"));
            cale.Add(new CasutaDrum(p3, 3, 0));
            cale.Add(new CasutaDrum(p3, 2, 0));
            cale.Add(new CasutaDrum(p3, 1, 0));
            cale.Add(new CasutaDrum(p3, 0, 0));
            cale.Add(new CasutaDrum(p4, 2, 5));
            cale.Add(new CasutaDrum(p4, 2, 4));
            cale.Add(new CasutaDrum(p4, 2, 3));
            cale.Add(new CasutaDrum(p4, 2, 2, "", true));
            cale.Add(new CasutaDrum(p4, 2, 1));
            cale.Add(new CasutaDrum(p4, 2, 0));
            cale.Add(new CasutaDrum(p4, 1, 0));
            cale.Add(new CasutaDrum(p4, 0, 0));
            cale.Add(new CasutaDrum(p4, 0, 1, "G"));
            cale.Add(new CasutaDrum(p4, 0, 2));
            cale.Add(new CasutaDrum(p4, 0, 3));
            cale.Add(new CasutaDrum(p4, 0, 4));
            cale.Add(new CasutaDrum(p4, 0, 5));
            return cale;
        }
        public int ObtinePozitiaStartPentruCuloare(string culoare)
        {
            switch (culoare.ToUpper())
            {
                case "Y": return 8;
                case "B": return 21;
                case "R": return 34;
                case "G": return 47;
                default: return 0;
            }
        }
        public List<CasutaDrum> ObtineCaleaFinalaPentruCuloare(string culoare)
        {
            var caleFinala = new List<CasutaDrum>();

            switch (culoare.ToUpper())
            {
                case "Y": 
                    caleFinala.Add(new CasutaDrum(panouri[1], 1, 1));
                    caleFinala.Add(new CasutaDrum(panouri[1], 2, 1));
                    caleFinala.Add(new CasutaDrum(panouri[1], 3, 1));
                    caleFinala.Add(new CasutaDrum(panouri[1], 4, 1));
                    caleFinala.Add(new CasutaDrum(panouri[1], 5, 1));
                    caleFinala.Add(new CasutaDrum(panouri[0], 1, 1));
                    break;

                case "B": 
                    caleFinala.Add(new CasutaDrum(panouri[4], 1, 4));
                    caleFinala.Add(new CasutaDrum(panouri[4], 1, 3));
                    caleFinala.Add(new CasutaDrum(panouri[4], 1, 2));
                    caleFinala.Add(new CasutaDrum(panouri[4], 1, 1));
                    caleFinala.Add(new CasutaDrum(panouri[4], 1, 0));
                    caleFinala.Add(new CasutaDrum(panouri[0], 1, 1));
                    break;

                case "R": 
                    caleFinala.Add(new CasutaDrum(panouri[2], 4, 1));
                    caleFinala.Add(new CasutaDrum(panouri[2], 3, 1));
                    caleFinala.Add(new CasutaDrum(panouri[2], 2, 1));
                    caleFinala.Add(new CasutaDrum(panouri[2], 1, 1));
                    caleFinala.Add(new CasutaDrum(panouri[2], 0, 1));
                    caleFinala.Add(new CasutaDrum(panouri[0], 1, 1));
                    break;

                case "G": 
                    caleFinala.Add(new CasutaDrum(panouri[3], 1, 1));
                    caleFinala.Add(new CasutaDrum(panouri[3], 1, 2));
                    caleFinala.Add(new CasutaDrum(panouri[3], 1, 3));
                    caleFinala.Add(new CasutaDrum(panouri[3], 1, 4));
                    caleFinala.Add(new CasutaDrum(panouri[3], 1, 5));
                    caleFinala.Add(new CasutaDrum(panouri[0], 1, 1));
                    break;
            }
            return caleFinala;
        }
        public List<CasutaDrum> ObtineDrumulCompletPentruPiesa(string culoare)
        {
            var drumComplet = new List<CasutaDrum>();
            var drumDeBaza = ObtineCaleaPionului();
            int indexStart = ObtinePozitiaStartPentruCuloare(culoare);
            for (int i = 0; i < 51; i++)
            {
                int index = (indexStart + i) % drumDeBaza.Count;
                drumComplet.Add(drumDeBaza[index]);
            }
            drumComplet.AddRange(ObtineCaleaFinalaPentruCuloare(culoare));

            return drumComplet;
        }

        public void AdaugaImagineFundal(int coloana, int rand, string fisier, int indexPanel)
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Margin = new Padding(0);
            pictureBox.Image = Image.FromFile(fisier);

            pictureBox.Paint += delegate (object sender, PaintEventArgs e)
            {
                Pen pen = new Pen(Color.FromArgb(90, 55, 49), 2);
                e.Graphics.DrawRectangle(pen, 1, 1, pictureBox.Width - 2, pictureBox.Height - 2);
            };

            if (fisier.Contains("CASA_"))
            {
                char c = fisier[fisier.Length - 5];
                Adauga4ChildPictureBox(pictureBox, c);
            }

            panouri[indexPanel - 1].Controls.Add(pictureBox, coloana, rand);
        }

        public void Adauga4ChildPictureBox(PictureBox parinte, char caracterCuloare)
        {
            string culoare = caracterCuloare.ToString();

            if (!pioniColorati.ContainsKey(culoare))
                pioniColorati[culoare] = new List<PictureBox>();

            for (int i = 0; i < 4; i++)
            {
                PictureBox piesa = new PictureBox();
                piesa.SizeMode = PictureBoxSizeMode.Zoom;

                string caleImagine = $"imagini/PAWN_{caracterCuloare}.png";
                piesa.Image = Image.FromFile(caleImagine);
                piesa.Image.Tag = caleImagine; 

                piesa.BackColor = Color.Transparent;
                parinte.Controls.Add(piesa);
                pioniColorati[culoare].Add(piesa);
            }

            parinte.Resize += delegate (object sender, EventArgs e)
            {
                for (int i = 0; i < 4; i++)
                {
                    PictureBox piesa = (PictureBox)parinte.Controls[i];
                    int col = i % 2;
                    int rand = i / 2;
                    int latimePatrat = parinte.Width / 2;
                    int inaltimePatrat = parinte.Height / 2;
                    int latimePiesa = parinte.Width / 3;
                    int inaltimePiesa = parinte.Height / 3;
                    piesa.Size = new Size(latimePiesa, inaltimePiesa);
                    piesa.Location = new Point(col * latimePatrat + (latimePatrat - latimePiesa) / 2, rand * inaltimePatrat + (inaltimePatrat - inaltimePiesa) / 2);
                }
            };
        }


        public void RepoziționeazăTabla()
        {
            float dimensiuneRelativa = 0.69f;
            float pozitieYRelativa = 0.2f;

            int latimeMaxima = (int)(parinte.ClientSize.Width * dimensiuneRelativa);
            int inaltimeMaxima = (int)(parinte.ClientSize.Height * dimensiuneRelativa);

            int patrat = Math.Min(latimeMaxima, inaltimeMaxima);

            int x = (parinte.ClientSize.Width - patrat) / 2;
            int y = (int)(parinte.ClientSize.Height * pozitieYRelativa);

            panouri[0].Size = new Size(patrat, patrat);
            panouri[0].Location = new Point(x, y);
        }

        public void PozitioneazaEticheteSiButon(Label eticheta1, Label eticheta2, Label eticheta3, Label eticheta4, Button buton1, string p1, string p2, string p3, string p4)
        {
            float relX1 = 0.12f;
            float relY1 = 0.22f;
            float relX2 = 1.4f;
            float relY2 = 1f;

            int newX1 = (int)(parinte.ClientSize.Width * relX1);
            int newY1 = (int)(parinte.ClientSize.Height * relY1);
            int newX2 = (int)(parinte.ClientSize.Width * relX2);
            int newY2 = (int)(parinte.ClientSize.Height * relY2);

            eticheta1.Location = new Point(newX1, newY1);
            eticheta2.Location = new Point(newX1, newY2);
            eticheta3.Location = new Point(newX2, newY1);
            eticheta4.Location = new Point(newX2, newY2);

            eticheta1.Text = "★" + p1;
            eticheta2.Text = "★" + p2;
            eticheta3.Text = "★" + p3;
            eticheta4.Text = "★" + p4;

            float relX4 = 0.62f;
            float relY4 = 0.5f;
            int newX4 = (int)(parinte.ClientSize.Width * relX4);
            int newY4 = (int)(parinte.ClientSize.Height * relY4);

            buton1.Location = new Point(newX4, newY4);

            float latimeButon = 0.23f;
            float inaltimeButon = 0.1f;
            int newW = (int)(parinte.ClientSize.Width * latimeButon);
            int newH = (int)(parinte.ClientSize.Width * inaltimeButon);
            buton1.Size = new Size(newW, newH);
        }
        public Control ObtineCasaPentruCuloare(string culoare)
        {
            TableLayoutPanel tabel = panouri[0];

            switch (culoare.ToUpper())
            {
                case "Y":
                    return tabel.GetControlFromPosition(2, 0);
                case "B":
                    return tabel.GetControlFromPosition(2, 2);
                case "R":
                    return tabel.GetControlFromPosition(0, 2);
                case "G":
                    return tabel.GetControlFromPosition(0, 0);
                default:
                    return null;
            }
        }
        public void InitializeazaPioniEtichete(Label eticheta, string culoare)
        {
            var lista = new List<PictureBox>();
            for (int i = 0; i < 4; i++)
            {
                PictureBox pawn = new PictureBox();
                pawn.SizeMode = PictureBoxSizeMode.Zoom;
                pawn.Size = new Size(40, 40);
                pawn.Image = Image.FromFile($"imagini/PAWN_{culoare}.png");
                pawn.Visible = false;

                int spacing = 50; 
                int x = eticheta.Left + i * spacing;
                int y = eticheta.Bottom + 10;
                pawn.Location = new Point(x, y);

                parinte.Controls.Add(pawn);
                lista.Add(pawn);
            }

            pioniEtichete[culoare.ToUpper()] = lista;
        }
        public void AfiseazaPionSubEticheta(string culoare)
        {
            culoare = culoare.ToUpper();
            if (!pioniEtichete.ContainsKey(culoare)) return;

            var lista = pioniEtichete[culoare];
            foreach (var pawn in lista)
            {
                if (!pawn.Visible)
                {
                    pawn.Visible = true;
                    break;
                }
            }
        }
        public string ObtineCuloarePiesa(PictureBox piesa)
        {
            foreach (var kvp in pioniColorati)
            {
                if (kvp.Value.Contains(piesa))
                    return kvp.Key;
            }
            return null;
        }

    }
}
