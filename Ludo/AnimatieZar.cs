using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Ludo
{
    internal class AnimatieZar
    {
        private readonly Random rng = new Random();
        private readonly Timer timer;
        private readonly PictureBox pictureBox;
        private readonly Panel parentPanel;

        private List<(int value, Image image)> diceFaces;
        private int rollStep;
        private int maxSteps;
        private int rollDelay;

        public int LastRolledValue { get; private set; }
        public event Action<int> DiceRollCompleted;

        public AnimatieZar(Timer timer, PictureBox pictureBox, Panel parentPanel)
        {
            this.timer = timer;
            this.pictureBox = pictureBox;
            this.parentPanel = parentPanel;

            LoadDiceFaces();
            this.timer.Tick += Timer_Tick;
        }

        private void LoadDiceFaces()
        {
            diceFaces = new List<(int, Image)>
            {
                (1, Image.FromFile("imagini/ZAR_1.png")),
                (2, Image.FromFile("imagini/ZAR_2.png")),
                (3, Image.FromFile("imagini/ZAR_3.png")),
                (4, Image.FromFile("imagini/ZAR_4.png")),
                (5, Image.FromFile("imagini/ZAR_5.png")),
                (6, Image.FromFile("imagini/ZAR_6.png"))
            };
        }

        public void InitializeUI(Form parent)
        {
            parentPanel.Parent = parent;
            pictureBox.BringToFront();
            SetPanelPositionRelativeTo(parent);
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
            MovePictureBoxRandomly();
            ChangeDiceFaceRandomly();

            rollStep++;

            if (rollStep > maxSteps * 0.5)
                timer.Interval += 10;
            if (rollStep > maxSteps * 0.8)
                timer.Interval += 10;

            if (rollStep >= maxSteps)
            {
                timer.Stop();
                ShowFinalDiceFace();
            }
        }

        private void MovePictureBoxRandomly()
        {
            int maxX = parentPanel.ClientSize.Width - pictureBox.Width;
            int maxY = parentPanel.ClientSize.Height - pictureBox.Height;

            int x = rng.Next(0, Math.Max(1, maxX));
            int y = rng.Next(0, Math.Max(1, maxY));
            pictureBox.Location = new Point(x, y);
        }

        private void ChangeDiceFaceRandomly()
        {
            int faceIndex = rng.Next(diceFaces.Count);
            var (value, image) = diceFaces[faceIndex];
            pictureBox.Image = image;
        }

        private void ShowFinalDiceFace()
        {
            timer.Stop();

            // Find the image and get the value
            foreach (var (value, image) in diceFaces)
            {
                if (pictureBox.Image == image)
                {
                    LastRolledValue = value;
                    DiceRollCompleted?.Invoke(LastRolledValue);
                    break;
                }
            }
        }

        private void ShuffleFaces()
        {
            diceFaces = diceFaces.OrderBy(_ => rng.Next()).ToList();
        }
        public void SkipAnimation()
        {
            timer.Stop();
            ShowFinalDiceFace(); 
        }
    }
}
