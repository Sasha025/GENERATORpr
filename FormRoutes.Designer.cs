using System.Drawing;

namespace GENERATORpr.Editor
{
    partial class FormRoutes
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListBox listBoxRoutes;
        private System.Windows.Forms.ListBox listBoxDetails;
        private System.Windows.Forms.Label labelRoutes;
        private System.Windows.Forms.Label labelDetails;
        private System.Windows.Forms.Button btnSave;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.listBoxRoutes = new System.Windows.Forms.ListBox();
            this.listBoxDetails = new System.Windows.Forms.ListBox();
            this.labelRoutes = new System.Windows.Forms.Label();
            this.labelDetails = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxRoutes
            // 
            this.listBoxRoutes.FormattingEnabled = true;
            this.listBoxRoutes.Location = new System.Drawing.Point(12, 35);
            this.listBoxRoutes.Name = "listBoxRoutes";
            this.listBoxRoutes.Size = new System.Drawing.Size(250, 394);
            this.listBoxRoutes.TabIndex = 1;
            this.listBoxRoutes.SelectedIndexChanged += new System.EventHandler(this.listBoxRoutes_SelectedIndexChanged);
            // 
            // listBoxDetails
            // 
            this.listBoxDetails.FormattingEnabled = true;
            this.listBoxDetails.Location = new System.Drawing.Point(270, 35);
            this.listBoxDetails.Name = "listBoxDetails";
            this.listBoxDetails.Size = new System.Drawing.Size(200, 394);
            this.listBoxDetails.TabIndex = 2;
            // 
            // labelRoutes
            // 
            this.labelRoutes.AutoSize = true;
            this.labelRoutes.Location = new System.Drawing.Point(12, 15);
            this.labelRoutes.Name = "labelRoutes";
            this.labelRoutes.Size = new System.Drawing.Size(103, 13);
            this.labelRoutes.TabIndex = 3;
            this.labelRoutes.Text = "Список маршрутов";
            // 
            // labelDetails
            // 
            this.labelDetails.AutoSize = true;
            this.labelDetails.Location = new System.Drawing.Point(270, 15);
            this.labelDetails.Name = "labelDetails";
            this.labelDetails.Size = new System.Drawing.Size(98, 13);
            this.labelDetails.TabIndex = 4;
            this.labelDetails.Text = "Детали маршрута";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(476, 407);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 22);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Сохранить маршруты";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // FormRoutes
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(600, 460);
            this.Controls.Add(this.labelDetails);
            this.Controls.Add(this.labelRoutes);
            this.Controls.Add(this.listBoxDetails);
            this.Controls.Add(this.listBoxRoutes);
            this.Controls.Add(this.btnSave);
            this.Name = "FormRoutes";
            this.Text = "Все маршруты на станции";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
