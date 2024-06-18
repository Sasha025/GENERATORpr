using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

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

                // Получаем элементы Section внутри элемента Sections
                var sections = inputDoc.Descendants("Sections").Elements("Section")
                                        .Select(section => new
                                        {
                                            Guid = section.Attribute("Guid")?.Value,
                                            StartId = section.Element("End")?.Attribute("Id")?.Value,
                                            EndId = section.Element("Start")?.Attribute("Id")?.Value
                                        }).Where(s => s.StartId != null && s.EndId != null).ToList();

                // Получаем точки
                var points = inputDoc.Descendants("SchemaPoint")
                                        .Select(point => new
                                        {
                                            Id = point.Attribute("Id")?.Value,
                                            X = point.Attribute("X")?.Value,
                                            Y = point.Attribute("Y")?.Value
                                        }).ToList();
                var a = 1;

                // Добавляем точки в выходной XML
                var pointsElement = outputDoc.Descendants("points").FirstOrDefault();
                if (pointsElement != null)
                {
                    int pointId = 1;
                    foreach (var point in points)
                    {
                        XElement pointElement = new XElement("point",
                            new XAttribute("id", point.Id),
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
                    foreach (var sec in sections)
                    {
                        int startX = 0;
                        int startY = 0;
                        int endX = 0;
                        int endY = 0;
                        foreach (var point in points)
                        {
                            if (sec.StartId == point.Id && startX == 0 && startY == 0)
                            {
                                startX = int.Parse(point.X);
                                startY = int.Parse(point.Y);
                            }
                            else if (sec.EndId == point.Id && endX == 0 && endY == 0)
                            {
                                endX = int.Parse(point.X);
                                endY = int.Parse(point.Y);
                            }
                        }
                        XElement lineElement = new XElement("line",
                              new XAttribute("id", lineId),
                              new XAttribute("sX", startX),
                              new XAttribute("sY", startY),
                              new XAttribute("eX", endX),
                              new XAttribute("eY", endY),
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

