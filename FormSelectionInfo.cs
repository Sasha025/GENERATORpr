using System;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GENERATORpr
{
    public partial class FormSelectionInfo : Form
    {
        public FormSelectionInfo()
        {
            InitializeComponent();
        }

        public void ShowPoint(string id, int x, int y)
        {
            groupBoxInfo.Visible = true;
            lblHeader.Text = "Информация о стрелке";
            txtInfo.Text = $"ID: {id}\r\nКоординаты: ({x}, {y})";
        }

        public void ShowLine(string name, int sx, int sy, int ex, int ey, int length)
        {
            groupBoxInfo.Visible = true;
            lblHeader.Text = "Информация о пути";
            txtInfo.Text = $"Имя: {name}\r\nКоорд: ({sx},{sy}) → ({ex},{ey})\r\nДлина: {length}";
        }

        public void ClearInfo()
        {
            groupBoxInfo.Visible = false;
        }
    }
}
