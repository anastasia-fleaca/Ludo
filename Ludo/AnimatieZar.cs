using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ludo
{
    internal class AnimatieZar
    {
        private Random rng = new Random();
        private Timer timer;
        private PictureBox pictureBox;
        private Panel parentPanel;
        private List<Image> diceFaces;
        private int rollStep;
        private int maxSteps;
        private int rollDelay;
        public event Action<int> DiceRollCompleted;

        public AnimatieZar(Timer timer, PictureBox pictureBox, Panel parentPanel)
        {
            this.timer = timer;
            this.pictureBox = pictureBox;
            this.parentPanel = parentPanel;
            this.diceFaces = new List<Image>
    {
        Image.FromFile("imagini/ZAR_1.png"),
        Image.FromFile("imagini/ZAR_2.png"),
        Image.FromFile("imagini/ZAR_3.png"),
        Image.FromFile("imagini/ZAR_4.png"),
        Image.FromFile("imagini/ZAR_5.png"),
        Image.FromFile("imagini/ZAR_6.png")
    };

            this.timer.Tick += Timer_Tick;
        }
        public void SetPanelPositionRelativeTo(Control parent)
        {
            float relX = 1.6f;
            float relY = 0.41f;
            int newX = (int)(parent.ClientSize.Width * relX);
            int newY = (int)(parent.ClientSize.Height * relY);
            parentPanel.Location = new Point(newX, newY);

            float relWidth = 0.23f;
            int newWidth = (int)(parent.ClientSize.Width * relWidth);
            parentPanel.Size = new Size(newWidth, newWidth);
        }
        public void InitializeUI(Form parent)
        {
            pictureBox.BringToFront();
            parentPanel.Parent = parent;
            SetPanelPositionRelativeTo(parent);
        }

        public void Start()
        {
            rollStep = 0;
            maxSteps = rng.Next(30, 50);
            rollDelay = 30;
            timer.Interval = rollDelay;
            ShuffleFaces();
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int maxX = parentPanel.ClientSize.Width - pictureBox.Width;
            int maxY = parentPanel.ClientSize.Height - pictureBox.Height;

            int x = rng.Next(0, Math.Max(1, maxX));
            int y = rng.Next(0, Math.Max(1, maxY));
            pictureBox.Location = new Point(x, y);

            int faceIndex = rng.Next(6);
            pictureBox.Image = diceFaces[faceIndex];

            rollStep++;

            if (rollStep > maxSteps * 0.5)
                timer.Interval += 10;
            if (rollStep > maxSteps * 0.8)
                timer.Interval += 10;

            if (rollStep >= maxSteps)
            {
                timer.Stop();
                int finalFace = rng.Next(6);
                pictureBox.Image = diceFaces[finalFace];
                DiceRollCompleted?.Invoke(finalFace);
            }
        }

        private void ShuffleFaces()
        {
            diceFaces = diceFaces.OrderBy(x => rng.Next()).ToList();
        }
    }
}
