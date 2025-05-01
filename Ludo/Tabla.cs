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
        private Dictionary<PictureBox, List<PathSquare>> pawnPaths = new Dictionary<PictureBox, List<PathSquare>>();

        private Dictionary<PictureBox, int> pawnPositions = new Dictionary<PictureBox, int>();
        Dictionary<string, int> pawnsFinished = new Dictionary<string, int>()
{
    { "Y", 0 },
    { "B", 0 },
    { "R", 0 },
    { "G", 0 }
};
        List<string> winners = new List<string>(); // e.g., [ "R", "Y", "G", "B" ]

        public Tabla(string p1, string p2, string p3, string p4)
        {
            InitializeComponent();
            doubleClickTimer = new Timer();
            doubleClickTimer.Interval = doubleClickThreshold;
            doubleClickTimer.Tick += DoubleClickTimer_Tick;
            button1.EnabledChanged += button1_EnabledChanged;
            this.BackgroundImage = Image.FromFile("imagini/TABLA.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.ActiveControl = null;
            button1.BackColor = Color.White;
            button1.ForeColor = Color.Black;
            playerNames = new string[] { p1, p2, p3, p4 };

            diceAnimator = new AnimatieZar(timer1, pictureBox1, panel1);
            diceAnimator.InitializeUI(this);
            graphicsManager = new AfisareTabla(this, tableLayoutPanel1, tableLayoutPanel2, tableLayoutPanel3, tableLayoutPanel4, tableLayoutPanel5);
            diceAnimator.DiceRollCompleted += result =>
            {
                isRolling = false;
                button1.Enabled = false; // Always disable initially

                string currentPlayerColor = GetCurrentPlayerColor();
                var movablePawns = GetMovablePawns(currentPlayerColor, result);

                if (movablePawns.Count > 0)
                {
                    EnablePawnSelection(movablePawns);
                    movePending = true;

                    // Keep button disabled while selecting pawn
                    button1.Enabled = false;
                }
               
                    // Only enable for another roll if it's a 6 with no moves
                    else
                    {
                        string color = GetCurrentPlayerColor();
                        bool hasHomePawns = HasPawnsInHome(color);
                        var movable = GetMovablePawns(color, result);

                        if (result == 6 && (movable.Count > 0 || hasHomePawns))
                        {
                            button1.Enabled = true;
                        }
                        else
                        {
                            button1.Enabled = false;
                            EndTurn();
                        }
                    }

                
            };

            graphicsManager.DeseneazaTabla();
            graphicsManager.PozitioneazaLabeluriSiButon(label1, label2, label3, label4, button1, p1, p2, p3, p4);

            HighlightCurrentPlayer();
        }
        private void RemovePawnFromBoard(PictureBox pawn)
        {
            if (pawn.Parent != null)
                pawn.Parent.Controls.Remove(pawn);

            pawn.Dispose();
        }

        private List<PictureBox> GetMovablePawns(string color, int diceResult)
        {
            List<PictureBox> movable = new List<PictureBox>();

            if (graphicsManager.pawnsByColor.ContainsKey(color))
            {
                foreach (var pawn in graphicsManager.pawnsByColor[color])
                {
                    if (!pawnPositions.ContainsKey(pawn))
                    {
                        if (diceResult == 6)
                            movable.Add(pawn);
                    }
                    else
                    {
                        var path = pawnPaths[pawn];
                        int currentIndex = pawnPositions[pawn];
                        int destinationIndex = currentIndex + diceResult + 1;

                        // Only allow if destination is within or exactly reaching home
                        if (destinationIndex <= path.Count)
                            movable.Add(pawn);
                    }
                }
            }

            return movable;
        }



        private void EnablePawnSelection(List<PictureBox> movablePawns)
        {
            foreach (var pawn in movablePawns)
            {
                pawn.Cursor = Cursors.Hand;
                pawn.Click += Pawn_Click;
            }
        }
        private void MovePawnForward(PictureBox pawn, int steps)
        {
            if (!pawnPaths.ContainsKey(pawn)) return;

            var path = pawnPaths[pawn];
            int currentPos = pawnPositions[pawn];
            int newPos = currentPos + steps;
            if (newPos == path.Count)
            {
                // Pawn finishes the game
                string color = graphicsManager.GetPawnColor(pawn);
                RemovePawnFromBoard(pawn);

                if (pawnsFinished.ContainsKey(color))
                {
                    pawnsFinished[color]++;
                    Console.WriteLine($"{color} has {pawnsFinished[color]} pawns finished.");

                    if (winners.Count == 3)
                    {
                        // Find the last remaining player
                        string remaining = new[] { "G", "R", "Y", "B" }
                            .FirstOrDefault(c => !winners.Contains(c));

                        if (remaining != null)
                            winners.Add(remaining); // Add 4th as last by default

                        // Open leaderboard form with the 4 final positions
                        var leaderboard = new Leaderboard(winners[0], winners[1], winners[2], winners[3]);
                        leaderboard.Show();
                        this.Close();
                    }

                }

                // Remove tracking
                pawnPositions.Remove(pawn);
                pawnPaths.Remove(pawn);
                return;
            }
            else if (newPos > path.Count)
            {
                // Oversteps final square — not a valid move
                Console.WriteLine("Invalid move: would go beyond the final square.");
                return;
            }


            var pathSquare = path[newPos];


            if (pawn.Parent != null)
            {
                pawn.Parent.Controls.Remove(pawn);
            }

            Control container = pathSquare.Panel.GetControlFromPosition(pathSquare.Column, pathSquare.Row);
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

            pawnPositions[pawn] = newPos;
        }

        public class PathSquare
        {
            public TableLayoutPanel Panel { get; set; }
            public int Row { get; set; }
            public int Column { get; set; }
            public string Color { get; set; }

            public PathSquare(TableLayoutPanel panel, int row, int column, string color = "")
            {
                Panel = panel;
                Row = row;
                Column = column;
                Color = color;
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

            button1.Enabled = true;
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
                    doubleClickTimer.Stop();
                }
            }
            else
            {
                isRolling = true;
                movePending = false;
                button1.Enabled = true;
                diceAnimator.Start();
                doubleClickTimer.Start();
            }
            lastClickTime = now;
        }

        private void DoubleClickTimer_Tick(object sender, EventArgs e)
        {
            doubleClickTimer.Stop();
            if (isRolling)
            {
                button1.Enabled = false;
            }
        }

        private void EnablePawnSelection(string color)
        {
            if (graphicsManager.pawnsByColor.ContainsKey(color))
            {
                foreach (var pawn in graphicsManager.pawnsByColor[color])
                {
                    if (pawn.Parent != null && pawn.Parent.Parent is TableLayoutPanel)
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
                int steps = diceAnimator.LastRolledValue;

                if (!pawnPositions.ContainsKey(pawn) && steps == 6)
                {
                    MoveSelectedPawnToStart(pawn);
                }
                else if (pawnPositions.ContainsKey(pawn))
                {
                    var path = pawnPaths[pawn];
                    int currentIndex = pawnPositions[pawn];
                    int destinationIndex = currentIndex + steps + 1;

                    if (destinationIndex >= path.Count)
                    {
                        // Pawn reaches the house
                        RemovePawnFromBoard(pawn);
                        pawnPositions.Remove(pawn);

                        string color = GetCurrentPlayerColor();
                        pawnsFinished[color]++;

                        // Check if player has finished all 4
                        if (pawnsFinished[color] == 4 && !winners.Contains(color))
                        {
                            winners.Add(color);
                            Console.WriteLine($"{color} is winner #{winners.Count}");

                            if (winners.Count == 3)
                            {
                                // Add remaining player as 4th
                                string remaining = new[] { "G", "R", "Y", "B" }
                                    .FirstOrDefault(c => !winners.Contains(c));
                                if (remaining != null)
                                    winners.Add(remaining);

                                var leaderboard = new Leaderboard(winners[0], winners[1], winners[2], winners[3]);
                                leaderboard.Show();
                                button1.Enabled = false;
                                return;
                            }
                        }
                    }
                    else
                    {
                        MovePawnForward(pawn, steps);
                    }
                }


                DisablePawnSelection();
                movePending = false;

                // Handle 6-roll special case
                if (steps == 6)
                {
                    // Only enable if there are possible moves
                    string color = GetCurrentPlayerColor();
                    bool hasMoves = GetMovablePawns(color, 6).Count > 0 || HasPawnsInHome(color);
                    button1.Enabled = hasMoves;

                    if (!hasMoves) EndTurn();
                }
                else
                {
                    EndTurn();
                }
            }
        }
        private bool HasPawnsInHome(string color)
        {
            return graphicsManager.pawnsByColor[color]
                   .Any(p => !pawnPositions.ContainsKey(p));
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

            var path = graphicsManager.GetFullPathForPawn(color);
            pawnPaths[pawn] = path;

            // Get the start square's coordinates
            PathSquare startSquare = graphicsManager.GetPawnPath()[graphicsManager.GetStartPositionForColor(color)];

            // Find the index of the square in the full path that matches the start square location
            int actualIndex = path.FindIndex(sq =>
                sq.Panel == startSquare.Panel &&
                sq.Row == startSquare.Row &&
                sq.Column == startSquare.Column
            );

            if (actualIndex == -1)
            {
                Console.WriteLine($"Start square not found in full path for color {color}");
                return;
            }

            var pathSquare = path[actualIndex];

            if (pawn.Parent != null)
                pawn.Parent.Controls.Remove(pawn);

            Control container = pathSquare.Panel.GetControlFromPosition(pathSquare.Column, pathSquare.Row);
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

                pawnPositions[pawn] = actualIndex; // now reflects real index in full path
                Console.WriteLine($"Moved {color} pawn to position {actualIndex}");
            }
        }

        private int FindPositionInPath(PictureBox pawn, TableLayoutPanel panel, int column, int row)
        {
            if (!pawnPaths.ContainsKey(pawn)) return -1;

            var path = pawnPaths[pawn];

            for (int i = 0; i < path.Count; i++)
            {
                if (path[i].Panel == panel && path[i].Column == column && path[i].Row == row)
                {
                    return i;
                }
            }
            return -1;
        }

        private void Tabla_Load(object sender, EventArgs e)
        {
            graphicsManager.RepozitioneazaTabla();

        }

        private void MovePawnToSquare(PictureBox pawn, Panel square)
        {
            if (pawn.Parent != null)
            {
                pawn.Parent.Controls.Remove(pawn);
            }

            if (square != null)
            {

                square.Controls.Add(pawn);

                pawn.Dock = DockStyle.None;
                pawn.SizeMode = PictureBoxSizeMode.Zoom;
                pawn.Size = new Size(30, 30);
                pawn.Location = new Point(
                    (square.Width - pawn.Width) / 2,
                    (square.Height - pawn.Height) / 2
                );
                pawn.BringToFront();
            }
            else
            {
                Console.WriteLine("Error: Target square is null.");
            }
        }

    }
}
