using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GENERATORpr.Editor;

namespace GENERATORpr
{
    public partial class FormBuildRoute : Form
    {
        private FormEditor editor;
        private List<Tuple<int, int, string>> allPoints;
        private List<Tuple<int, int, int, int>> allLines;

        public List<string> BanLines { get; private set; } = new List<string>();
        public List<string> BanPoints { get; private set; } = new List<string>();
        public List<string> RequiredLines { get; private set; } = new List<string>();

        public string StartPointId { get; private set; }
        public string EndPointId { get; private set; }

        public event EventHandler RouteBuilt;

        public FormBuildRoute(List<Tuple<int, int, string>> points, List<Tuple<int, int, int, int>> lines, FormEditor editor)
        {
            InitializeComponent();

            this.allPoints = points;
            this.allLines = lines;
            this.editor = editor;

            this.lstBanPoints.SelectedIndexChanged += lstBanPoints_SelectedIndexChanged;
            this.lstBanLines.SelectedIndexChanged += lstBanLines_SelectedIndexChanged;
            this.lstRequiredLines.SelectedIndexChanged += lstRequiredLines_SelectedIndexChanged;
            this.lstStart.SelectedIndexChanged += LstStart_SelectedIndexChanged;
            this.lstEnd.SelectedIndexChanged += LstEnd_SelectedIndexChanged;

            this.FormClosing += FormBuildRoute_FormClosing;

            PopulateStartEndPoints();
            PopulateLineLists();
        }

        private void PopulateStartEndPoints()
        {
            lstStart.Items.Clear();
            lstEnd.Items.Clear();
            lstBanPoints.Items.Clear();

            foreach (var pt in allPoints)
            {
                string label = $"{pt.Item3} ({pt.Item1}, {pt.Item2})";
                lstStart.Items.Add(label);
                lstEnd.Items.Add(label);
                lstBanPoints.Items.Add(label);
            }
        }

        private void PopulateLineLists()
        {
            lstBanLines.Items.Clear();
            lstRequiredLines.Items.Clear();

            foreach (var line in allLines)
            {
                string id = $"{line.Item1},{line.Item2},{line.Item3},{line.Item4}";
                string label = $"{id}";
                lstBanLines.Items.Add(label);
                lstRequiredLines.Items.Add(label);
            }
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            StartPointId = lstStart.SelectedItem?.ToString()?.Split(' ')[0];
            EndPointId = lstEnd.SelectedItem?.ToString()?.Split(' ')[0];

            BanPoints = lstBanPoints.SelectedItems.Cast<string>().Select(s => s.Split(' ')[0]).ToList();
            BanLines = lstBanLines.SelectedItems.Cast<string>().ToList(); // строки с координатами
            RequiredLines = lstRequiredLines.SelectedItems.Cast<string>().ToList(); // строки с координатами

            RouteBuilt?.Invoke(this, EventArgs.Empty);
            this.Close();
        }

        private void LstStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstStart.SelectedIndex >= 0)
            {
                var text = lstStart.SelectedItem.ToString();
                var id = text.Split(' ')[0];
                var point = allPoints.FirstOrDefault(p => p.Item3 == id);
                if (point != null)
                {
                    StartPointId = id;
                    editor.StartPoint = point;
                    editor.UpdateRoutePreview();
                }
            }
        }

        private void LstEnd_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstEnd.SelectedIndex >= 0)
            {
                var text = lstEnd.SelectedItem.ToString();
                var id = text.Split(' ')[0];
                var point = allPoints.FirstOrDefault(p => p.Item3 == id);
                if (point != null)
                {
                    EndPointId = id;
                    editor.EndPoint = point;
                    editor.UpdateRoutePreview();
                }
            }
        }

        private void lstBanPoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ids = lstBanPoints.SelectedItems.Cast<string>().Select(s => s.Split(' ')[0]).ToList();
            editor.BanPoints = allPoints.Where(p => ids.Contains(p.Item3)).ToList();
            editor.UpdateRoutePreview();
        }

        private void lstBanLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ids = lstBanLines.SelectedItems.Cast<string>().ToList();
            editor.BanLines = allLines.Where(l => ids.Contains($"{l.Item1},{l.Item2},{l.Item3},{l.Item4}")).ToList();
            editor.UpdateRoutePreview();
        }

        private void lstRequiredLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ids = lstRequiredLines.SelectedItems.Cast<string>().ToList();
            editor.RequiredLines = allLines.Where(l => ids.Contains($"{l.Item1},{l.Item2},{l.Item3},{l.Item4}")).ToList();
            editor.UpdateRoutePreview();
        }

        private void FormBuildRoute_FormClosing(object sender, FormClosingEventArgs e)
        {
            editor.StartPoint = null;
            editor.EndPoint = null;
            editor.BanPoints.Clear();
            editor.BanLines.Clear();
            editor.RequiredLines.Clear();
            editor.UpdateRoutePreview();
        }
    }
}
