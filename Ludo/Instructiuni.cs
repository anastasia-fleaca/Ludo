using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ludo
{
    public partial class Instructiuni : Form
    {
        public Instructiuni()
        {
            InitializeComponent();
            this.Text = "Instrucțiuni Ludo";
            this.Size = new Size(500, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.BackColor = Color.FromArgb(255, 243, 228);
            Panel scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20),
                BackColor = Color.Snow
            };
            this.Controls.Add(scrollPanel);

            int yOffset = 10;

            void AddLabel(string text, bool bold = false, int extraSpace = 20, Color? color = null)
            {
                Label label = new Label
                {
                    Text = text,
                    Font = new Font("Arial Black", bold ? 12F : 10F, bold ? FontStyle.Bold : FontStyle.Regular),
                    AutoSize = true,
                    MaximumSize = new Size(440, 0),
                    Location = new Point(10, yOffset),
                    ForeColor = color ?? Color.Black 
                };

                scrollPanel.Controls.Add(label);
                yOffset += label.Height + extraSpace;
            }
            AddLabel("🎲 INSTRUCȚIUNI JOC LUDO 🎲", true, 45, Color.FromArgb(90, 55, 49));
            AddLabel("🔹 Scopul jocului:", true, 20 ,Color.FromArgb(163, 43, 9));
            AddLabel("Fiecare jucător trebuie să își aducă cele 4 piese în zona finală (de aceeași culoare), parcurgând traseul complet de pe tablă.", false, 20, Color.FromArgb(163, 43, 9));

            AddLabel("🔹 Cum se joacă:", true, 20, Color.FromArgb(160, 119, 0));
            AddLabel("1. Fiecare jucător aruncă zarul pe rând.", false, 20, Color.FromArgb(160, 119, 0));
            AddLabel("2. Pentru a scoate o piesă din bază, trebuie să obțină un 6.", false, 20, Color.FromArgb(160, 119, 0));
            AddLabel("3. La o aruncare de 6, jucătorul are dreptul la o nouă aruncare.", false, 20, Color.FromArgb(160, 119, 0));
            AddLabel("4. Piesele se deplasează în sensul acelor de ceasornic.", false, 20, Color.FromArgb(160, 119, 0));
            AddLabel("5. Dacă o piesă ajunge pe o căsuță ocupată de o piesă adversă, aceasta este eliminată și revine în bază.", false, 20, Color.FromArgb(160, 119, 0));

            AddLabel("🔹 Câștigătorul:", true, 20, Color.FromArgb(60, 78, 37));
            AddLabel("Primul jucător care aduce toate cele 4 piese în zona sa finală câștigă jocul.",false, 20, Color.FromArgb(60, 78, 37));

            AddLabel("💡 Sfat:", true, 20, Color.FromArgb(43, 41, 97));
            AddLabel("Gândește strategic. Uneori e mai bine să blochezi o piesă adversă decât să avansezi prea repede.", false, 20, Color.FromArgb(43, 41, 97));

            AddLabel("Distracție plăcută și mult noroc! 🎉", true, 10, Color.FromArgb(90, 55, 49));
            AddLabel(" ", true, 10);
        }
    }
}
