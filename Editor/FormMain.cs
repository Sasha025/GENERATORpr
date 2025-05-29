namespace PrometeusWayReferenceControlWindow
{
    using PrometeusWayReferenceControl;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using System.Xml;

    public class FormMain : Form
    {
        private MapPointGroup mpg;
        private IContainer components;
        private Panel panel1;
        private ToolStrip toolStrip;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton3;
        private Panel panel2;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private ToolStripButton toolStripButton4;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lStatus;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton toolStripButton5;
        public PrometeusWayReferenceControl.PrometeusWayReferenceControl Map;
        private ToolStripButton toolStripButton6;

        public FormMain()
        {
            this.InitializeComponent();
            this.mpg = new MapPointGroup(this.Map.stationMap);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FormMain));
            this.panel1 = new Panel();
            this.toolStrip = new ToolStrip();
            this.toolStripButton1 = new ToolStripButton();
            this.toolStripButton2 = new ToolStripButton();
            this.toolStripButton3 = new ToolStripButton();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.toolStripButton4 = new ToolStripButton();
            this.toolStripButton5 = new ToolStripButton();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.toolStripButton6 = new ToolStripButton();
            this.panel2 = new Panel();
            this.Map = new PrometeusWayReferenceControl.PrometeusWayReferenceControl();
            this.statusStrip1 = new StatusStrip();
            this.lStatus = new ToolStripStatusLabel();
            this.openFileDialog = new OpenFileDialog();
            this.saveFileDialog = new SaveFileDialog();
            this.panel1.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.panel2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            base.SuspendLayout();
            this.panel1.Controls.Add(this.toolStrip);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x325, 0x19);
            this.panel1.TabIndex = 0;
            this.toolStrip.Dock = DockStyle.Fill;
            ToolStripItem[] toolStripItems = new ToolStripItem[] { this.toolStripButton1, this.toolStripButton2, this.toolStripButton3, this.toolStripSeparator1, this.toolStripButton4, this.toolStripButton5, this.toolStripSeparator2, this.toolStripButton6 };
            this.toolStrip.Items.AddRange(toolStripItems);
            this.toolStrip.Location = new Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new Size(0x325, 0x19);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            this.toolStrip.Paint += new PaintEventHandler(this.toolStrip_Paint);
            this.toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = (Image) manager.GetObject("toolStripButton1.Image");
            this.toolStripButton1.ImageTransparentColor = Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new Size(0x17, 0x16);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.ToolTipText = "Создать новую схему путевого развития";
            this.toolStripButton1.Click += new EventHandler(this.toolStripButton1_Click);
            this.toolStripButton2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = (Image) manager.GetObject("toolStripButton2.Image");
            this.toolStripButton2.ImageTransparentColor = Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new Size(0x17, 0x16);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.ToolTipText = "Открыть ранее сохраненную схему путевого развития";
            this.toolStripButton2.Click += new EventHandler(this.toolStripButton2_Click);
            this.toolStripButton3.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = (Image) manager.GetObject("toolStripButton3.Image");
            this.toolStripButton3.ImageTransparentColor = Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new Size(0x17, 0x16);
            this.toolStripButton3.Text = "toolStripButton3";
            this.toolStripButton3.ToolTipText = "Сохранить схему путевого развития";
            this.toolStripButton3.Click += new EventHandler(this.toolStripButton3_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 0x19);
            this.toolStripButton4.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = (Image) manager.GetObject("toolStripButton4.Image");
            this.toolStripButton4.ImageTransparentColor = Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new Size(0x17, 0x16);
            this.toolStripButton4.Text = "toolStripButton4";
            this.toolStripButton4.ToolTipText = "Группы точек";
            this.toolStripButton4.Visible = false;
            this.toolStripButton4.Click += new EventHandler(this.toolStripButton4_Click);
            this.toolStripButton5.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = (Image) manager.GetObject("toolStripButton5.Image");
            this.toolStripButton5.ImageTransparentColor = Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new Size(0x17, 0x16);
            this.toolStripButton5.Text = "toolStripButton5";
            this.toolStripButton5.ToolTipText = "Построить все простые маршруты";
            this.toolStripButton5.Click += new EventHandler(this.toolStripButton5_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(6, 0x19);
            this.toolStripButton6.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = (Image) manager.GetObject("toolStripButton6.Image");
            this.toolStripButton6.ImageTransparentColor = Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new Size(0x17, 0x16);
            this.toolStripButton6.Text = "Сформировать картинку";
            this.toolStripButton6.Click += new EventHandler(this.toolStripButton6_Click);
            this.panel2.Controls.Add(this.Map);
            this.panel2.Controls.Add(this.statusStrip1);
            this.panel2.Dock = DockStyle.Fill;
            this.panel2.Location = new Point(0, 0x19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x325, 0x233);
            this.panel2.TabIndex = 1;
            this.Map.Dock = DockStyle.Fill;
            this.Map.Location = new Point(0, 0);
            this.Map.Name = "Map";
            this.Map.Padding = new Padding(2);
            this.Map.Size = new Size(0x325, 0x21d);
            this.Map.TabIndex = 3;
            this.Map.add_OnMapLineClick(new PrometeusWayReferenceControl.PrometeusWayReferenceControl.OnMapLineClickHandler(this, this.Map_OnMapLineClick));
            this.Map.add_OnMapPointClick(new PrometeusWayReferenceControl.PrometeusWayReferenceControl.OnMapPointClickHandler(this, this.Map_OnMapPointClick));
            this.Map.add_OnMapLineDoubleClick(new PrometeusWayReferenceControl.PrometeusWayReferenceControl.OnMapLineDoubleClickHandler(this, this.Map_OnMapLineDoubleClick));
            this.Map.add_OnMapPointDoubleClick(new PrometeusWayReferenceControl.PrometeusWayReferenceControl.OnMapPointDoubleClickHandler(this, this.Map_OnMapPointDoubleClick));
            this.Map.Load += new EventHandler(this.Map_Load);
            ToolStripItem[] itemArray2 = new ToolStripItem[] { this.lStatus };
            this.statusStrip1.Items.AddRange(itemArray2);
            this.statusStrip1.Location = new Point(0, 0x21d);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new Size(0x325, 0x16);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            this.lStatus.AutoSize = false;
            this.lStatus.Name = "lStatus";
            this.lStatus.Size = new Size(350, 0x11);
            this.lStatus.TextAlign = ContentAlignment.MiddleLeft;
            this.openFileDialog.Filter = "Схема станции|*.xml";
            this.saveFileDialog.Filter = "Схема станции|*.xml";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x325, 0x24c);
            base.Controls.Add(this.panel2);
            base.Controls.Add(this.panel1);
            base.Name = "FormMain";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Схема путевого развития";
            base.WindowState = FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            base.ResumeLayout(false);
        }

        private void Map_Load(object sender, EventArgs e)
        {
        }

        private void Map_OnMapLineClick(MapLine sender)
        {
            this.lStatus.Text = $"{"Клик по отрезку "}{sender.ToString()}";
        }

        private void Map_OnMapLineDoubleClick(MapLine sender)
        {
            this.lStatus.Text = $"{"Двойной клик по отрезку "}{sender.ToString()}";
            this.Map.stationMap.lines.selectedLines.Clear();
            this.Map.stationMap.lines.selectedLines.Add(sender);
            this.mpg.lines.Add(sender);
        }

        private void Map_OnMapPointClick(MapPoint sender)
        {
            this.lStatus.Text = $"{"Клик по точке "}{sender.ToString()}";
        }

        private void Map_OnMapPointDoubleClick(MapPoint sender)
        {
            this.lStatus.Text = $"{"Двойной клик по точке "}{sender.ToString()}";
        }

        private void toolStrip_Paint(object sender, PaintEventArgs e)
        {
            Rectangle bounds = ((ToolStrip) sender).Bounds;
            LinearGradientBrush brush = new LinearGradientBrush(bounds, Color.White, Color.FromArgb(0x87, 0xad, 0xe4), 90f);
            e.Graphics.FillRectangle(brush, bounds);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Map.Clear();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.openFileDialog.Filter = "Схема станции|*.xml";
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                XmlDocument document = new XmlDocument();
                document.Load(this.openFileDialog.FileName);
                this.Map.LoadFromXML(document.DocumentElement);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.saveFileDialog.AddExtension = true;
            this.saveFileDialog.Filter = "Схема станции|*.xml";
            this.saveFileDialog.DefaultExt = ".xml";
            if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml("<StationMap></StationMap>");
                this.Map.SaveToXML(document.DocumentElement);
                document.Save(this.saveFileDialog.FileName);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            new Form1().Show(this);
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            this.saveFileDialog.Filter = "Схема в Png|*png|Схема в Jpg|*jpg|Схема в Bmp|*bmp";
            this.saveFileDialog.DefaultExt = ".png";
            if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.Map.stationMap.SaveToImageFile(this.saveFileDialog.FileName);
            }
        }

        private void userControl11_Load(object sender, EventArgs e)
        {
        }
    }
}

