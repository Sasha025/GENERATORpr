using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GENERATORpr
{
    public partial class FormRouteResults : Form
    {
        public FormRouteResults()
        {
            InitializeComponent();
        }

        public void SetData(string start, string end, List<string> route)
        {
            txtName.Text = $"{start} → {end}";
            txtName.ReadOnly = true;

            dgvRoutes.Rows.Clear();
            dgvRoutes.Columns.Clear();

            dgvRoutes.Columns.Add("Step", "Шаг");
            dgvRoutes.Columns.Add("Point", "Точка");
            dgvRoutes.Columns.Add("Description", "Описание перехода");

            int step = 1;
            for (int i = 0; i < route.Count; i++)
            {
                string description = (i == 0)
                    ? "Начальная точка"
                    : $"Из точки {route[i - 1]} в {route[i]}";

                dgvRoutes.Rows.Add(step++, route[i], description);
            }

            lblInfo.Text = $"Протяжённость маршрута: {route.Count - 1} шагов.";
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Функция сохранения пока не реализована.");
        }
    }
}
