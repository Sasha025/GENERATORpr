using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GENERATORpr.Editor
{
    public partial class FormEditor : Form
    {
        private string xmlFilePath;

        public FormEditor(string xmlFilePath)
        {
            InitializeComponent();
            this.xmlFilePath = xmlFilePath;
            LoadXmlAndDraw(xmlFilePath);
        }

        private void LoadXmlAndDraw(string xmlFilePath)
        {
            XDocument doc = XDocument.Load(xmlFilePath);

            var points = doc.Descendants("point")
                .Select(p => new
                {
                    Id = (string)p.Attribute("id"),
                    X = (int)p.Attribute("X"),
                    Y = (int)p.Attribute("Y")
                }).ToList();

            var lines = doc.Descendants("line")
                .Select(l => new
                {
                    StartX = (int)l.Attribute("sX"),
                    StartY = (int)l.Attribute("sY"),
                    EndX = (int)l.Attribute("eX"),
                    EndY = (int)l.Attribute("eY")
                }).ToList();

            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);

                Pen linePen = new Pen(Color.Black, 2);
                Brush pointBrush = Brushes.Red;
                Font font = new Font("Arial", 10);
                Brush textBrush = Brushes.Blue;

                foreach (var line in lines)
                {
                    g.DrawLine(linePen, line.StartX * 10, line.StartY * 10, line.EndX * 10, line.EndY * 10);
                }

                foreach (var point in points)
                {
                    g.FillEllipse(pointBrush, point.X * 10 - 3, point.Y * 10 - 3, 6, 6);
                    g.DrawString(point.Id, font, textBrush, point.X * 10 + 5, point.Y * 10 - 5);
                }
            }

            pictureBox1.Image = bmp;
        }

        private void BtnGenerateRoutes_Click(object sender, EventArgs e)
        {
            // Вызовем отдельную форму, где будут маршруты
            var routesForm = new FormRoutes(xmlFilePath);
            routesForm.Show();
        }
    }
}
