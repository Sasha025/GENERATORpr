using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GENERATORpr.Editor
{
    public partial class FormRoutes : Form
    {
        private FormEditor editorForm;
        private string xmlFilePath;
        private Dictionary<string, List<string>> routeMap = new Dictionary<string, List<string>>();

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

                    var path = FindPath(graph, start.Id, end.Id);
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

        private List<string> FindPath(Dictionary<string, List<string>> graph, string start, string end)
        {
            var visited = new HashSet<string>();
            var queue = new Queue<List<string>>();
            queue.Enqueue(new List<string> { start });

            while (queue.Count > 0)
            {
                var path = queue.Dequeue();
                var last = path.Last();

                if (last == end)
                    return path;

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
                    int maxPerLine = 6;
                    for (int i = 0; i < path.Count; i += maxPerLine)
                    {
                        var segment = path.Skip(i).Take(maxPerLine).ToList();
                        string line = (i == 0)
                            ? string.Join(" → ", segment)
                            : "→ " + string.Join(" → ", segment);
                        listBoxDetails.Items.Add(line);
                    }

                    // ✅ Подсветка маршрута
                    if (editorForm != null)
                    {
                        editorForm.HighlightRoute(path);
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

    }
}
