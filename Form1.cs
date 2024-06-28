using System;
using System.Drawing;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;
using System.Collections.Generic;

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

        private void btnProcess_Click(object sender, EventArgs e)
        {
            string inputFilePath = txtInputFilePath.Text;

            if (!File.Exists(inputFilePath))
            {
                MessageBox.Show($"Входной файл {inputFilePath} не найден.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                ProcessXmlFiles(inputFilePath);
                MessageBox.Show("XML файл успешно свормирован.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcessXmlFiles(string inputFilePath)
        {
            try
            {
                // Загружаем исходный XML файл
                XDocument inputDoc = XDocument.Load(inputFilePath);

                // Загружаем выходной XML файл
                XDocument outputDoc = XDocument.Parse(@"<?xml version='1.0' encoding='utf-8'?>
                    <StationMap Step='20' Width='200' Height='90'>
                    <points></points>
                    <lines></lines>
                    <textCollection>
                        <text location_X='5' location_Y='2' size_W='8' size_H='2' text='' alignment='2' fontFamilyName='Microsoft Sans Serif' fontStyle='100' fontSize='15' color='-16777216' angle='0' />
                    </textCollection>
                    <settings StationMap_backgroundColor='-1' StationMap_selectionColor='-16776961' MapGrid_visible='True' MapGrid_color='-2302756' MapCursorPoint_color='-5658199' MapCursorPoint_coordinatesVisible='True' MapLineDraw_lineColor='-8388608' MapLineDraw_pointColor='-8388608' MapLineDraw_incorrectLineColor='-5658199' MapLineDraw_incorrectPointColor='-5658199' MapLineDraw_coordinatesVisible='True' MapSelectionBox_borderColor='-16777077' MapSelectionBox_innerColor='-16776961' MapSelectionBox_coordinatesVisible='True' MapLines_defaultColor='-9868951' MapLines_defaultColorWithLength='-5103070' MapLines_wayColor='-8388608' MapLines_peregonColor='-16777088' MapLines_signalColor='-8388608' MapLines_signalVisible='True' MapLines_signalNames='True' MapPoints_errorArrowColor='-65536' MapPoints_simpleArrowColor='-8388480' MapPoints_crossColor='-5658199' MapPoints_tunnelColor='-16777216' MapGroups_groupColor='-8531' MapGroups_groupVisible='True' MapRoutes_routeColor='-65536' MapRoutesBuildSettings.maxRoutesCount='250' MapRoutesBuildSettings.maxRoutesCountForWay='250' MapText_textVisible='True' MapText_defaultColor='-16777216' MapText_defaultDraw='False' />
                    <routesList />
                    </StationMap>");

                // Извлекаем значение атрибута Title
                string schemaTitle = inputDoc.Descendants("Schema").FirstOrDefault()?.Attribute("Title")?.Value ?? "Unknown";

                // Обновляем название для схемы
                var textElement = outputDoc.Descendants("text").FirstOrDefault();
                if (textElement != null)
                {
                    textElement.SetAttributeValue("text", $"ст. {schemaTitle}");
                }

                // Получаем элементы switch
                var switches = inputDoc.Descendants("Switch")
                               .Select(switchElement => new
                               {
                                   Id = switchElement.Element("Point")?.Attribute("Id")?.Value,
                                   Name = switchElement.Attribute("Name")?.Value
                               }).ToList();

                // Получаем все секции в один список
                var allSections = inputDoc.Descendants("Section")
                    .Select(section => new
                    {
                        Guid = section.Attribute("Guid")?.Value,
                        Name = section.Attribute("Name")?.Value,
                        Length = int.Parse(section.Attribute("Length")?.Value ?? "0"),
                        EndId = section.Element("End")?.Attribute("Id")?.Value,
                        StartId = section.Element("Start")?.Attribute("Id")?.Value
                    }).Where(s => s.StartId != null && s.EndId != null).ToList();

                // Получаем секции и типы путей из EditorTracks
                var editorTracks = inputDoc.Descendants("EditorTrack")
                    .Where(track => track.Attribute("Type")?.Value == "Station")
                    .Select(track => new
                    {
                        Guid = track.Attribute("Guid")?.Value,
                        Number = track.Attribute("Number")?.Value,
                        Sections = track.Element("Sections")?.Elements("Section").Select(section => section.Attribute("Guid")?.Value).ToList()
                    }).ToList();

                // Определяем секции с наибольшей длиной для каждого EditorTrack
                var longestSections = new Dictionary<string, string>();

                foreach (var track in editorTracks)
                {
                    var longestSection = allSections
                        .Where(s => track.Sections.Contains(s.Guid))
                        .Aggregate((max, current) => max.Length > current.Length ? max : current);

                    longestSections[longestSection.Guid] = track.Number;
                }


                // Получаем элементы Section внутри элемента Sections
                var sections = inputDoc.Descendants("Sections").Elements("Section")
                                        .Select(section => new
                                        {
                                            Guid = section.Attribute("Guid")?.Value,
                                            EndId = section.Element("End")?.Attribute("Id")?.Value,
                                            StartId = section.Element("Start")?.Attribute("Id")?.Value,
                                            Length = int.Parse(section.Attribute("Length")?.Value ?? "0")
                                        }).Where(s => s.StartId != null && s.EndId != null).ToList();


                // Получаем точки
                var points = inputDoc.Descendants("SchemaPoint")
                                        .Select(point => new
                                        {
                                            Id = point.Attribute("Id")?.Value,
                                            X = point.Attribute("X") != null ? TryParseDouble(point.Attribute("X")?.Value) : (double?)null,
                                            Y = point.Attribute("Y") != null ? TryParseDouble(point.Attribute("Y")?.Value) : (double?)null
                                        }).ToList();
                double? TryParseDouble(string value)
                {
                    try
                    {
                        double result = double.Parse(value, CultureInfo.InvariantCulture);
                        return Math.Round(result / 100);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                // Добавляем точки в выходной XML
                var pointsElement = outputDoc.Descendants("points").FirstOrDefault();
                if (pointsElement != null)
                {
                    int pointId = 1;
                    foreach (var point in points)
                    {
                        // Ищем имя свитча для текущей точки
                        var switchName = switches.FirstOrDefault(s => s.Id == point.Id)?.Name ?? "";

                        XElement pointElement = new XElement("point",
                            new XAttribute("id", pointId),
                            new XAttribute("X", (int)Math.Round(point.X.Value)),
                            new XAttribute("Y", (int)Math.Round(point.Y.Value))
                        );
                        pointElement.Add(new XElement("pointInfo",
                            new XAttribute("number", switchName),
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
                    var uniqueLines = new HashSet<(int, int, int, int)>();
                    foreach (var sec in sections)
                    {
                        int startX = -1;
                        int startY = -1;
                        int endX = -1;
                        int endY = -1;
                        foreach (var point in points)
                        {
                            if (sec.StartId == point.Id && startX == -1 && startY == -1)
                            {
                                startX = (int)Math.Round(point.X.Value);
                                startY = (int)Math.Round(point.Y.Value);
                            }
                            else if (sec.EndId == point.Id && endX == -1 && endY == -1)
                            {
                                endX = (int)Math.Round(point.X.Value);
                                endY = (int)Math.Round(point.Y.Value);
                            }
                        }

                        // Пропускаем линию, если стартовые координаты совпадают с конечными
                        if (startX == endX && startY == endY)
                        {
                            continue;
                        }

                        if (startX != -1 && startY != -1 && endX != -1 && endY != -1)
                        {
                            var lineKey1 = (startX, startY, endX, endY);
                            var lineKey2 = (endX, endY, startX, startY);
                            if (uniqueLines.Contains(lineKey1) || uniqueLines.Contains(lineKey2))
                            {
                                continue; // Пропускаем дублирующуюся линию
                            }
                            uniqueLines.Add(lineKey1);

                            int kind = 7;
                            if (startX == endX)
                            {
                                kind = 0; // Вертикальная линия
                            }
                            else if (startY == endY)
                            {
                                kind = 1; // Горизонтальная линия
                            }
                            else if ((startX < endX) && (startY < endY))
                            {
                                kind = 2;
                            }
                            else if ((startX < endX) && (startY > endY))
                            {
                                kind = 3;
                            }
                            else if ((startX > endX) && (startY < endY))
                            {
                                kind = 3;
                            }
                            else if ((startX > endX) && (startY > endY))
                            {
                                kind = 2;
                            }

                            // Определяем тип и номер линии
                            int type = 1; // По умолчанию type=2
                            string number = "";
                            int specialz = 17;
                            if (longestSections.TryGetValue(sec.Guid, out string trackNumber))
                            {
                                type = 2;
                                number = trackNumber;
                                specialz = 2;
                            }


                            XElement lineElement = new XElement("line",
                              new XAttribute("id", lineId),
                              new XAttribute("sX", startX),
                              new XAttribute("sY", startY),
                              new XAttribute("eX", endX),
                              new XAttribute("eY", endY),
                              new XAttribute("kind", kind)
                            );
                            lineElement.Add(new XElement("lineInfo",
                                  new XAttribute("type", type),
                                  new XAttribute("name", number),
                                  new XAttribute("specialization", specialz),
                                  new XAttribute("lengthInVagons", "0"),
                                  new XAttribute("length", sec.Length),
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
                // Просим пользователя выбрать место для сохранения файла
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "XML Files|*.xml",
                    Title = "Save an XML File"
                };
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    outputDoc.Save(saveFileDialog.FileName);
                }
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

