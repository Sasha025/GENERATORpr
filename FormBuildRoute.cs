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
            // Получаем выбранные значения
            StartPointId = lstStart.SelectedItem?.ToString()?.Split(' ')[0];
            EndPointId = lstEnd.SelectedItem?.ToString()?.Split(' ')[0];

            BanPoints = lstBanPoints.SelectedItems.Cast<string>().Select(s => s.Split(' ')[0]).ToList();
            BanLines = lstBanLines.SelectedItems.Cast<LineDisplayItem>().Select(x => x.Value).ToList();
            RequiredLines = lstRequiredLines.SelectedItems.Cast<LineDisplayItem>().Select(x => x.Value).ToList();

            // Пытаемся построить маршрут
            var route = BuildRoute(StartPointId, EndPointId, BanPoints, BanLines, RequiredLines);

            if (route == null)
            {
                MessageBox.Show("Невозможно построить маршрут с заданными условиями.");
                return;
            }

            // Формируем список сегментов маршрута с длинами
            var routeSegments = new List<Tuple<string, string, int>>();

            for (int i = 0; i < route.Count - 1; i++)
            {
                string from = route[i];
                string to = route[i + 1];

                var p1 = allPoints.First(p => p.Item3 == from);
                var p2 = allPoints.First(p => p.Item3 == to);

                var match = $"{p1.Item1},{p1.Item2},{p2.Item1},{p2.Item2}";
                var matchRev = $"{p2.Item1},{p2.Item2},{p1.Item1},{p1.Item2}";

                int length = 0;

                try
                {
                    var doc = System.Xml.Linq.XDocument.Load(editor.XmlFilePath);
                    var lineEl = doc.Descendants("line").FirstOrDefault(l =>
                        (int)l.Attribute("sX") == p1.Item1 && (int)l.Attribute("sY") == p1.Item2 &&
                        (int)l.Attribute("eX") == p2.Item1 && (int)l.Attribute("eY") == p2.Item2
                        ||
                        (int)l.Attribute("sX") == p2.Item1 && (int)l.Attribute("sY") == p2.Item2 &&
                        (int)l.Attribute("eX") == p1.Item1 && (int)l.Attribute("eY") == p1.Item2
                    );

                    if (lineEl != null)
                    {
                        var info = lineEl.Element("lineInfo");
                        if (info != null)
                            int.TryParse(info.Attribute("length")?.Value ?? "0", out length);
                    }
                }
                catch
                {
                    // на случай проблем с XML
                }

                routeSegments.Add(Tuple.Create(from, to, length));
            }

            // ✅ Подсветка маршрута на схеме
            editor.HighlightRoute(route);

            // Переход к форме результатов
            var resultForm = new FormRouteResults();
            resultForm.SetData(StartPointId, EndPointId, route, routeSegments, this, editor);
            resultForm.Show();

            this.Hide();
        }

        private Dictionary<string, List<string>> BuildGraph()
        {
            var graph = new Dictionary<string, List<string>>();

            foreach (var pt in allPoints)
                graph[pt.Item3] = new List<string>();

            foreach (var line in allLines)
            {
                string startId = pointMap.TryGetValue((line.Item1, line.Item2), out var sId) ? sId : null;
                string endId = pointMap.TryGetValue((line.Item3, line.Item4), out var eId) ? eId : null;

                if (string.IsNullOrEmpty(startId) || string.IsNullOrEmpty(endId))
                    continue;

                string code = $"{line.Item1},{line.Item2},{line.Item3},{line.Item4}";
                string codeRev = $"{line.Item3},{line.Item4},{line.Item1},{line.Item2}";

                // ❌ Пропускаем запрещённые линии
                if (BanLines.Contains(code) || BanLines.Contains(codeRev))
                    continue;

                if (!graph[startId].Contains(endId))
                    graph[startId].Add(endId);

                if (!graph[endId].Contains(startId))
                    graph[endId].Add(startId);
            }

            return graph;
        }


        private List<string> BuildRoute(string startId, string endId, List<string> bannedPoints, List<string> bannedLines, List<string> requiredLines)
        {
            this.BanLines = bannedLines;
            var graph = BuildGraph();

            // ❌ Удаляем запрещённые точки
            foreach (var ban in bannedPoints)
                graph.Remove(ban);

            foreach (var kv in graph.ToList())
            {
                graph[kv.Key] = kv.Value
                    .Where(n => !bannedPoints.Contains(n))
                    .ToList();
            }

            // BFS: но сохраняем ВСЕ маршруты
            var resultPaths = new List<List<string>>();
            var queue = new Queue<List<string>>();
            queue.Enqueue(new List<string> { startId });
            var visitedPaths = new HashSet<string>();

            while (queue.Count > 0)
            {
                var path = queue.Dequeue();
                var last = path.Last();

                if (last == endId)
                {
                    resultPaths.Add(path);
                    continue;
                }

                if (!graph.ContainsKey(last)) continue;

                foreach (var neighbor in graph[last])
                {
                    if (!path.Contains(neighbor)) // избегаем циклов
                    {
                        var newPath = new List<string>(path) { neighbor };
                        queue.Enqueue(newPath);
                    }
                }
            }

            // 🎯 Фильтрация — ищем маршрут, содержащий ВСЕ обязательные линии
            foreach (var path in resultPaths.OrderBy(p => p.Count))
            {
                var linesInPath = GetLinesFromRoute(path);
                bool containsAll = requiredLines.All(req =>
                    linesInPath.Contains(req) || linesInPath.Contains(ReverseLine(req)));

                if (requiredLines.Count == 0 || containsAll)
                    return path;
            }

            return null;
        }

        private string ReverseLine(string code)
        {
            var parts = code.Split(',');
            if (parts.Length != 4) return code;

            return $"{parts[2]},{parts[3]},{parts[0]},{parts[1]}";
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
                    UpdateRouteName(); 
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
                    UpdateRouteName(); 
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
        private void UpdateRouteName()
        {
            if (!string.IsNullOrEmpty(StartPointId) && !string.IsNullOrEmpty(EndPointId))
            {
                txtName.Text = $"{StartPointId} → {EndPointId}";
            }
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
