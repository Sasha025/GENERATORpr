using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GENERATORpr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSelectInputFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "XML files (*.xml)|*.xml",
                Title = "Select Input XML File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtInputFilePath.Text = openFileDialog.FileName;
            }
        }

        private void btnSelectOutputFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "XML files (*.xml)|*.xml",
                Title = "Select Output XML File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtOutputFilePath.Text = openFileDialog.FileName;
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            string inputFilePath = txtInputFilePath.Text;
            string outputFilePath = txtOutputFilePath.Text;

            if (string.IsNullOrEmpty(inputFilePath) || string.IsNullOrEmpty(outputFilePath))
            {
                MessageBox.Show("Please select both input and output XML files.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(inputFilePath))
            {
                MessageBox.Show($"Input file {inputFilePath} not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(outputFilePath))
            {
                MessageBox.Show($"Output file {outputFilePath} not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                ProcessXmlFiles(inputFilePath, outputFilePath);
                MessageBox.Show("XML files processed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcessXmlFiles(string inputFilePath, string outputFilePath)
        {
            try
            {
                // Загружаем исходный XML файл
                XDocument inputDoc = XDocument.Load(inputFilePath);

                // Загружаем выходной XML файл
                XDocument outputDoc = XDocument.Load(outputFilePath);

                // Получаем секции
                var sections = inputDoc.Descendants("Section")
                                       .Select(section => new
                                       {
                                           Guid = section.Attribute("Guid")?.Value,
                                           StartId = section.Element("Start")?.Attribute("Id")?.Value,
                                           EndId = section.Element("End")?.Attribute("Id")?.Value
                                       }).ToList();

                // Получаем точки
                var points = inputDoc.Descendants("SchemaPoint")
                                     .Select(point => new
                                     {
                                         Id = point.Attribute("Id")?.Value,
                                         X = Convert.ToInt32(Math.Round(Convert.ToDouble(point.Attribute("X")?.Value) / 100)),
                                         Y = Convert.ToInt32(Math.Round(Convert.ToDouble(point.Attribute("Y")?.Value) / 100))
                                     }).ToDictionary(p => p.Id);

                // Добавляем точки в выходной XML
                var pointsElement = outputDoc.Descendants("points").FirstOrDefault();
                if (pointsElement != null)
                {
                    int pointId = 1;
                    foreach (var point in points.Values)
                    {
                        XElement pointElement = new XElement("point",
                            new XAttribute("id", pointId),
                            new XAttribute("X", point.X),
                            new XAttribute("Y", point.Y)
                        );
                        pointElement.Add(new XElement("pointInfo",
                            new XAttribute("number", ""),
                            new XAttribute("type", "2"),
                            new XAttribute("textPosition", "3"),
                            new XAttribute("gorlovina", "")
                        ));
                        pointsElement.Add(pointElement);
                        pointId++;
                    }
                }

                // Добавляем линии в выходной XML
                var linesElement = outputDoc.Descendants("lines").FirstOrDefault();
                if (linesElement != null)
                {
                    int lineId = 1;
                    foreach (var section in sections)
                    {
                        if (points.TryGetValue(section.StartId, out var start) && points.TryGetValue(section.EndId, out var end))
                        {
                            XElement lineElement = new XElement("line",
                                new XAttribute("id", lineId),
                                new XAttribute("sX", start.X),
                                new XAttribute("sY", start.Y),
                                new XAttribute("eX", end.X),
                                new XAttribute("eY", end.Y),
                                new XAttribute("kind", "2")
                            );
                            lineElement.Add(new XElement("lineInfo",
                                new XAttribute("type", "1"),
                                new XAttribute("name", ""),
                                new XAttribute("specialization", "17"),
                                new XAttribute("lengthInVagons", "0"),
                                new XAttribute("length", "0"),
                                new XAttribute("park", ""),
                                new XAttribute("lengthLeft", "0"),
                                new XAttribute("nameLeft", ""),
                                new XAttribute("signalLeft", "3"),
                                new XAttribute("lengthRight", "0"),
                                new XAttribute("nameRight", ""),
                                new XAttribute("signalRight", "3")
                            ));
                            linesElement.Add(lineElement);
                            lineId++;
                        }
                    }
                }

                // Сохраняем результат в выходной XML файл
                outputDoc.Save(outputFilePath);
            }
            catch (Exception ex)
            {
                // Логируем сообщение об ошибке
                MessageBox.Show($"An error occurred while processing XML files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
    }
}
