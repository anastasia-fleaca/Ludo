using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Ludo.Tabla;

namespace Ludo
{
    internal class AfisareTabla
    {
        private Form parent;
        private TableLayoutPanel[] panels;
        public Dictionary<string, List<PictureBox>> pawnsByColor = new Dictionary<string, List<PictureBox>>();

        public AfisareTabla(Form form, params TableLayoutPanel[] tablePanels)
        {
            parent = form;
            panels = tablePanels;
        }

        public void DeseneazaTabla()
        {
            AddBackgroundImage(1, 1, "imagini/CASA.png", 1);
            AddBackgroundImage(0, 0, "imagini/CASA_G.png", 1);
            AddBackgroundImage(2, 0, "imagini/CASA_Y.png", 1);
            AddBackgroundImage(0, 2, "imagini/CASA_R.png", 1);
            AddBackgroundImage(2, 2, "imagini/CASA_B.png", 1);
            AddBackgroundImage(1, 1, "imagini/YELLOW.png", 2);
            AddBackgroundImage(1, 2, "imagini/YELLOW.png", 2);
            AddBackgroundImage(2, 1, "imagini/STAR_Y.png", 2);
            AddBackgroundImage(1, 4, "imagini/YELLOW.png", 2);
            AddBackgroundImage(1, 3, "imagini/YELLOW.png", 2);
            AddBackgroundImage(1, 5, "imagini/YELLOW.png", 2);
            AddBackgroundImage(4, 2, "imagini/STAR_B.png", 5);
            AddBackgroundImage(0, 1, "imagini/BLUE.png", 5);
            AddBackgroundImage(1, 1, "imagini/BLUE.png", 5);
            AddBackgroundImage(2, 1, "imagini/BLUE.png", 5);
            AddBackgroundImage(3, 1, "imagini/BLUE.png", 5);
            AddBackgroundImage(4, 1, "imagini/BLUE.png", 5);
            AddBackgroundImage(1, 1, "imagini/GREEN.png", 4);
            AddBackgroundImage(2, 1, "imagini/GREEN.png", 4);
            AddBackgroundImage(3, 1, "imagini/GREEN.png", 4);
            AddBackgroundImage(4, 1, "imagini/GREEN.png", 4);
            AddBackgroundImage(5, 1, "imagini/GREEN.png", 4);
            AddBackgroundImage(1, 0, "imagini/STAR_G.png", 4);
            AddBackgroundImage(1, 1, "imagini/RED.png", 3);
            AddBackgroundImage(1, 2, "imagini/RED.png", 3);
            AddBackgroundImage(1, 3, "imagini/RED.png", 3);
            AddBackgroundImage(1, 4, "imagini/RED.png", 3);
            AddBackgroundImage(1, 0, "imagini/RED.png", 3);
            AddBackgroundImage(0, 4, "imagini/STAR_R.png", 3);
            AddBackgroundImage(1, 5, "imagini/ARROW_R.png", 3);
            AddBackgroundImage(5, 1, "imagini/ARROW_B.png", 5);
            AddBackgroundImage(0, 1, "imagini/ARROW_G.png", 4);
            AddBackgroundImage(1, 0, "imagini/ARROW_Y.png", 2);
            AddBackgroundImage(0, 2, "imagini/STAR.png", 2);
            AddBackgroundImage(3, 0, "imagini/STAR.png", 5);
            AddBackgroundImage(2, 2, "imagini/STAR.png", 4);
            AddBackgroundImage(2, 3, "imagini/STAR.png", 3);

            for (int i = 1; i < 5; i++)
            {
                TableLayoutPanel panel = panels[i];
                for (int row = 0; row < panel.RowCount; row++)
                {
                    for (int col = 0; col < panel.ColumnCount; col++)
                    {
                        Control c = panel.GetControlFromPosition(col, row);
                        if (c == null)
                        {
                            AddBackgroundImage(col, row, "imagini/WHITE.png", i + 1);
                        }
                    }
                }
            }
        }
        public List<PathSquare> GetPawnPath()
        {
            var path = new List<PathSquare>();
            var p2 = panels[1];
            var p5 = panels[4]; 
            var p3 = panels[2]; 
            var p4 = panels[3]; 
            path.Add(new PathSquare(p2, 5, 0));
            path.Add(new PathSquare(p2, 4, 0)); 
            path.Add(new PathSquare(p2, 3, 0)); 
            path.Add(new PathSquare(p2, 2, 0));
            path.Add(new PathSquare(p2, 1, 0));

            path.Add(new PathSquare(p2, 0, 0));
            path.Add(new PathSquare(p2, 0, 1));
            path.Add(new PathSquare(p2, 0, 2));
            path.Add(new PathSquare(p2, 1, 2, "Y"));
            path.Add(new PathSquare(p2, 2, 2));
            path.Add(new PathSquare(p2, 3, 2));
            path.Add(new PathSquare(p2, 4, 2));
            path.Add(new PathSquare(p2, 5, 2));
            path.Add(new PathSquare(p5, 0, 0));
            path.Add(new PathSquare(p5, 0, 1)); 
            path.Add(new PathSquare(p5, 0, 2));
            path.Add(new PathSquare(p5, 0, 3));
            path.Add(new PathSquare(p5, 0, 4));
            path.Add(new PathSquare(p5, 0, 5));
            path.Add(new PathSquare(p5, 1, 5));
            path.Add(new PathSquare(p5, 2, 5));
            path.Add(new PathSquare(p5, 2, 4, "B"));
            path.Add(new PathSquare(p5, 2, 3));
            path.Add(new PathSquare(p5, 2, 2));
            path.Add(new PathSquare(p5, 2, 1));
            path.Add(new PathSquare(p5, 2, 0));
            path.Add(new PathSquare(p3, 0, 2)); 
            path.Add(new PathSquare(p3, 1, 2));
            path.Add(new PathSquare(p3, 2, 2));
            path.Add(new PathSquare(p3, 3, 2));
            path.Add(new PathSquare(p3, 4, 2));
            path.Add(new PathSquare(p3, 5, 2));
            path.Add(new PathSquare(p3, 5, 1));
            path.Add(new PathSquare(p3, 5, 0));
            path.Add(new PathSquare(p3, 4, 0, "R"));
            path.Add(new PathSquare(p3, 3, 0));
            path.Add(new PathSquare(p3, 2, 0));
            path.Add(new PathSquare(p3, 1, 0));
            path.Add(new PathSquare(p3, 0, 0));
            path.Add(new PathSquare(p4, 2, 5));
            path.Add(new PathSquare(p4, 2, 4));
            path.Add(new PathSquare(p4, 2, 3)); 
            path.Add(new PathSquare(p4, 2, 2));
            path.Add(new PathSquare(p4, 2, 1));
            path.Add(new PathSquare(p4, 2, 0));
            path.Add(new PathSquare(p4, 1, 0));
            path.Add(new PathSquare(p4, 0, 0));
            path.Add(new PathSquare(p4, 0, 1, "G"));
            path.Add(new PathSquare(p4, 0, 2));
            path.Add(new PathSquare(p4, 0, 3));
            path.Add(new PathSquare(p4, 0, 4));
            path.Add(new PathSquare(p4, 0, 5));
            return path;
        }
        public int GetStartPositionForColor(string color)
        {
            switch (color.ToUpper())
            {
                case "Y": return 8;   
                case "B": return 21;  
                case "R": return 34;  
                case "G": return 47;  
                default: return 0;
            }
        }
        public List<PathSquare> GetFinalPathForColor(string color)
        {
            var finalPath = new List<PathSquare>();

            switch (color.ToUpper())
            {
                case "Y": // Yellow's final 5 squares
                    finalPath.Add(new PathSquare(panels[1], 1, 1));
                    finalPath.Add(new PathSquare(panels[1], 2, 1));
                    finalPath.Add(new PathSquare(panels[1], 3, 1));
                    finalPath.Add(new PathSquare(panels[1], 4, 1));
                    finalPath.Add(new PathSquare(panels[1], 5, 1));
                    break;

                case "B": // Blue
                    finalPath.Add(new PathSquare(panels[4], 1, 4));
                    finalPath.Add(new PathSquare(panels[4], 1, 3));
                    finalPath.Add(new PathSquare(panels[4], 1, 2));
                    finalPath.Add(new PathSquare(panels[4], 1, 1));
                    finalPath.Add(new PathSquare(panels[4], 1, 0));
                    break;

                case "R": // Red
                    finalPath.Add(new PathSquare(panels[2], 4, 1));
                    finalPath.Add(new PathSquare(panels[2], 3, 1));
                    finalPath.Add(new PathSquare(panels[2], 2, 1));
                    finalPath.Add(new PathSquare(panels[2], 1, 1));
                    finalPath.Add(new PathSquare(panels[2], 0, 1));
                    break;

                case "G": // Green
                    finalPath.Add(new PathSquare(panels[3], 1, 1));
                    finalPath.Add(new PathSquare(panels[3], 1, 2));
                    finalPath.Add(new PathSquare(panels[3], 1, 3));
                    finalPath.Add(new PathSquare(panels[3], 1, 4));
                    finalPath.Add(new PathSquare(panels[3], 1, 0));
                    break;
            }

            return finalPath;
        }
        public List<PathSquare> GetFullPathForPawn(string color)
        {
            var fullPath = new List<PathSquare>();
            var basePath = GetPawnPath();
            int startIndex = GetStartPositionForColor(color);
            for (int i = 0; i < 51; i++)
            {
                int index = (startIndex + i) % basePath.Count;
                fullPath.Add(basePath[index]);
            }
            fullPath.AddRange(GetFinalPathForColor(color));

            return fullPath; 
        }


        public void AddBackgroundImage(int column, int row, string file, int panelIndex)
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Margin = new Padding(0);
            pictureBox.Image = Image.FromFile(file);

            pictureBox.Paint += delegate (object sender, PaintEventArgs e)
            {
                Pen pen = new Pen(Color.FromArgb(90, 55, 49), 2);
                e.Graphics.DrawRectangle(pen, 1, 1, pictureBox.Width - 2, pictureBox.Height - 2);
            };

            if (file.Contains("CASA_"))
            {
                char c = file[file.Length - 5];
                Add4ChildPictureBoxes(pictureBox, c);
            }

            panels[panelIndex - 1].Controls.Add(pictureBox, column, row);
        }

        public void Add4ChildPictureBoxes(PictureBox parent, char colorChar)
        {
            string color = colorChar.ToString(); // e.g., "R", "G", "B", "Y"

            if (!pawnsByColor.ContainsKey(color))
                pawnsByColor[color] = new List<PictureBox>();

            for (int i = 0; i < 4; i++)
            {
                PictureBox pawn = new PictureBox();
                pawn.SizeMode = PictureBoxSizeMode.Zoom;
                pawn.Image = Image.FromFile($"imagini/PAWN_{colorChar}.png");
                pawn.BackColor = Color.Transparent;
                parent.Controls.Add(pawn);
                pawnsByColor[color].Add(pawn); 
            }

            parent.Resize += delegate (object sender, EventArgs e)
            {
                for (int i = 0; i < 4; i++)
                {
                    PictureBox pawn = (PictureBox)parent.Controls[i];
                    int col = i % 2;
                    int row = i / 2;
                    int qW = parent.Width / 2;
                    int qH = parent.Height / 2;
                    int pW = parent.Width / 3;
                    int pH = parent.Height / 3;
                    pawn.Size = new Size(pW, pH);
                    pawn.Location = new Point(col * qW + (qW - pW) / 2, row * qH + (qH - pH) / 2);
                }
            };
        }


        public void RepozitioneazaTabla()
        {
            float relSize = 0.69f;
            float relY = 0.2f;

            int maxWidth = (int)(parent.ClientSize.Width * relSize);
            int maxHeight = (int)(parent.ClientSize.Height * relSize);

            int square = Math.Min(maxWidth, maxHeight);

            int x = (parent.ClientSize.Width - square) / 2;
            int y = (int)(parent.ClientSize.Height * relY);

            panels[0].Size = new Size(square, square);
            panels[0].Location = new Point(x, y);
        }
        public void PozitioneazaLabeluriSiButon(Label label1, Label label2, Label label3, Label label4, Button button1, string p1, string p2, string p3, string p4)
        {
            float relX1 = 0.14f;
            float relY1 = 0.22f;
            float relX2 = 1.5f;
            float relY2 = 1f;

            int newX1 = (int)(parent.ClientSize.Width * relX1);
            int newY1 = (int)(parent.ClientSize.Height * relY1);
            int newX2 = (int)(parent.ClientSize.Width * relX2);
            int newY2 = (int)(parent.ClientSize.Height * relY2);

            label1.Location = new Point(newX1, newY1);
            label2.Location = new Point(newX1, newY2);
            label3.Location = new Point(newX2, newY1);
            label4.Location = new Point(newX2, newY2);

            label1.Text = "★" + p1;
            label2.Text = "★" + p2;
            label3.Text = "★" + p3;
            label4.Text = "★" + p4;

            float relX4 = 1.61f;
            float relY4 = 0.78f;
            int newX4 = (int)(parent.ClientSize.Width * relX4);
            int newY4 = (int)(parent.ClientSize.Height * relY4);

            button1.Location = new Point(newX4, newY4);

            float buttonW = 0.23f;
            float buttonH = 0.1f;
            int newW = (int)(parent.ClientSize.Width * buttonW);
            int newH = (int)(parent.ClientSize.Width * buttonH);
            button1.Size = new Size(newW, newH);
        }

        public string GetPawnColor(PictureBox pawn)
        {
            foreach (var kvp in pawnsByColor)
            {
                if (kvp.Value.Contains(pawn))
                    return kvp.Key;
            }
            return null;
        }


    }
}