namespace GENERATORpr.Editor
{
    partial class FormRoutes
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.ListBox listBoxRoutes;
        private System.Windows.Forms.ListBox listBoxDetails;
        private System.Windows.Forms.Label labelRoutes;
        private System.Windows.Forms.Label labelDetails;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.buttonClose = new System.Windows.Forms.Button();
            this.listBoxRoutes = new System.Windows.Forms.ListBox();
            this.listBoxDetails = new System.Windows.Forms.ListBox();
            this.labelRoutes = new System.Windows.Forms.Label();
            this.labelDetails = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(500, 20);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 25);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Закрыть";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // listBoxRoutes
            // 
            this.listBoxRoutes.FormattingEnabled = true;
            this.listBoxRoutes.Location = new System.Drawing.Point(12, 35);
            this.listBoxRoutes.Name = "listBoxRoutes";
            this.listBoxRoutes.Size = new System.Drawing.Size(250, 400);
            this.listBoxRoutes.TabIndex = 1;
            this.listBoxRoutes.SelectedIndexChanged += new System.EventHandler(this.listBoxRoutes_SelectedIndexChanged);
            // 
            // listBoxDetails
            // 
            this.listBoxDetails.FormattingEnabled = true;
            this.listBoxDetails.Location = new System.Drawing.Point(270, 35);
            this.listBoxDetails.Name = "listBoxDetails";
            this.listBoxDetails.Size = new System.Drawing.Size(200, 400);
            this.listBoxDetails.TabIndex = 2;
            // 
            // labelRoutes
            // 
            this.labelRoutes.AutoSize = true;
            this.labelRoutes.Location = new System.Drawing.Point(12, 15);
            this.labelRoutes.Name = "labelRoutes";
            this.labelRoutes.Size = new System.Drawing.Size(97, 13);
            this.labelRoutes.TabIndex = 3;
            this.labelRoutes.Text = "Список маршрута";
            // 
            // labelDetails
            // 
            this.labelDetails.AutoSize = true;
            this.labelDetails.Location = new System.Drawing.Point(270, 15);
            this.labelDetails.Name = "labelDetails";
            this.labelDetails.Size = new System.Drawing.Size(96, 13);
            this.labelDetails.TabIndex = 4;
            this.labelDetails.Text = "Детали маршрута";
            // 
            // FormRoutes
            // 
            this.ClientSize = new System.Drawing.Size(600, 460);
            this.Controls.Add(this.labelDetails);
            this.Controls.Add(this.labelRoutes);
            this.Controls.Add(this.listBoxDetails);
            this.Controls.Add(this.listBoxRoutes);
            this.Controls.Add(this.buttonClose);
            this.Name = "FormRoutes";
            this.Text = "Все маршруты на станции";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
