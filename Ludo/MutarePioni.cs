using System;
using System.Collections.Generic;
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

            if (pozitieNoua >= drum.Count)
            {
                string culoare = managerGrafica.ObtineCuloarePiesa(pion);
                StergePionDePeTabla(pion);

                if (pioniFinalizati.ContainsKey(culoare))
                {
                    pioniFinalizati[culoare]++;
                    if (castigatori.Count == 3)
                    {
                        string ramas = new[] { "G", "R", "Y", "B" }
                            .FirstOrDefault(c => !castigatori.Contains(c));

                        if (ramas != null)
                            castigatori.Add(ramas);

                        formParinte.AfiseazaClasament(castigatori);
                    }
                }

                pozitiiPioni.Remove(pion);
                drumuriPioni.Remove(pion);
                Control finalSquareContainer = drum[drum.Count - 1].Panel.GetControlFromPosition(drum[drum.Count - 1].Column, drum[drum.Count - 1].Row);
                if (finalSquareContainer is PictureBox finalSquareBackground)
                {
                    finalSquareBackground.Controls.Add(pion);
                    pion.Dock = DockStyle.None;
                    pion.SizeMode = PictureBoxSizeMode.Zoom;
                    pion.Size = new Size(30, 30);
                    pion.Location = new Point(
                        (finalSquareBackground.Width - pion.Width) / 2,
                        (finalSquareBackground.Height - pion.Height) / 2
                    );
                    pion.BringToFront();
                }

                return;
            }

            var casutaDrum = drum[pozitieNoua];

            if (pion.Parent != null)
            {
                pion.Parent.Controls.Remove(pion);
            }

            Control containerDrum = casutaDrum.Panel.GetControlFromPosition(casutaDrum.Column, casutaDrum.Row);
            PictureBox pionPeCasuta = formParinte.ObtinePionPeCasuta(containerDrum);

            if (pionPeCasuta != null)
            {
                VerificaSiTrimitePionInCasa(pionPeCasuta, managerGrafica.ObtineCuloarePiesa(pion));
            }

            if (containerDrum is PictureBox newPictureBackground)
            {
                newPictureBackground.Controls.Add(pion);
                pion.Dock = DockStyle.None;
                pion.SizeMode = PictureBoxSizeMode.Zoom;
                pion.Size = new Size(30, 30);
                pion.Location = new Point(
                    (newPictureBackground.Width - pion.Width) / 2,
                    (newPictureBackground.Height - pion.Height) / 2
                );
                pion.BringToFront();
            }

            pozitiiPioni[pion] = pozitieNoua;
        }

        public void MutarePionLaStart(PictureBox pion)
        {
            string culoare = managerGrafica.ObtineCuloarePiesa(pion);
            if (string.IsNullOrEmpty(culoare))
                return;

            var drum = managerGrafica.ObtineDrumulCompletPentruPiesa(culoare);
            drumuriPioni[pion] = drum;

            var casutaStart = managerGrafica.ObtineCaleaPionului()[managerGrafica.ObtinePozitiaStartPentruCuloare(culoare)];

            int indexActual = drum.FindIndex(sq =>
                sq.Panel == casutaStart.Panel &&
                sq.Row == casutaStart.Row &&
                sq.Column == casutaStart.Column
            );

            if (indexActual == -1)
                return;

            var casutaDrum = drum[indexActual];

            if (pion.Parent != null)
                pion.Parent.Controls.Remove(pion);

            Control container = casutaDrum.Panel.GetControlFromPosition(casutaDrum.Column, casutaDrum.Row);

            if (container == null)
                return;

            PictureBox pionPeCasuta = formParinte.ObtinePionPeCasuta(container);

            if (pionPeCasuta != null)
            {
                string culoarePionPeCasuta = managerGrafica.ObtineCuloarePiesa(pionPeCasuta);

                if (culoarePionPeCasuta != culoare)
                {
                    AdaugaPionInCasa(pionPeCasuta, culoarePionPeCasuta);
                    pozitiiPioni.Remove(pionPeCasuta);
                    drumuriPioni.Remove(pionPeCasuta);
                    pionPeCasuta.Dispose();
                }
            }

            if (container is PictureBox pictureBackground)
            {
                pictureBackground.Controls.Add(pion);
                pion.Dock = DockStyle.None;
                pion.SizeMode = PictureBoxSizeMode.Zoom;
                pion.Size = new Size(30, 30);
                pion.Location = new Point(
                    (pictureBackground.Width - pion.Width) / 2,
                    (pictureBackground.Height - pion.Height) / 2
                );
                pion.BringToFront();

                pozitiiPioni[pion] = indexActual;
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
        }

        public List<PictureBox> PioniMutabili(string culoare, int rezultatZar)
        {
            List<PictureBox> mutabili = new List<PictureBox>();

            if (managerGrafica.pioniColorati.ContainsKey(culoare))
            {
                foreach (var pion in managerGrafica.pioniColorati[culoare])
                {
                    if (!pozitiiPioni.ContainsKey(pion))
                    {
                        if (rezultatZar == 6)
                            mutabili.Add(pion);
                    }
                    else
                    {
                        var drum = drumuriPioni[pion];
                        int indexCurent = pozitiiPioni[pion];
                        int indexDestinatie = indexCurent + rezultatZar + 1;

                        if (indexDestinatie <= drum.Count)
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

            if (!pozitiiPioni.ContainsKey(pion) && pasi == 6)
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
        public Dictionary<PictureBox, int> ObținePozitiilePioni() => pozitiiPioni;
        public Dictionary<PictureBox, List<Tabla.CasutaDrum>> ObțineDrumurilePioni() => drumuriPioni;
        public List<string> ObțineCastigatori() => castigatori;
        public Dictionary<string, int> ObținePioniFinalizați() => pioniFinalizati;
    }
}
