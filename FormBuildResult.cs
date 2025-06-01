using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GENERATORpr
{
    public partial class FormRouteResults : Form
    {
        private FormBuildRoute buildForm;
        private Editor.FormEditor editor;

        public FormRouteResults()
        {
            InitializeComponent();
        }

        public void SetData(
            string start,
            string end,
            List<string> route,
            List<Tuple<string, string, int>> pathSegments,
            FormBuildRoute parentForm,
            Editor.FormEditor editorInstance)
        {
            this.buildForm = parentForm;
            this.editor = editorInstance;

            txtName.Text = $"{start} → {end}";
            txtName.ReadOnly = true;

            dgvRoutes.Rows.Clear();
            dgvRoutes.Columns.Clear();

            dgvRoutes.Columns.Add("Step", "Шаг");
            dgvRoutes.Columns.Add("Point", "Точка");
            dgvRoutes.Columns.Add("Description", "Описание перехода");
            dgvRoutes.Columns.Add("Length", "Длина (м)");

            int totalLength = 0;
            int step = 1;

            for (int i = 0; i < route.Count; i++)
            {
                string description = "Начальная точка";
                string lengthText = "";

                if (i > 0)
                {
                    string from = route[i - 1];
                    string to = route[i];

                    var seg = pathSegments.FirstOrDefault(s => s.Item1 == from && s.Item2 == to);
                    int len = seg?.Item3 ?? 0;

                    description = $"Из точки {from} в {to}";
                    lengthText = $"{len}";
                    totalLength += len;
                }

                dgvRoutes.Rows.Add(step++, route[i], description, lengthText);
            }

            lblInfo.Text = $"Протяжённость маршрута: {route.Count - 1} шагов, {totalLength} м.";
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            ClearEditorHighlights();

            if (buildForm != null)
                buildForm.Show();

            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV файлы (*.csv)|*.csv",
                Title = "Сохранить маршрут"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var writer = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8))
                    {
                        var headers = dgvRoutes.Columns.Cast<DataGridViewColumn>().Select(c => c.HeaderText);
                        writer.WriteLine(string.Join(";", headers));

                        foreach (DataGridViewRow row in dgvRoutes.Rows)
                        {
                            var values = row.Cells.Cast<DataGridViewCell>().Select(c => c.Value?.ToString() ?? "");
                            writer.WriteLine(string.Join(";", values));
                        }
                    }

                    MessageBox.Show("Маршрут успешно сохранён.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении: " + ex.Message);
                }
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            ClearEditorHighlights();
        }

        private void ClearEditorHighlights()
        {
            if (editor != null)
            {
                editor.ClearHighlight();
                editor.StartPoint = null;
                editor.EndPoint = null;
                editor.banPoints.Clear();
                editor.banLines.Clear();
                editor.RequiredLines.Clear();
                editor.UpdateRoutePreview();
            }
        }
    }
}
