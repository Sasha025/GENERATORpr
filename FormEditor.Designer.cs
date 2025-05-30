using System;

namespace GENERATORpr.Editor
{
    partial class FormEditor
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.PictureBox pictureBox1;
        private PanelWithoutScrollWheel canvasScrollPanel;
        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnOpenXml;
        private System.Windows.Forms.ToolStripButton btnOpenConvertor;
        private System.Windows.Forms.ToolStripButton btnSaveImage;
        private System.Windows.Forms.Button btnDrawLine;
        private System.Windows.Forms.Button btnPan;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnShowAllRoutes;
        private System.Windows.Forms.Button btnBuildRoute;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.Button btnZoomOut;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.canvasScrollPanel = new PanelWithoutScrollWheel();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnOpenXml = new System.Windows.Forms.ToolStripButton();
            this.btnOpenConvertor = new System.Windows.Forms.ToolStripButton();
            this.btnSaveImage = new System.Windows.Forms.ToolStripButton();

            this.btnDrawLine = new System.Windows.Forms.Button();
            this.btnPan = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnShowAllRoutes = new System.Windows.Forms.Button();
            this.btnBuildRoute = new System.Windows.Forms.Button();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.btnZoomOut = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.canvasScrollPanel.SuspendLayout();
            this.leftPanel.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();

            // pictureBox1
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;
            this.pictureBox1.Size = new System.Drawing.Size(5000, 3000);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseClick);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseMove);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseDown);

            // canvasScrollPanel
            this.canvasScrollPanel.BackColor = System.Drawing.Color.White;
            this.canvasScrollPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvasScrollPanel.AutoScroll = false; // отключаем прокрутку
            this.canvasScrollPanel.Controls.Add(this.pictureBox1);
            this.canvasScrollPanel.Location = new System.Drawing.Point(150, 25);
            this.canvasScrollPanel.Name = "canvasScrollPanel";
            this.canvasScrollPanel.Size = new System.Drawing.Size(850, 675);
            this.canvasScrollPanel.TabIndex = 1;

            // leftPanel
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Width = 150;
            this.leftPanel.BackColor = System.Drawing.Color.LightGray;

            // toolStrip
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.btnOpenXml, this.btnOpenConvertor, this.btnSaveImage
            });
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Size = new System.Drawing.Size(1000, 25);
            this.toolStrip.TabIndex = 2;

            this.btnOpenXml.Text = "Открыть XML";
            this.btnOpenXml.Click += new System.EventHandler(this.BtnOpenXml_Click);

            this.btnOpenConvertor.Text = "Открыть Конвертер";
            this.btnOpenConvertor.Click += new System.EventHandler(this.BtnOpenConvertor_Click);

            this.btnSaveImage.Text = "Сохранить";
            this.btnSaveImage.Click += new System.EventHandler(this.BtnSaveImage_Click);

            // Кнопки панели слева
            int y = 20;
            System.Windows.Forms.Button[] buttons = new[] {
                btnDrawLine, btnPan, btnSelect, btnShowAllRoutes, btnBuildRoute, btnZoomIn, btnZoomOut
            };

            string[] texts = {
                "Рисование", "Перемещение", "Выбор", "Маршруты", "Построить", "Увеличить", "Уменьшить"
            };

            EventHandler[] handlers = new EventHandler[] {
                BtnDrawLine_Click, BtnPan_Click, BtnSelect_Click,
                BtnGenerateRoutes_Click, BtnBuildRoute_Click,
                BtnZoomIn_Click, BtnZoomOut_Click
            };

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Size = new System.Drawing.Size(130, 30);
                buttons[i].Location = new System.Drawing.Point(10, y);
                buttons[i].Text = texts[i];
                buttons[i].Click += handlers[i];
                this.leftPanel.Controls.Add(buttons[i]);
                y += 40;
            }

            // FormEditor
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.canvasScrollPanel);
            this.Controls.Add(this.leftPanel);
            this.Controls.Add(this.toolStrip);
            this.Name = "FormEditor";
            this.Text = "Редактор схемы";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            this.canvasScrollPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.leftPanel.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
