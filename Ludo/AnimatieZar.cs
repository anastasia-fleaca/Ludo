using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Ludo
{
    internal class AnimatieZar
    {
        private readonly Random generator = new Random();
        private readonly Timer cronometru;
        private readonly PictureBox casetaImagine;
        private readonly Panel panouParinte;

        private List<(int valoare, Image imagine)> feteZar;
        private int pasAruncare;
        private int pasiMaximi;
        private int intarziereAruncare;

        public int UltimaValoareAruncata { get; private set; }
        public event Action<int> AruncareZarCompletata;

        public AnimatieZar(Timer cronometru, PictureBox casetaImagine, Panel panouParinte)
        {
            this.cronometru = cronometru;
            this.casetaImagine = casetaImagine;
            this.panouParinte = panouParinte;

            IncarcaFeteleZarului();
            this.cronometru.Tick += Cronometru_Tick;
        }

        private void IncarcaFeteleZarului()
        {
            feteZar = new List<(int, Image)>
            {
                (1, Image.FromFile("imagini/ZAR_1.png")),
                (2, Image.FromFile("imagini/ZAR_2.png")),
                (3, Image.FromFile("imagini/ZAR_3.png")),
                (4, Image.FromFile("imagini/ZAR_4.png")),
                (5, Image.FromFile("imagini/ZAR_5.png")),
                (6, Image.FromFile("imagini/ZAR_6.png"))
            };
        }

        public void InitializeazaUI(Form parinte)
        {
            panouParinte.Parent = parinte;
            casetaImagine.BringToFront();
            SeteazaPozitiaPanouRelativ(parinte);
        }

        public void SeteazaPozitiaPanouRelativ(Control parinte)
        {
            float relX = 0.625f;
            float relY = 0.42f;
            int nouX = (int)(parinte.ClientSize.Width * relX);
            int nouY = (int)(parinte.ClientSize.Height * relY);
            panouParinte.Location = new Point(nouX, nouY);

            float relLatime = 0.21f;
            int nouaLatime = (int)(parinte.ClientSize.Width * relLatime);
            panouParinte.Size = new Size(nouaLatime, nouaLatime);
        }

        public void Start()
        {
            pasAruncare = 0;
            pasiMaximi = generator.Next(30, 50);
            intarziereAruncare = 30;
            cronometru.Interval = intarziereAruncare;
            AmestecaFetele();
            cronometru.Start();
        }

        private void Cronometru_Tick(object sender, EventArgs e)
        {
            MutareCasetaImagineAleatorie();
            SchimbaFataZaruluiAleatorie();

            pasAruncare++;

            if (pasAruncare > pasiMaximi * 0.5)
                cronometru.Interval += 10;
            if (pasAruncare > pasiMaximi * 0.8)
                cronometru.Interval += 10;

            if (pasAruncare >= pasiMaximi)
            {
                cronometru.Stop();
                ArataFataFinalaAzarului();
            }
        }

        private void MutareCasetaImagineAleatorie()
        {
            int maxX = panouParinte.ClientSize.Width - casetaImagine.Width;
            int maxY = panouParinte.ClientSize.Height - casetaImagine.Height;

            int x = generator.Next(0, Math.Max(1, maxX));
            int y = generator.Next(0, Math.Max(1, maxY));
            casetaImagine.Location = new Point(x, y);
        }

        private void SchimbaFataZaruluiAleatorie()
        {
            int indiceFata = generator.Next(feteZar.Count);
            var (valoare, imagine) = feteZar[indiceFata];
            casetaImagine.Image = imagine;
        }

        private void ArataFataFinalaAzarului()
        {
            cronometru.Stop();

            foreach (var (valoare, imagine) in feteZar)
            {
                if (casetaImagine.Image == imagine)
                {
                    UltimaValoareAruncata = valoare;
                    AruncareZarCompletata?.Invoke(UltimaValoareAruncata);
                    break;
                }
            }
        }

        private void AmestecaFetele()
        {
            feteZar = feteZar.OrderBy(_ => generator.Next()).ToList();
        }

        public void SkipAnimatie()
        {
            cronometru.Stop();
            ArataFataFinalaAzarului();
        }
    }
}
