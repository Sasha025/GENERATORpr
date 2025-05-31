using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            System.Windows.Forms.ToolTip toolTip1 = new System.Windows.Forms.ToolTip();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.btnDrawLine = new System.Windows.Forms.Button();
            this.btnPan = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnShowAllRoutes = new System.Windows.Forms.Button();
            this.btnBuildRoute = new System.Windows.Forms.Button();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnOpenXml = new System.Windows.Forms.ToolStripButton();
            this.btnOpenConvertor = new System.Windows.Forms.ToolStripButton();
            this.btnSaveImage = new System.Windows.Forms.ToolStripButton();
            this.canvasScrollPanel = new GENERATORpr.Editor.PanelWithoutScrollWheel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.leftPanel.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.canvasScrollPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // leftPanel
            // 
            this.leftPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(241)))));
            this.leftPanel.Controls.Add(this.btnDrawLine);
            this.leftPanel.Controls.Add(this.btnPan);
            this.leftPanel.Controls.Add(this.btnSelect);
            this.leftPanel.Controls.Add(this.btnShowAllRoutes);
            this.leftPanel.Controls.Add(this.btnBuildRoute);
            this.leftPanel.Controls.Add(this.btnZoomIn);
            this.leftPanel.Controls.Add(this.btnZoomOut);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Location = new System.Drawing.Point(0, 25);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(41, 675);
            this.leftPanel.TabIndex = 2;
            // 
            // btnDrawLine
            // 
            this.btnDrawLine.BackColor = System.Drawing.Color.Transparent;
            this.btnDrawLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDrawLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnDrawLine.Image = global::GENERATORpr.Properties.Resources.icons8_line_30;
            this.btnDrawLine.Location = new System.Drawing.Point(3, 3);
            this.btnDrawLine.Name = "btnDrawLine";
            this.btnDrawLine.Size = new System.Drawing.Size(35, 35);
            this.btnDrawLine.TabIndex = 0;
            this.btnDrawLine.UseVisualStyleBackColor = false;
            this.btnDrawLine.Click += new System.EventHandler(this.BtnDrawLine_Click);
            // 
            // btnPan
            // 
            this.btnPan.BackColor = System.Drawing.Color.Transparent;
            this.btnPan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnPan.Image = global::GENERATORpr.Properties.Resources.icons8_move_30;
            this.btnPan.Location = new System.Drawing.Point(3, 44);
            this.btnPan.Name = "btnPan";
            this.btnPan.Size = new System.Drawing.Size(35, 35);
            this.btnPan.TabIndex = 1;
            this.btnPan.UseVisualStyleBackColor = false;
            this.btnPan.Click += new System.EventHandler(this.BtnPan_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.BackColor = System.Drawing.Color.Transparent;
            this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnSelect.Image = global::GENERATORpr.Properties.Resources.icons8_click_30;
            this.btnSelect.Location = new System.Drawing.Point(3, 85);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(35, 35);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.BtnSelect_Click);
            // 
            // btnShowAllRoutes
            // 
            this.btnShowAllRoutes.BackColor = System.Drawing.Color.Transparent;
            this.btnShowAllRoutes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowAllRoutes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnShowAllRoutes.Image = global::GENERATORpr.Properties.Resources.icons8_list_view_24;
            this.btnShowAllRoutes.Location = new System.Drawing.Point(3, 126);
            this.btnShowAllRoutes.Name = "btnShowAllRoutes";
            this.btnShowAllRoutes.Size = new System.Drawing.Size(35, 35);
            this.btnShowAllRoutes.TabIndex = 3;
            this.btnShowAllRoutes.UseVisualStyleBackColor = false;
            this.btnShowAllRoutes.Click += new System.EventHandler(this.BtnGenerateRoutes_Click);
            // 
            // btnBuildRoute
            // 
            this.btnBuildRoute.BackColor = System.Drawing.Color.Transparent;
            this.btnBuildRoute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuildRoute.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnBuildRoute.Image = global::GENERATORpr.Properties.Resources.icons8_route_30;
            this.btnBuildRoute.Location = new System.Drawing.Point(3, 167);
            this.btnBuildRoute.Name = "btnBuildRoute";
            this.btnBuildRoute.Size = new System.Drawing.Size(35, 35);
            this.btnBuildRoute.TabIndex = 4;
            this.btnBuildRoute.UseVisualStyleBackColor = false;
            this.btnBuildRoute.Click += new System.EventHandler(this.BtnBuildRoute_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.BackColor = System.Drawing.Color.Transparent;
            this.btnZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnZoomIn.Image = global::GENERATORpr.Properties.Resources.icons8_zoom_in_24;
            this.btnZoomIn.Location = new System.Drawing.Point(3, 208);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(35, 35);
            this.btnZoomIn.TabIndex = 5;
            this.btnZoomIn.UseVisualStyleBackColor = false;
            this.btnZoomIn.Click += new System.EventHandler(this.BtnZoomIn_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.BackColor = System.Drawing.Color.Transparent;
            this.btnZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomOut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnZoomOut.Image = global::GENERATORpr.Properties.Resources.icons8_zoom_out_24;
            this.btnZoomOut.Location = new System.Drawing.Point(3, 249);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(35, 35);
            this.btnZoomOut.TabIndex = 6;
            this.btnZoomOut.UseVisualStyleBackColor = false;
            this.btnZoomOut.Click += new System.EventHandler(this.BtnZoomOut_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOpenXml,
            this.btnOpenConvertor,
            this.btnSaveImage});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(1000, 25);
            this.toolStrip.TabIndex = 2;
            // 
            // btnOpenXml
            // 
            this.btnOpenXml.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenXml.Name = "btnOpenXml";
            this.btnOpenXml.Size = new System.Drawing.Size(85, 22);
            this.btnOpenXml.Text = "Открыть XML";
            this.btnOpenXml.Click += new System.EventHandler(this.BtnOpenXml_Click);
            // 
            // btnOpenConvertor
            // 
            this.btnOpenConvertor.Name = "btnOpenConvertor";
            this.btnOpenConvertor.Size = new System.Drawing.Size(69, 22);
            this.btnOpenConvertor.Text = "Конвертер";
            this.btnOpenConvertor.Click += new System.EventHandler(this.BtnOpenConvertor_Click);
            // 
            // btnSaveImage
            // 
            this.btnSaveImage.Name = "btnSaveImage";
            this.btnSaveImage.Size = new System.Drawing.Size(70, 22);
            this.btnSaveImage.Text = "Сохранить";
            this.btnSaveImage.Click += new System.EventHandler(this.BtnSaveImage_Click);
            // 
            // canvasScrollPanel
            // 
            this.canvasScrollPanel.BackColor = System.Drawing.Color.White;
            this.canvasScrollPanel.Controls.Add(this.pictureBox1);
            this.canvasScrollPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvasScrollPanel.Location = new System.Drawing.Point(41, 25);
            this.canvasScrollPanel.Name = "canvasScrollPanel";
            this.canvasScrollPanel.Size = new System.Drawing.Size(959, 675);
            this.canvasScrollPanel.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(5000, 3000);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseClick);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseMove);
            // 
            // FormEditor
            // 
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.canvasScrollPanel);
            this.Controls.Add(this.leftPanel);
            this.Controls.Add(this.toolStrip);
            this.Name = "FormEditor";
            this.Text = "Редактор схемы";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.leftPanel.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.canvasScrollPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
            //
            // Настройка кнопки
            //
            toolTip1.SetToolTip(this.btnDrawLine, "Режим рисования линии");
            toolTip1.SetToolTip(this.btnPan, "Перемещение холста");
            toolTip1.SetToolTip(this.btnZoomIn, "Приблизить");
            toolTip1.SetToolTip(this.btnZoomOut, "Отдалить");
            toolTip1.SetToolTip(this.btnSelect, "Выбрать элемент");         
            toolTip1.SetToolTip(this.btnShowAllRoutes, "Построить все маршруты на станции");  
            toolTip1.SetToolTip(this.btnBuildRoute, "Построить маршрут");

            btnOpenXml.ToolTipText = "Открыть XML файл для посроение схемы станции";
            btnOpenConvertor.ToolTipText = "Открыть конвертер данных";
            btnSaveImage.ToolTipText = "Сохранить изображение станции";
        }

    }
}
