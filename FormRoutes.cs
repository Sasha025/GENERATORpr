using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GENERATORpr.Editor
{
    public partial class FormRoutes : Form
    {
        private FormEditor editorForm;
        private string xmlFilePath;
        private Dictionary<string, Tuple<List<string>, int>> routeMap = new Dictionary<string, Tuple<List<string>, int>>();


        public FormRoutes(string xmlFilePath, FormEditor editor)
        {
            InitializeComponent();
            this.xmlFilePath = xmlFilePath;
            this.editorForm = editor;
            this.Text = "Все маршруты на станции";
            LoadRoutes();
        }

        private void LoadRoutes()
        {
            var doc = XDocument.Load(xmlFilePath);

            var points = doc.Descendants("point")
                .Select(p => (
                    Id: (string)p.Attribute("id"),
                    X: (int)p.Attribute("X"),
                    Y: (int)p.Attribute("Y")
                ))
                .ToList();

            var lines = doc.Descendants("line")
                .Select(l => (
                    StartX: (int)l.Attribute("sX"),
                    StartY: (int)l.Attribute("sY"),
                    EndX: (int)l.Attribute("eX"),
                    EndY: (int)l.Attribute("eY")
                ))
                .ToList();

            var graph = BuildGraph(points, lines);

            routeMap.Clear();
            listBoxRoutes.Items.Clear();

            var visitedPairs = new HashSet<string>();

            foreach (var start in points)
            {
                foreach (var end in points)
                {
                    if (start.Id == end.Id) continue;

                    var key = $"{start.Id} → {end.Id}";
                    var reverseKey = $"{end.Id} → {start.Id}";

                    if (visitedPairs.Contains(key) || visitedPairs.Contains(reverseKey)) continue;

                    var path = FindPath(graph, start.Id, end.Id, doc);
                    if (path != null)
                    {
                        routeMap[key] = path;
                        listBoxRoutes.Items.Add(key);
                        visitedPairs.Add(key);
                    }
                }
            }
        }

        private Dictionary<string, List<string>> BuildGraph(List<(string Id, int X, int Y)> points, List<(int StartX, int StartY, int EndX, int EndY)> lines)
        {
            var graph = new Dictionary<string, List<string>>();

            foreach (var point in points)
                graph[point.Id] = new List<string>();

            foreach (var line in lines)
            {
                var start = points.FirstOrDefault(p => p.X == line.StartX && p.Y == line.StartY);
                var end = points.FirstOrDefault(p => p.X == line.EndX && p.Y == line.EndY);

                if (start != default && end != default)
                {
                    if (!graph[start.Id].Contains(end.Id))
                        graph[start.Id].Add(end.Id);
                    if (!graph[end.Id].Contains(start.Id))
                        graph[end.Id].Add(start.Id);
                }
            }

            return graph;
        }

        private Tuple<List<string>, int> FindPath(Dictionary<string, List<string>> graph, string start, string end, XDocument doc)
        {
            var visited = new HashSet<string>();
            var queue = new Queue<List<string>>();
            queue.Enqueue(new List<string> { start });

            while (queue.Count > 0)
            {
                var path = queue.Dequeue();
                var last = path.Last();

                if (last == end)
                {
                    int totalLength = 0;

                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        string from = path[i];
                        string to = path[i + 1];

                        var p1 = doc.Descendants("point").FirstOrDefault(p => (string)p.Attribute("id") == from);
                        var p2 = doc.Descendants("point").FirstOrDefault(p => (string)p.Attribute("id") == to);

                        if (p1 != null && p2 != null)
                        {
                            int x1 = (int)p1.Attribute("X");
                            int y1 = (int)p1.Attribute("Y");
                            int x2 = (int)p2.Attribute("X");
                            int y2 = (int)p2.Attribute("Y");

                            var line = doc.Descendants("line").FirstOrDefault(l =>
                                ((int)l.Attribute("sX") == x1 && (int)l.Attribute("sY") == y1 &&
                                 (int)l.Attribute("eX") == x2 && (int)l.Attribute("eY") == y2) ||
                                ((int)l.Attribute("sX") == x2 && (int)l.Attribute("sY") == y2 &&
                                 (int)l.Attribute("eX") == x1 && (int)l.Attribute("eY") == y1));

                            if (line != null)
                            {
                                var info = line.Element("lineInfo");
                                if (info != null && int.TryParse(info.Attribute("length")?.Value, out int len))
                                {
                                    totalLength += len;
                                }
                            }
                        }
                    }

                    return Tuple.Create(path, totalLength);
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

        private void listBoxRoutes_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxDetails.Items.Clear();

            if (listBoxRoutes.SelectedItem != null)
            {
                string selectedRoute = listBoxRoutes.SelectedItem.ToString();

                if (routeMap.TryGetValue(selectedRoute, out var path))
                {
                    List<string> points = path.Item1;

                    int maxPerLine = 6;
                    for (int i = 0; i < points.Count; i += maxPerLine)
                    {
                        var segment = points.Skip(i).Take(maxPerLine).ToList();
                        string line = (i == 0)
                            ? string.Join(" → ", segment)
                            : "→ " + string.Join(" → ", segment);
                        listBoxDetails.Items.Add(line);
                    }

                    //  Подсветка маршрута
                    if (editorForm != null)
                    {
                        editorForm.HighlightRoute(points);
                    }
                }
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (editorForm != null)
                editorForm.ClearHighlight(); // вызываем очистку выделений
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "CSV файлы (*.csv)|*.csv",
                Title = "Сохранить маршруты"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var writer = new System.IO.StreamWriter(saveDialog.FileName, false, Encoding.UTF8))
                    {
                        writer.WriteLine("Маршрут;Описание;Длина (м)");

                        foreach (var entry in routeMap)
                        {
                            string key = entry.Key;
                            List<string> points = entry.Value.Item1;
                            int length = entry.Value.Item2;

                            string fullPath = string.Join(" → ", points);
                            writer.WriteLine($"\"{key}\";\"{fullPath}\";\"{length}\"");
                        }
                    }

                    MessageBox.Show("Список маршрутов успешно сохранён.", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


    }
}
