using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Ludo
{
    internal class MutarePioni
    {
        private Tabla formParinte;
        private AfisareTabla managerGrafica;
        private Dictionary<PictureBox, List<Tabla.CasutaDrum>> drumuriPioni = new Dictionary<PictureBox, List<Tabla.CasutaDrum>>();
        private Dictionary<PictureBox, int> pozitiiPioni = new Dictionary<PictureBox, int>();
        private Dictionary<string, int> pioniFinalizati = new Dictionary<string, int>()
        {
            { "Y", 0 },
            { "B", 0 },
            { "R", 0 },
            { "G", 0 }
        };
        private List<string> castigatori = new List<string>();
        public bool AreTuraExtra { get; private set; } = false;
        public void ResetareTuraExtra() => AreTuraExtra = false;
        public MutarePioni(Tabla formular, AfisareTabla managerGrafica)
        {
            this.formParinte = formular;
            this.managerGrafica = managerGrafica;
        }
        public void MutarePionInainte(PictureBox pion, int pasi)
        {
            if (!drumuriPioni.ContainsKey(pion)) return;

            var drum = drumuriPioni[pion];
            int pozitieCurenta = pozitiiPioni[pion];
            int pozitieNoua = pozitieCurenta + pasi;

            if (pozitieNoua >= drum.Count - 1)
            {
                string culoare = managerGrafica.ObtineCuloarePiesa(pion);
                pozitiiPioni.Remove(pion);
                drumuriPioni.Remove(pion);
                managerGrafica.pioniColorati[culoare].Remove(pion);
                StergePionDePeTabla(pion);
                managerGrafica.AfiseazaPionSubEticheta(culoare);
                pioniFinalizati[culoare]++;
                if (pioniFinalizati[culoare] == 4 && !castigatori.Contains(culoare))
                    castigatori.Add(culoare);
                if (castigatori.Count == 3)
                {
                    string ramas = new[] { "G", "R", "Y", "B" }.FirstOrDefault(c => !castigatori.Contains(c));
                    if (ramas != null) castigatori.Add(ramas);
                    formParinte.AfiseazaClasament(castigatori);
                }

                AreTuraExtra = true;
                return;
            }

            var casutaDrum = drum[pozitieNoua];
            Control container = casutaDrum.Panel.GetControlFromPosition(casutaDrum.Column, casutaDrum.Row);
            string culoarePion = managerGrafica.ObtineCuloarePiesa(pion);

            var pioniExistenti = container.Controls.OfType<PictureBox>().ToList();
            var pioniOponenti = pioniExistenti.Where(p => managerGrafica.ObtineCuloarePiesa(p) != culoarePion).ToList();
            bool isSafe = casutaDrum.IsSafe;

            if (!isSafe && pioniOponenti.Count >= 2)
            {
                return;
            }
            if (pion.Parent != null)
            {
                var oldContainer = pion.Parent;
                oldContainer.Controls.Remove(pion);
                ReorganizeazaPioniInCasuta(oldContainer);
            }

            container.Controls.Add(pion);
            pozitiiPioni[pion] = pozitieNoua;
            ReorganizeazaPioniInCasuta(container);

            if (!isSafe && pioniOponenti.Count == 1)
            {
                var pionExistent = pioniOponenti[0];
                container.Controls.Remove(pionExistent);
                pozitiiPioni.Remove(pionExistent);
                drumuriPioni.Remove(pionExistent);
                AdaugaPionInCasa(pionExistent, managerGrafica.ObtineCuloarePiesa(pionExistent));
                ReorganizeazaPioniInCasuta(container);
                AreTuraExtra = true;
            }
        }

        public void MutarePionLaStart(PictureBox pion)
        {
            string culoare = managerGrafica.ObtineCuloarePiesa(pion);
            if (string.IsNullOrEmpty(culoare)) return;

            var drum = managerGrafica.ObtineDrumulCompletPentruPiesa(culoare);
            drumuriPioni[pion] = drum;

            var startIndex = managerGrafica.ObtinePozitiaStartPentruCuloare(culoare);
            var startSquare = managerGrafica.ObtineCaleaPionului()[startIndex];

            int indexInDrum = drum.FindIndex(sq =>
                sq.Panel == startSquare.Panel &&
                sq.Row == startSquare.Row &&
                sq.Column == startSquare.Column
            );

            if (indexInDrum == -1) return;

            Control container = startSquare.Panel.GetControlFromPosition(startSquare.Column, startSquare.Row);
            if (container == null) return;
            var pioniExistenti = container.Controls.OfType<PictureBox>().ToList();
            var pioniOponenti = pioniExistenti.Where(p => managerGrafica.ObtineCuloarePiesa(p) != culoare).ToList();

            if (pioniOponenti.Count >= 2)
            {
                return;
            }

            if (pion.Parent != null)
            {
                var oldContainer = pion.Parent;
                oldContainer.Controls.Remove(pion);
            }            
            container.Controls.Add(pion);
            pozitiiPioni[pion] = indexInDrum;
            ReorganizeazaPioniInCasuta(container);
            if (pioniOponenti.Count == 1)
            {
                var pionExistent = pioniOponenti[0];
                container.Controls.Remove(pionExistent);
                pozitiiPioni.Remove(pionExistent);
                drumuriPioni.Remove(pionExistent);
                AdaugaPionInCasa(pionExistent, managerGrafica.ObtineCuloarePiesa(pionExistent));
                ReorganizeazaPioniInCasuta(container);
                AreTuraExtra = true;
            }
        }
        public void AdaugaPionInCasa(PictureBox pion, string culoare)
        {
            if (!managerGrafica.pioniColorati.ContainsKey(culoare))
                managerGrafica.pioniColorati[culoare] = new List<PictureBox>();

            Control casa = managerGrafica.ObtineCasaPentruCuloare(culoare);

            List<Point> pozitiiDisponibile = new List<Point>
    {
        new Point(0, 0),
        new Point(1, 0),
        new Point(0, 1),
        new Point(1, 1)
    };

            int latimePatrat = casa.Width / 2;
            int inaltimePatrat = casa.Height / 2;
            int latimePiesa = casa.Width / 3;
            int inaltimePiesa = casa.Height / 3;

            foreach (Control control in casa.Controls)
            {
                if (control is PictureBox pb)
                {
                    int col = (pb.Location.X + pb.Width / 2) / latimePatrat;
                    int row = (pb.Location.Y + pb.Height / 2) / inaltimePatrat;
                    pozitiiDisponibile.Remove(new Point(col, row));
                }
            }

            if (pozitiiDisponibile.Count == 0) return;

            Point pozitie = pozitiiDisponibile[0];

            PictureBox pionNou = new PictureBox();
            pionNou.SizeMode = PictureBoxSizeMode.Zoom;
            pionNou.Image = Image.FromFile($"imagini/PAWN_{culoare[0]}.png");
            pionNou.BackColor = Color.Transparent;
            casa.Controls.Add(pionNou);

            pionNou.Size = new Size(latimePiesa, inaltimePiesa);
            pionNou.Location = new Point(
                pozitie.X * latimePatrat + (latimePatrat - latimePiesa) / 2,
                pozitie.Y * inaltimePatrat + (inaltimePatrat - inaltimePiesa) / 2
            );
            managerGrafica.pioniColorati[culoare].Add(pionNou);
            if (pion.Parent != null)
            {
                var oldContainer = pion.Parent;
                oldContainer.Controls.Remove(pion);
                ReorganizeazaPioniInCasuta(oldContainer);
            }

        }
        public void VerificaSiTrimitePionInCasa(PictureBox pion, string culoareDestinatie)
        {
            string culoarePionCurent = managerGrafica.ObtineCuloarePiesa(pion);

            if (!drumuriPioni.ContainsKey(pion)) return;

            var drum = drumuriPioni[pion];
            int pozitieCurenta = pozitiiPioni[pion];

            if (pozitieCurenta == drum.Count - 1)
            {
                return;
            }

            Control container = (pion.Parent as TableLayoutPanel)?.GetControlFromPosition(1, 1);

            if (container is PictureBox && container.Controls.Contains(pion))
            {
                return;
            }

            if (culoarePionCurent != culoareDestinatie)
            {
                if (pion.Parent != null)
                {
                    pion.Parent.Controls.Remove(pion);
                }

                if (pozitiiPioni.ContainsKey(pion))
                {
                    pozitiiPioni.Remove(pion);
                }

                if (drumuriPioni.ContainsKey(pion))
                {
                    drumuriPioni.Remove(pion);
                }

                AdaugaPionInCasa(pion, culoarePionCurent);
            }
            AreTuraExtra = true;
        }
        public List<PictureBox> PioniMutabili(string culoare, int rezultatZar)
        {
            List<PictureBox> mutabili = new List<PictureBox>();

            if (!managerGrafica.pioniColorati.ContainsKey(culoare))
                return mutabili;

            foreach (var pion in managerGrafica.pioniColorati[culoare])
            {
                if (!pozitiiPioni.ContainsKey(pion))
                {
                    if (rezultatZar == 6)
                    {
                        var startIndex = managerGrafica.ObtinePozitiaStartPentruCuloare(culoare);
                        var startSquare = managerGrafica.ObtineCaleaPionului()[startIndex];
                        Control container = startSquare.Panel.GetControlFromPosition(startSquare.Column, startSquare.Row);

                        var pioniExistenti = container.Controls.OfType<PictureBox>().ToList();
                        var pioniOponenti = pioniExistenti.Where(p => managerGrafica.ObtineCuloarePiesa(p) != culoare).ToList();

                        if (pioniOponenti.Count <= 1)
                        {
                            mutabili.Add(pion);
                        }
                    }
                }
                else
                {
                    var drum = drumuriPioni[pion];
                    int indexCurent = pozitiiPioni[pion];
                    int indexDestinatie = indexCurent + rezultatZar;

                    if (indexDestinatie < drum.Count)
                    {
                        var casutaDrum = drum[indexDestinatie];
                        Control container = casutaDrum.Panel.GetControlFromPosition(casutaDrum.Column, casutaDrum.Row);
                        bool isSafe = casutaDrum.IsSafe;

                        var pioniExistenti = container.Controls.OfType<PictureBox>().ToList();
                        var pioniOponenti = pioniExistenti.Where(p => managerGrafica.ObtineCuloarePiesa(p) != culoare).ToList();

                        if (pioniOponenti.Count <= 1||isSafe)
                        {
                            mutabili.Add(pion);
                        }
                    }
                    else if (indexDestinatie == drum.Count - 1) 
                    {
                        mutabili.Add(pion);
                    }
                }
            }

            return mutabili;
        }
        public bool ArePioniInCasa(string culoare)
        {
            return managerGrafica.pioniColorati[culoare]
                   .Any(p => !pozitiiPioni.ContainsKey(p));
        }

        public void StergePionDePeTabla(PictureBox pion)
        {
            if (pion.Parent != null)
            {
                pion.Parent.Controls.Remove(pion);
            }
            pion.Dispose();
        }
        public void ProceseazaClickPion(PictureBox pion, int pasi)
        {
            string culoare = managerGrafica.ObtineCuloarePiesa(pion);
            bool areMutari = false;

            foreach (var p in managerGrafica.pioniColorati[culoare])
            {
                if (!pozitiiPioni.ContainsKey(p))
                {
                    if (pasi == 6)
                    {
                        var drum = managerGrafica.ObtineDrumulCompletPentruPiesa(culoare);
                        var casutaStart = managerGrafica.ObtineCaleaPionului()[managerGrafica.ObtinePozitiaStartPentruCuloare(culoare)];
                        int indexStart = drum.FindIndex(sq =>
                            sq.Panel == casutaStart.Panel &&
                            sq.Row == casutaStart.Row &&
                            sq.Column == casutaStart.Column
                        );

                        if (indexStart != -1)
                        {
                            var container = casutaStart.Panel.GetControlFromPosition(casutaStart.Column, casutaStart.Row);
                            PictureBox pionPeStart = formParinte.ObtinePionPeCasuta(container);

                            if (pionPeStart == null || managerGrafica.ObtineCuloarePiesa(pionPeStart) != culoare)
                            {
                                areMutari = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    var drum = drumuriPioni[p];
                    int poz = pozitiiPioni[p];
                    if (poz + pasi < drum.Count)
                    {
                        areMutari = true;
                        break;
                    }
                }
            }

            if (!areMutari)
            {
                return;
            }

            if (pasi == 6 && !pozitiiPioni.ContainsKey(pion))
            {
                MutarePionLaStart(pion);
            }
            else if (pozitiiPioni.ContainsKey(pion))
            {
                var drum = drumuriPioni[pion];
                int indexCurent = pozitiiPioni[pion];
                int indexDestinatie = indexCurent + pasi + 1;

                if (indexDestinatie >= drum.Count)
                {
                    MutarePionInainte(pion, pasi);

                    if (pioniFinalizati[culoare] == 4 && !castigatori.Contains(culoare))
                    {
                        castigatori.Add(culoare);
                        if (castigatori.Count == 3)
                        {
                            string ramas = new[] { "G", "R", "Y", "B" }
                                .FirstOrDefault(c => !castigatori.Contains(c));
                            if (ramas != null)
                                castigatori.Add(ramas);

                            formParinte.AfiseazaClasament(castigatori);
                        }
                    }
                    AreTuraExtra = true; 
                }
                else
                {
                    MutarePionInainte(pion, pasi);
                }
            }
        }
        public void PreiaPionulExistent(PictureBox pionNou, Tabla.CasutaDrum casutaDrum)
        {
            PictureBox pionExistent = formParinte.ObtinePionPeCasuta(casutaDrum.Panel.GetControlFromPosition(casutaDrum.Column, casutaDrum.Row) as Control);

            if (pionExistent != null)
            {
                AdaugaPionInCasa(pionExistent, managerGrafica.ObtineCuloarePiesa(pionExistent));
            }

            Control container = casutaDrum.Panel.GetControlFromPosition(casutaDrum.Column, casutaDrum.Row);
            if (container is PictureBox pictureBackground)
            {
                pictureBackground.Controls.Add(pionNou);
                pionNou.Dock = DockStyle.None;
                pionNou.SizeMode = PictureBoxSizeMode.Zoom;
                pionNou.Size = new Size(30, 30);
                pionNou.Location = new Point(
                    (pictureBackground.Width - pionNou.Width) / 2,
                    (pictureBackground.Height - pionNou.Height) / 2
                );
                pionNou.BringToFront();
            }
        }
        public void ReorganizeazaPioniInCasuta(Control container)
        {
            var pioni = container.Controls.OfType<PictureBox>().ToList();
            int count = pioni.Count;

            if (count == 0) return;

            int cols = (int)Math.Ceiling(Math.Sqrt(count));
            int rows = (int)Math.Ceiling((double)count / cols);

            int cellWidth = container.Width / cols;
            int cellHeight = container.Height / rows;
            int size = Math.Min(cellWidth, cellHeight) - 4;

            Size dimensiunePion = new Size(size, size);
            Point[] pozitii = new Point[count];

            for (int i = 0; i < count; i++)
            {
                int row = i / cols;
                int col = i % cols;
                int x = col * cellWidth + (cellWidth - size) / 2;
                int y = row * cellHeight + (cellHeight - size) / 2;
                pozitii[i] = new Point(x, y);
            }

            for (int i = 0; i < count; i++)
            {
                var pion = pioni[i];
                pion.Size = dimensiunePion;
                pion.Location = pozitii[i];
                pion.BringToFront();
            }
        }

        public Dictionary<PictureBox, int> ObținePozitiilePioni() => pozitiiPioni;
        public Dictionary<PictureBox, List<Tabla.CasutaDrum>> ObțineDrumurilePioni() => drumuriPioni;
        public List<string> ObțineCastigatori() => castigatori;
        public Dictionary<string, int> ObținePioniFinalizați() => pioniFinalizati;
    }
}
