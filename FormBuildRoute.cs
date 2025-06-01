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
            pointMap = allPoints.ToDictionary(p => (p.Item1, p.Item2), p => p.Item3);
            this.lstBanPoints.SelectedIndexChanged += new System.EventHandler(this.lstBanPoints_SelectedIndexChanged);
            this.lstBanLines.SelectedIndexChanged += new System.EventHandler(this.lstBanLines_SelectedIndexChanged);
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
        private Dictionary<(int x, int y), string> pointMap;


        private void PopulateLineLists()
        {
            lstBanLines.Items.Clear();
            lstRequiredLines.Items.Clear();

            foreach (var line in allLines)
            {
                string startId = pointMap.TryGetValue((line.Item1, line.Item2), out var sId) ? sId : "?";
                string endId = pointMap.TryGetValue((line.Item3, line.Item4), out var eId) ? eId : "?";

                string label = $"{startId} → {endId}";
                string internalCode = $"{line.Item1},{line.Item2},{line.Item3},{line.Item4}";

                // Привязываем текст к значениям
                lstBanLines.Items.Add(new LineDisplayItem(label, internalCode));
                lstRequiredLines.Items.Add(new LineDisplayItem(label, internalCode));
            }
        }
        private class LineDisplayItem
        {
            public string Label { get; }
            public string Value { get; }

            public LineDisplayItem(string label, string value)
            {
                Label = label;
                Value = value;
            }

            public override string ToString() => Label;
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            StartPointId = lstStart.SelectedItem?.ToString()?.Split(' ')[0];
            EndPointId = lstEnd.SelectedItem?.ToString()?.Split(' ')[0];

            BanPoints = lstBanPoints.SelectedItems.Cast<string>().Select(s => s.Split(' ')[0]).ToList();
            BanLines = lstBanLines.SelectedItems.Cast<LineDisplayItem>().Select(x => x.Value).ToList();
            RequiredLines = lstRequiredLines.SelectedItems.Cast<LineDisplayItem>().Select(x => x.Value).ToList();

            // Построим маршрут
            var route = BuildRoute(StartPointId, EndPointId, BanPoints, BanLines, RequiredLines);

            if (route == null)
            {
                MessageBox.Show("Невозможно построить маршрут с заданными условиями.");
                return;
            }

            // Открываем форму с результатом
            var resultForm = new FormRouteResults();
            resultForm.SetData(StartPointId, EndPointId, route);
            resultForm.Show();

            this.Close();
        }
        private Dictionary<string, List<string>> BuildGraph()
        {
            var graph = new Dictionary<string, List<string>>();

            foreach (var pt in allPoints)
            {
                graph[pt.Item3] = new List<string>();
            }

            foreach (var line in allLines)
            {
                string startId = pointMap.TryGetValue((line.Item1, line.Item2), out var sId) ? sId : null;
                string endId = pointMap.TryGetValue((line.Item3, line.Item4), out var eId) ? eId : null;

                if (!string.IsNullOrEmpty(startId) && !string.IsNullOrEmpty(endId))
                {
                    if (!graph[startId].Contains(endId))
                        graph[startId].Add(endId);

                    if (!graph[endId].Contains(startId))
                        graph[endId].Add(startId);
                }
            }

            return graph;
        }

        private List<string> BuildRoute(string startId,string endId,List<string> bannedPoints,List<string> bannedLines,List<string> requiredLines)
        {
            var graph = BuildGraph();

            // Удаляем запрещённые точки
            foreach (var ban in bannedPoints)
                graph.Remove(ban);

            // Удаляем запрещённые связи
            foreach (var kv in graph.ToList())
            {
                graph[kv.Key] = kv.Value
                    .Where(n => !bannedPoints.Contains(n))
                    .ToList();
            }

            // BFS
            var queue = new Queue<List<string>>();
            queue.Enqueue(new List<string> { startId });
            var visited = new HashSet<string> { startId };

            while (queue.Count > 0)
            {
                var path = queue.Dequeue();
                var last = path.Last();

                if (last == endId)
                {
                    // Проверяем, содержит ли обязательные линии
                    if (requiredLines.Count > 0)
                    {
                        var linesInPath = GetLinesFromRoute(path);
                        bool containsAll = requiredLines.All(req => linesInPath.Contains(req));
                        if (!containsAll) continue; // игнорируем путь
                    }

                    return path; // маршрут найден
                }

                foreach (var neighbor in graph[last])
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        var newPath = new List<string>(path) { neighbor };
                        queue.Enqueue(newPath);
                    }
                }
            }

            return null;
        }
        private List<string> GetLinesFromRoute(List<string> path)
        {
            var lines = new List<string>();
            for (int i = 0; i < path.Count - 1; i++)
            {
                var p1 = allPoints.First(p => p.Item3 == path[i]);
                var p2 = allPoints.First(p => p.Item3 == path[i + 1]);

                var match = $"{p1.Item1},{p1.Item2},{p2.Item1},{p2.Item2}";
                var matchReverse = $"{p2.Item1},{p2.Item2},{p1.Item1},{p1.Item2}";

                if (allLines.Any(l => $"{l.Item1},{l.Item2},{l.Item3},{l.Item4}" == match ||
                                      $"{l.Item1},{l.Item2},{l.Item3},{l.Item4}" == matchReverse))
                {
                    lines.Add(match);
                }
            }

            return lines;
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
            BanPoints = lstBanPoints.SelectedItems.Cast<string>().Select(s => s.Split(' ')[0]).ToList();

            editor.UpdateBanHighlights(BanPoints, BanLines);
        }

        private void lstBanLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            BanLines = lstBanLines.SelectedItems.Cast<LineDisplayItem>().Select(item => item.Value).ToList();

            editor.UpdateBanHighlights(BanPoints, BanLines);
        }
        private void lstRequiredLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            RequiredLines = lstRequiredLines.SelectedItems.Cast<LineDisplayItem>().Select(x => x.Value).ToList();
            editor.RequiredLines = allLines
                .Where(l => RequiredLines.Contains($"{l.Item1},{l.Item2},{l.Item3},{l.Item4}"))
                .ToList();

            editor.UpdateRoutePreview();
        }

        private void FormBuildRoute_FormClosing(object sender, FormClosingEventArgs e)
        {
            editor.StartPoint = null;
            editor.EndPoint = null;
            editor.banPoints.Clear();
            editor.banLines.Clear();
            editor.RequiredLines.Clear();
            editor.UpdateRoutePreview();
        }
    }
}
