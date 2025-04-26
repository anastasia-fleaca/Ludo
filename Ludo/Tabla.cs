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
        private Timer doubleClickTimer;

        private AfisareTabla graphicsManager;
        private bool isRolling = false; 
        private bool movePending = false; 
        private string[] playerNames;
        private int currentPlayerIndex = 0;
        private DateTime lastClickTime = DateTime.MinValue;
        private const int doubleClickThreshold = 1000;
        private List<PathSquare> pawnPath;
        public Tabla(string p1, string p2, string p3, string p4)
        {
            InitializeComponent();
            doubleClickTimer = new Timer();
            doubleClickTimer.Interval = doubleClickThreshold; // 500 ms
            doubleClickTimer.Tick += DoubleClickTimer_Tick;
            button1.EnabledChanged += button1_EnabledChanged;
            this.BackgroundImage = Image.FromFile("imagini/TABLA.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.ActiveControl = null;
            button1.BackColor = Color.White; // Or whatever background color you want
            button1.ForeColor = Color.Black;
            playerNames = new string[] { p1, p2, p3, p4 };

            diceAnimator = new AnimatieZar(timer1, pictureBox1, panel1);
            diceAnimator.InitializeUI(this);
            graphicsManager = new AfisareTabla(this, tableLayoutPanel1, tableLayoutPanel2, tableLayoutPanel3, tableLayoutPanel4, tableLayoutPanel5);
            diceAnimator.DiceRollCompleted += result =>
            {
                isRolling = false;
                button1.Enabled = false; // After rolling, disable the dice button

                string currentPlayerColor = GetCurrentPlayerColor();
                movePending = true;

                if (result == 6)
                {
                    EnablePawnSelection(currentPlayerColor);
                    // Wait for player to click a pawn
                }
                else
                {
                        movePending = false;
                        EndTurn(); // No moves possible, automatically end turn
                   
                }
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
        private string GetCurrentPlayerColor()
        {
            switch (currentPlayerIndex)
            {
                case 0: return "G"; 
                case 1: return "R"; 
                case 2: return "Y"; 
                case 3: return "B"; 
                default: return "G"; 
            }
        }
        private void EndTurn()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % playerNames.Length;
            HighlightCurrentPlayer();

            button1.Enabled = true; // Allow next player to roll dice
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

            if (isRolling)
            {
                if ((now - lastClickTime).TotalMilliseconds <= doubleClickThreshold)
                {
                    diceAnimator.SkipAnimation();
                    doubleClickTimer.Stop(); // If skipped, stop timer
                }
            }
            else
            {
                isRolling = true;
                movePending = false;
                button1.Enabled = true; // Keep enabled for possible double click
                diceAnimator.Start();
                doubleClickTimer.Start(); // Start waiting for possible double click
            }
            lastClickTime = now;
        }
        private void DoubleClickTimer_Tick(object sender, EventArgs e)
        {
            doubleClickTimer.Stop();
            if (isRolling)
            {
                button1.Enabled = false; // Disable after double click timeout
            }
        }


        private void EnablePawnSelection(string color)
        {
            if (graphicsManager.pawnsByColor.ContainsKey(color))
            {
                foreach (var pawn in graphicsManager.pawnsByColor[color])
                {
                    if (pawn.Parent != null && pawn.Parent.Parent is TableLayoutPanel) // Still at home
                    {
                        pawn.Cursor = Cursors.Hand;
                        pawn.Click += Pawn_Click;
                    }
                }
            }
        }

        private void DisablePawnSelection()
        {
            foreach (var colorPawns in graphicsManager.pawnsByColor.Values)
            {
                foreach (var pawn in colorPawns)
                {
                    pawn.Cursor = Cursors.Default;
                    pawn.Click -= Pawn_Click;
                }
            }
        }

        private void Pawn_Click(object sender, EventArgs e)
        {
            if (sender is PictureBox pawn)
            {
                MoveSelectedPawnToStart(pawn);
                DisablePawnSelection();
                movePending = false;
                EndTurn();
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

        private void MoveSelectedPawnToStart(PictureBox pawn)
        {
            string color = graphicsManager.GetPawnColor(pawn);

            if (string.IsNullOrEmpty(color))
                return;

            TableLayoutPanel targetPanel = null;
            int targetColumn = 0, targetRow = 0;

            switch (color.ToLower())
            {
                case "r":
                    targetPanel = tableLayoutPanel3;
                    targetColumn = 0;
                    targetRow = 4;
                    break;
                case "b":
                    targetPanel = tableLayoutPanel5;
                    targetColumn = 4;
                    targetRow = 2;
                    break;
                case "y":
                    targetPanel = tableLayoutPanel2;
                    targetColumn = 2;
                    targetRow = 1;
                    break;
                case "g":
                    targetPanel = tableLayoutPanel4;
                    targetColumn = 1;
                    targetRow = 0;
                    break;
                default:
                    return;
            }

            if (pawn.Parent != null)
                pawn.Parent.Controls.Remove(pawn);

            if (targetPanel != null)
            {
                Control container = targetPanel.GetControlFromPosition(targetColumn, targetRow);
                if (container is PictureBox backgroundPicture)
                {
                    backgroundPicture.Controls.Add(pawn);

                    pawn.Dock = DockStyle.None;
                    pawn.SizeMode = PictureBoxSizeMode.Zoom;
                    pawn.Size = new Size(30, 30);
                    pawn.Location = new Point(
                        (backgroundPicture.Width - pawn.Width) / 2,
                        (backgroundPicture.Height - pawn.Height) / 2
                    );
                    pawn.BringToFront();
                }
            }
        }

        private void Tabla_Load(object sender, EventArgs e)
        {
            graphicsManager.RepozitioneazaTabla();
            pawnPath = graphicsManager.GetPawnPath();
        }
    }
}

