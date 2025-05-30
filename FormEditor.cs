using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GENERATORpr.Editor
{
    public partial class FormEditor : Form
    {
        private string xmlFilePath;
        private const int gridSize = 20;
        private float zoomFactor = 1.0f;
        private const float zoomStep = 0.1f;
        private const float zoomMin = 0.5f;
        private const float zoomMax = 3.0f;

        private bool isDrawingMode = false;
        private bool isPanningMode = false;
        private Point? firstPoint = null;
        private Point lastMouseDown;

        private Point? mouseSnapPreview = null;
        private Point panOffset = new Point(0, 0);

        private List<Tuple<Point, Point>> userLines = new List<Tuple<Point, Point>>();
        private List<Tuple<int, int, string>> loadedPoints = new List<Tuple<int, int, string>>();
        private List<Tuple<int, int, int, int>> loadedLines = new List<Tuple<int, int, int, int>>();

        public FormEditor(string xmlFilePath)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.xmlFilePath = xmlFilePath;

            pictureBox1.Size = new Size(5000, 3000);
            pictureBox1.MouseWheel += (s, e) => { }; // игнорируем
            canvasScrollPanel.MouseWheel += (s, e) => { }; // тоже игнор

            if (!string.IsNullOrEmpty(xmlFilePath))
                LoadXmlAndDraw(xmlFilePath);
        }

        public void LoadXmlAndDraw(string xmlFilePath)
        {
            try
            {
                this.xmlFilePath = xmlFilePath;

                XDocument doc = XDocument.Load(xmlFilePath);

                loadedPoints.Clear();
                foreach (var p in doc.Descendants("point"))
                {
                    int x = (int)p.Attribute("X");
                    int y = (int)p.Attribute("Y");
                    string id = (string)p.Attribute("id");
                    loadedPoints.Add(Tuple.Create(x, y, id));
                }

                loadedLines.Clear();
                foreach (var l in doc.Descendants("line"))
                {
                    int sx = (int)l.Attribute("sX");
                    int sy = (int)l.Attribute("sY");
                    int ex = (int)l.Attribute("eX");
                    int ey = (int)l.Attribute("eY");
                    loadedLines.Add(Tuple.Create(sx, sy, ex, ey));
                }

                pictureBox1.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки XML: " + ex.Message);
            }
        }

        private void UpdateCanvasSize()
        {
            int width = (int)(5000 * zoomFactor);
            int height = (int)(3000 * zoomFactor);
            pictureBox1.Size = new Size(width, height);
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.None;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.None;

            int zoomedGrid = (int)(gridSize * zoomFactor);
            g.TranslateTransform(panOffset.X, panOffset.Y);

            for (int x = 0; x < pictureBox1.Width; x += zoomedGrid)
                g.DrawLine(Pens.LightGray, x, 0, x, pictureBox1.Height);
            for (int y = 0; y < pictureBox1.Height; y += zoomedGrid)
                g.DrawLine(Pens.LightGray, 0, y, pictureBox1.Width, y);

            Pen linePen = new Pen(Color.Black, 2);
            foreach (var line in loadedLines)
                g.DrawLine(linePen,
                    line.Item1 * zoomedGrid, line.Item2 * zoomedGrid,
                    line.Item3 * zoomedGrid, line.Item4 * zoomedGrid);

            Brush pointBrush = Brushes.Red;
            Font font = new Font("Arial", 10);
            Brush textBrush = Brushes.Blue;
            foreach (var point in loadedPoints)
            {
                g.FillEllipse(pointBrush, point.Item1 * zoomedGrid - 3, point.Item2 * zoomedGrid - 3, 6, 6);
                g.DrawString(point.Item3, font, textBrush, point.Item1 * zoomedGrid + 5, point.Item2 * zoomedGrid - 5);
            }

            foreach (var line in userLines)
            {
                Pen userPen = new Pen(Color.Black, 2);
                g.DrawLine(userPen,
                    line.Item1.X * zoomedGrid, line.Item1.Y * zoomedGrid,
                    line.Item2.X * zoomedGrid, line.Item2.Y * zoomedGrid);

                g.FillEllipse(Brushes.Red,
                    line.Item1.X * zoomedGrid - 3, line.Item1.Y * zoomedGrid - 3, 6, 6);
                g.FillEllipse(Brushes.Red,
                    line.Item2.X * zoomedGrid - 3, line.Item2.Y * zoomedGrid - 3, 6, 6);
            }

            if (isDrawingMode && mouseSnapPreview != null)
            {
                int x = mouseSnapPreview.Value.X * zoomedGrid;
                int y = mouseSnapPreview.Value.Y * zoomedGrid;

                if (firstPoint == null)
                    g.FillEllipse(Brushes.Red, x - 4, y - 4, 8, 8);
                else
                {
                    Pen tempLine = new Pen(Color.Green, 1) { DashStyle = DashStyle.Dash };
                    g.DrawLine(tempLine,
                        firstPoint.Value.X * zoomedGrid, firstPoint.Value.Y * zoomedGrid,
                        x, y);

                    g.FillEllipse(Brushes.Red,
                        firstPoint.Value.X * zoomedGrid - 4, firstPoint.Value.Y * zoomedGrid - 4, 8, 8);
                    g.FillEllipse(Brushes.Red, x - 4, y - 4, 8, 8);
                }
            }
        }

        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!isDrawingMode) return;

            int zoomedGrid = (int)(gridSize * zoomFactor);
            int x = (e.X - panOffset.X) / zoomedGrid;
            int y = (e.Y - panOffset.Y) / zoomedGrid;
            Point snapped = new Point(x, y);

            if (firstPoint == null)
                firstPoint = snapped;
            else
            {
                userLines.Add(Tuple.Create(firstPoint.Value, snapped));
                firstPoint = null;
                pictureBox1.Invalidate();
            }
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPanningMode && e.Button == MouseButtons.Left)
            {
                panOffset.X += e.X - lastMouseDown.X;
                panOffset.Y += e.Y - lastMouseDown.Y;
                lastMouseDown = e.Location;
                pictureBox1.Invalidate();
            }

            if (isDrawingMode)
            {
                int zoomedGrid = (int)(gridSize * zoomFactor);
                int x = (e.X - panOffset.X) / zoomedGrid;
                int y = (e.Y - panOffset.Y) / zoomedGrid;
                mouseSnapPreview = new Point(x, y);
                pictureBox1.Invalidate();
            }
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (isPanningMode && e.Button == MouseButtons.Left)
                lastMouseDown = e.Location;
        }

        private void BtnZoomIn_Click(object sender, EventArgs e)
        {
            zoomFactor = Math.Min(zoomFactor + zoomStep, zoomMax);
            UpdateCanvasSize();
            pictureBox1.Invalidate();
        }

        private void BtnZoomOut_Click(object sender, EventArgs e)
        {
            zoomFactor = Math.Max(zoomFactor - zoomStep, zoomMin);
            UpdateCanvasSize();
            pictureBox1.Invalidate();
        }

        private void BtnDrawLine_Click(object sender, EventArgs e)
        {
            isDrawingMode = !isDrawingMode;
            btnDrawLine.BackColor = isDrawingMode ? Color.LightGreen : SystemColors.Control;
            firstPoint = null;
            pictureBox1.Invalidate();
        }

        private void BtnPan_Click(object sender, EventArgs e)
        {
            isPanningMode = !isPanningMode;
            btnPan.BackColor = isPanningMode ? Color.LightBlue : SystemColors.Control;
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Режим выбора пока не реализован.");
        }

        private void BtnGenerateRoutes_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(xmlFilePath))
                new FormRoutes(xmlFilePath).Show(this);
            else
                MessageBox.Show("Сначала загрузите XML-файл.");
        }

        private void BtnBuildRoute_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Функция построения конкретного маршрута пока в разработке.");
        }

        private void BtnOpenXml_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XML files (*.xml)|*.xml";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                xmlFilePath = dialog.FileName;
                LoadXmlAndDraw(xmlFilePath);
            }
        }

        private void BtnOpenConvertor_Click(object sender, EventArgs e)
        {
            FormConvertor form = new FormConvertor();
            form.Show(this);
        }

        private void BtnSaveImage_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.DrawToBitmap(bmp, pictureBox1.ClientRectangle);

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Png;
                if (dialog.FileName.EndsWith(".jpg")) format = System.Drawing.Imaging.ImageFormat.Jpeg;
                if (dialog.FileName.EndsWith(".bmp")) format = System.Drawing.Imaging.ImageFormat.Bmp;

                bmp.Save(dialog.FileName, format);
            }
        }
    }

    public class PanelWithoutScrollWheel : Panel
    {
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            // ничего — блокируем прокрутку
        }
    }
}
