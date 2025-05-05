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

            if (pozitieNoua == drum.Count)
            {
                string culoare = managerGrafica.ObtineCuloarePiesa(pion);
                StergePionDePeTablă(pion);

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
                return;
            }
            else if (pozitieNoua > drum.Count)
            {
                return;
            }

            var casutaDrum = drum[pozitieNoua];

            if (pion.Parent != null)
            {
                pion.Parent.Controls.Remove(pion);
            }

            Control container = casutaDrum.Panel.GetControlFromPosition(casutaDrum.Column, casutaDrum.Row);
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
            {
                return;
            }

            var casutaDrum = drum[indexActual];

            if (pion.Parent != null)
                pion.Parent.Controls.Remove(pion);

            Control container = casutaDrum.Panel.GetControlFromPosition(casutaDrum.Column, casutaDrum.Row);
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

        public void StergePionDePeTablă(PictureBox pion)
        {
            if (pion.Parent != null)
                pion.Parent.Controls.Remove(pion);

            pion.Dispose();
        }

        public void ProceseazaClickPion(PictureBox pion, int pasi)
        {
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
                    string culoare = managerGrafica.ObtineCuloarePiesa(pion);
                    pioniFinalizati[culoare]++;

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

        public Dictionary<PictureBox, int> ObținePozitiilePioni() => pozitiiPioni;
        public Dictionary<PictureBox, List<Tabla.CasutaDrum>> ObțineDrumurilePioni() => drumuriPioni;
        public List<string> ObțineCastigatori() => castigatori;
        public Dictionary<string, int> ObținePioniFinalizați() => pioniFinalizati;
    }
}
