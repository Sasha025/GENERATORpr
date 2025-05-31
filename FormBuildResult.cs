using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GENERATORpr
{
    public partial class FormRouteResults : Form
    {
        public FormRouteResults()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Заглушка
            MessageBox.Show("Функция сохранения пока не реализована.");
        }
    }
}