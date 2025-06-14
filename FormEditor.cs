﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GENERATORpr.Editor
{
    public partial class FormEditor : Form
    {
        private string xmlFilePath;
        public string XmlFilePath => xmlFilePath;
        private const int gridSize = 20;
        private float zoomFactor = 1.0f;
        private const float zoomStep = 0.1f;
        private const float zoomMin = 0.5f;
        private const float zoomMax = 3.0f;
        private string stationName = "";
        private Point stationNamePosition = new Point(0, 0);
        private Font stationFont = new Font("Microsoft Sans Serif", 15);
        private Color stationColor = Color.Black;

        private bool isDrawingMode = false;
        private bool isPanningMode = false;
        private Point? firstPoint = null;
        private Point lastMouseDown;
        private FormSelectionInfo selectionForm;
        private Point? mouseSnapPreview = null;
        private Point panOffset = new Point(0, 0);
        private List<Tuple<Point, Point>> highlightedRouteLines = new List<Tuple<Point, Point>>();
        private List<Tuple<Point, Point>> userLines = new List<Tuple<Point, Point>>();
        private Tuple<int, int, string> selectedPoint = null;
        private Tuple<int, int, int, int> selectedLine = null;
        // Маршрутные выделения
        public Tuple<int, int, string> StartPoint { get; set; }
        public Tuple<int, int, string> EndPoint { get; set; }

        public List<Tuple<int, int, int, int>> banLines { get; set; } = new List<Tuple<int, int, int, int>>();
        public List<Tuple<int, int, string>> banPoints { get; set; } = new List<Tuple<int, int, string>>();
        public List<Tuple<int, int, int, int>> RequiredLines { get; set; } = new List<Tuple<int, int, int, int>>();


        private Tuple<int, int, string> hoverPoint = null;
        private Tuple<int, int, int, int> hoverLine = null;

        private List<Tuple<int, int, string>> loadedPoints = new List<Tuple<int, int, string>>();
        private List<Tuple<int, int, int, int>> loadedLines = new List<Tuple<int, int, int, int>>();

        public FormEditor(string xmlFilePath)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.xmlFilePath = xmlFilePath;
            // Устанавливаем начальный размер холста 
            pictureBox1.Size = new Size(5000, 3000);
            // Отключаем прокрутку мышью — только управление кнопками
            pictureBox1.MouseWheel += (s, e) => { };
            canvasScrollPanel.MouseWheel += (s, e) => { }; 
            if (!string.IsNullOrEmpty(xmlFilePath))  // Загружаем XML-схему станции
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
                // Поиск названия станции
                var textNode = doc.Descendants("text").FirstOrDefault();
                if (textNode != null)
                {
                    stationName = textNode.Attribute("text")?.Value ?? "";
                    int locX = int.Parse(textNode.Attribute("location_X")?.Value ?? "0");
                    int locY = int.Parse(textNode.Attribute("location_Y")?.Value ?? "0");
                    stationNamePosition = new Point(locX, locY);

                    float fontSize = float.Parse(textNode.Attribute("fontSize")?.Value ?? "15", CultureInfo.InvariantCulture);
                    string fontFamily = textNode.Attribute("fontFamilyName")?.Value ?? "Microsoft Sans Serif";
                    stationFont = new Font(fontFamily, fontSize);

                    int colorVal = int.Parse(textNode.Attribute("color")?.Value ?? "-16777216");
                    stationColor = Color.FromArgb(colorVal);
                }
                else
                {
                    stationName = "";
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
            if (highlightedRouteLines.Count > 0)
            {
                using (Pen highlightPen = new Pen(Color.FromArgb(100, Color.LimeGreen), 8))
                {
                    highlightPen.StartCap = LineCap.Round;
                    highlightPen.EndCap = LineCap.Round;

                    foreach (var seg in highlightedRouteLines)
                    {
                        g.DrawLine(highlightPen,
                            seg.Item1.X * zoomedGrid, seg.Item1.Y * zoomedGrid,
                            seg.Item2.X * zoomedGrid, seg.Item2.Y * zoomedGrid);
                    }
                }
            }
            // HOVER
            if (hoverPoint != null)
            {
                var pt = hoverPoint;
                g.FillEllipse(Brushes.Orange, pt.Item1 * zoomedGrid - 5, pt.Item2 * zoomedGrid - 5, 10, 10);
            }
            if (hoverLine != null)
            {
                var ln = hoverLine;
                using (Pen p = new Pen(Color.Orange, 4))
                {
                    g.DrawLine(p,
                        ln.Item1 * zoomedGrid, ln.Item2 * zoomedGrid,
                        ln.Item3 * zoomedGrid, ln.Item4 * zoomedGrid);
                }
            }

            if (selectedLine != null)
            {
                var ln = selectedLine;
                using (Pen p = new Pen(Color.Blue, 4))
                {
                    g.DrawLine(p,
                        ln.Item1 * zoomedGrid, ln.Item2 * zoomedGrid,
                        ln.Item3 * zoomedGrid, ln.Item4 * zoomedGrid);
                }
            }
            if (!string.IsNullOrEmpty(stationName))
            {
                using (Brush brush = new SolidBrush(stationColor))
                {
                    g.DrawString(stationName, stationFont, brush,
                        stationNamePosition.X * zoomedGrid,
                        stationNamePosition.Y * zoomedGrid);
                }
            }

            // Подсветка — запрещенные линии
            using (Pen pen = new Pen(Color.Red, 6))
            {
                foreach (var ln in banLines)
                    g.DrawLine(pen, ln.Item1 * zoomedGrid, ln.Item2 * zoomedGrid, ln.Item3 * zoomedGrid, ln.Item4 * zoomedGrid);
            }

            // Подсветка — запрещенные стрелки (точки)
            foreach (var pt in banPoints)
            {
                g.FillEllipse(Brushes.Red, pt.Item1 * zoomedGrid - 7, pt.Item2 * zoomedGrid - 7, 14, 14);
            }

            // Подсветка — обязательные пути
            using (Pen pen = new Pen(Color.Gold, 3))
            {
                foreach (var ln in RequiredLines)
                    g.DrawLine(pen, ln.Item1 * zoomedGrid, ln.Item2 * zoomedGrid, ln.Item3 * zoomedGrid, ln.Item4 * zoomedGrid);
            }

            // Подсветка — начальная и конечная точки (толстая зелёная обводка)
            using (Pen greenPen = new Pen(Color.LimeGreen, 6))
            {
                if (StartPoint != null)
                {
                    g.DrawEllipse(greenPen, StartPoint.Item1 * zoomedGrid - 7, StartPoint.Item2 * zoomedGrid - 7, 14, 14);
                    g.FillEllipse(Brushes.Lime, StartPoint.Item1 * zoomedGrid - 5, StartPoint.Item2 * zoomedGrid - 5, 10, 10);
                }

                if (EndPoint != null)
                {
                    g.DrawEllipse(greenPen, EndPoint.Item1 * zoomedGrid - 7, EndPoint.Item2 * zoomedGrid - 7, 14, 14);
                    g.FillEllipse(Brushes.Lime, EndPoint.Item1 * zoomedGrid - 5, EndPoint.Item2 * zoomedGrid - 5, 10, 10);
                }
            }



        }
        public void ApplyRouteHighlight(string startPointId, string endPointId, List<string> banPointsIds, List<string> banLinesRaw, List<string> requiredLinesRaw)
        {
            // Обновляем точки
            StartPoint = loadedPoints.FirstOrDefault(p => p.Item3 == startPointId);
            EndPoint = loadedPoints.FirstOrDefault(p => p.Item3 == endPointId);

            // Обновляем запрещенные точки
            banPoints = loadedPoints.Where(p => banPointsIds.Contains(p.Item3)).ToList();

            // Обновляем запрещенные линии — по координатам
            banLines = loadedLines.Where(l =>
                banLinesRaw.Any(f =>
                    $"{l.Item1},{l.Item2},{l.Item3},{l.Item4}" == f)).ToList();

            // Обязательные линии
            RequiredLines = loadedLines.Where(l =>
                requiredLinesRaw.Any(f =>
                    $"{l.Item1},{l.Item2},{l.Item3},{l.Item4}" == f)).ToList();

            pictureBox1.Invalidate();
        }
        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {

            int zoomedGrid = (int)(gridSize * zoomFactor);
            int x = (e.X - panOffset.X) / zoomedGrid;
            int y = (e.Y - panOffset.Y) / zoomedGrid;
            Point snapped = new Point(x, y);
            if (isDrawingMode && e.Button == MouseButtons.Right)
            {
                var toRemove = userLines.FirstOrDefault(line =>
                    IsPointNearLine(x, y, line.Item1.X, line.Item1.Y, line.Item2.X, line.Item2.Y));

                if (toRemove != null)
                {
                    userLines.Remove(toRemove);
                    pictureBox1.Invalidate();
                }
                return;
            }
            //  Режим выбора
            if (selectionForm != null && selectionForm.Visible)
            {
                // Поиск точки
                bool ctrlPressed = (ModifierKeys & Keys.Control) == Keys.Control;

                if (!ctrlPressed)
                {
                    foreach (var pt in loadedPoints)
                    {
                        if (Math.Abs(pt.Item1 - x) <= 1 && Math.Abs(pt.Item2 - y) <= 1)
                        {
                            selectionForm.ShowPoint(pt.Item3, pt.Item1, pt.Item2);
                            selectedPoint = pt;
                            selectedLine = null;
                            pictureBox1.Invalidate();
                            return;
                        }
                    }
                }

                // всегда проверяем линии
                foreach (var line in loadedLines)
                {
                    if (IsPointNearLine(x, y, line.Item1, line.Item2, line.Item3, line.Item4))
                    {
                        string name = "безымянная";
                        int length = 0;

                        try
                        {
                            XDocument doc = XDocument.Load(xmlFilePath);
                            var match = doc.Descendants("line")
                                .FirstOrDefault(l =>
                                    (int)l.Attribute("sX") == line.Item1 &&
                                    (int)l.Attribute("sY") == line.Item2 &&
                                    (int)l.Attribute("eX") == line.Item3 &&
                                    (int)l.Attribute("eY") == line.Item4);

                            if (match != null)
                            {
                                var info = match.Element("lineInfo");
                                if (info != null)
                                {
                                    name = info.Attribute("name")?.Value ?? "безымянная";
                                    length = int.Parse(info.Attribute("length")?.Value ?? "0");
                                }
                            }
                        }
                        catch { }

                        selectionForm.ShowLine(name, line.Item1, line.Item2, line.Item3, line.Item4, length);
                        selectedLine = line;
                        selectedPoint = null;
                        pictureBox1.Invalidate();
                        return;
                    }
                }
                // Ни точка, ни линия не найдены — очистим окно
                selectionForm.ClearInfo();
                selectedPoint = null;
                selectedLine = null;
                pictureBox1.Invalidate();


                return;
            }

            // ✅ Стандартное рисование
            if (!isDrawingMode) return;

            if (firstPoint == null)
                firstPoint = snapped;
            else
            {
                userLines.Add(Tuple.Create(firstPoint.Value, snapped));
                firstPoint = null;
                pictureBox1.Invalidate();
            }
        }
        private bool IsPointNearLine(int px, int py, int x1, int y1, int x2, int y2)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            var lengthSquared = dx * dx + dy * dy;

            if (lengthSquared == 0) return false;

            double t = ((px - x1) * dx + (py - y1) * dy) / (double)lengthSquared;
            t = Math.Max(0, Math.Min(1, t));

            double projX = x1 + t * dx;
            double projY = y1 + t * dy;

            double dist = Math.Sqrt((px - projX) * (px - projX) + (py - projY) * (py - projY));
            return dist < 1.0;
        }


        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            int zoomedGrid = (int)(gridSize * zoomFactor);
            int x = (e.X - panOffset.X) / zoomedGrid;
            int y = (e.Y - panOffset.Y) / zoomedGrid;

            if (isPanningMode && e.Button == MouseButtons.Left)
            {
                panOffset.X = Math.Min(0, panOffset.X + e.X - lastMouseDown.X);
                panOffset.Y = Math.Min(0, panOffset.Y + e.Y - lastMouseDown.Y);

                lastMouseDown = e.Location;
                pictureBox1.Invalidate();
            }

            if (isDrawingMode)
            {
                mouseSnapPreview = new Point(x, y);
                pictureBox1.Invalidate();
            }

            if (selectionForm != null && selectionForm.Visible)
            {
                hoverPoint = null;
                hoverLine = null;

                bool ctrlPressed = (ModifierKeys & Keys.Control) == Keys.Control;

                if (!ctrlPressed)
                {
                    foreach (var pt in loadedPoints)
                    {
                        if (Math.Abs(pt.Item1 - x) <= 1 && Math.Abs(pt.Item2 - y) <= 1)
                        {
                            hoverPoint = pt;
                            pictureBox1.Invalidate();
                            return;
                        }
                    }
                }

                foreach (var ln in loadedLines)
                {
                    if (IsPointNearLine(x, y, ln.Item1, ln.Item2, ln.Item3, ln.Item4))
                    {
                        hoverLine = ln;
                        pictureBox1.Invalidate();
                        return;
                    }
                }

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
            isDrawingMode = false;
            isPanningMode = false;

            if (selectionForm == null || selectionForm.IsDisposed)
            {
                selectionForm = new FormSelectionInfo();
                selectionForm.StartPosition = FormStartPosition.Manual;
                selectionForm.Location = new Point(this.Right - selectionForm.Width - 20, this.Top + 50);
                selectionForm.Show(this);
            }
            else
            {
                selectionForm.BringToFront();
            }
        }


        private void BtnGenerateRoutes_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(xmlFilePath))
            {
                FormRoutes frm = new FormRoutes(xmlFilePath, this);
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = new Point(this.Right - frm.Width - 10, this.Bottom - frm.Height - 10);
                frm.Show(this);
            }
            else
            {
                MessageBox.Show("Сначала загрузите XML-файл.");
            }
        }
        public void UpdateRoutePreview()
        {
            pictureBox1.Invalidate();
        }


        private void BtnBuildRoute_Click(object sender, EventArgs e)
        {
            var form = new FormBuildRoute(loadedPoints, loadedLines, this);
            form.RouteBuilt += (s, args) =>
            {
                // подсветка
                ApplyRouteHighlight(
                    form.StartPointId,
                    form.EndPointId,
                    form.BanPoints,
                    form.BanLines,
                    form.RequiredLines
                );
            };
            form.Show(this);
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
            userLines.Clear(); // сбрасываем все нарисованные линии
        }

        private void BtnOpenConvertor_Click(object sender, EventArgs e)
        {
            FormConvertor form = new FormConvertor();
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(this.Left + leftPanel.Right + 5, this.Bottom - form.Height - 10);
            form.Show(this);
        }

        private void BtnSaveImage_Click(object sender, EventArgs e)
        {
            if (loadedPoints.Count == 0 && loadedLines.Count == 0 && string.IsNullOrEmpty(stationName))
            {
                MessageBox.Show("Нет данных для сохранения.");
                return;
            }

            int padding = 4;
            int gridSize = 20;

            var xs = loadedPoints.Select(p => p.Item1).ToList();
            var ys = loadedPoints.Select(p => p.Item2).ToList();

            xs.AddRange(loadedLines.SelectMany(l => new[] { l.Item1, l.Item3 }));
            ys.AddRange(loadedLines.SelectMany(l => new[] { l.Item2, l.Item4 }));

            if (!string.IsNullOrEmpty(stationName))
            {
                xs.Add(stationNamePosition.X);
                ys.Add(stationNamePosition.Y - 2);
            }

            int minX = xs.Min() - padding;
            int maxX = xs.Max() + padding;
            int minY = ys.Min() - padding;
            int maxY = ys.Max() + padding;

            // ❗ Не выходим за пределы холста
            minX = Math.Max(minX, 0);
            minY = Math.Max(minY, 0);

            int width = (maxX - minX + 1) * gridSize;
            int height = (maxY - minY + 1) * gridSize;

            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.TranslateTransform(-minX * gridSize, -minY * gridSize);
                g.TranslateTransform(panOffset.X, panOffset.Y);

                var args = new PaintEventArgs(g, new Rectangle(0, 0, width, height));
                pictureBox1_Paint(this.pictureBox1, args);
            }

            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "PNG Image|*.png",
                FileName = "station.png"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                bmp.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                MessageBox.Show("Изображение сохранено.");
            }
        }



        public void HighlightRoute(List<string> routePointIds)
        {
            highlightedRouteLines.Clear();

            var pointsDict = loadedPoints.ToDictionary(p => p.Item3, p => new Point(p.Item1, p.Item2));

            for (int i = 0; i < routePointIds.Count - 1; i++)
            {
                string fromId = routePointIds[i];
                string toId = routePointIds[i + 1];

                if (pointsDict.TryGetValue(fromId, out var pt1) &&
                    pointsDict.TryGetValue(toId, out var pt2))
                {
                    highlightedRouteLines.Add(Tuple.Create(pt1, pt2));
                }
            }

            pictureBox1.Invalidate();
        }
        public void SetBanData(List<Tuple<int, int, string>> banPts, List<Tuple<int, int, int, int>> banLns)
        {
            this.banPoints = banPts;
            this.banLines = banLns;
            pictureBox1.Invalidate(); // перерисовка с выделением
        }

        public void ClearHighlight()
        {
            highlightedRouteLines.Clear();
            pictureBox1.Invalidate();
        }

        public void UpdateBanHighlights(List<string> pointIds, List<string> lineIds)
        {
            var pts = loadedPoints.Where(p => pointIds.Contains(p.Item3)).ToList();
            var lns = loadedLines.Where(l => lineIds.Contains($"{l.Item1},{l.Item2},{l.Item3},{l.Item4}")).ToList();

            banPoints = pts;
            banLines = lns;

            banPoints = pts;
            banLines = lns;

            pictureBox1.Invalidate();
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
