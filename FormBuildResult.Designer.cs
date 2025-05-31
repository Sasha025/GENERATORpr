using System.Windows.Forms;

namespace GENERATORpr
{
    partial class FormRouteResults
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.NumericUpDown numTime;
        private System.Windows.Forms.DataGridView dgvRoutes;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnSave;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtName = new System.Windows.Forms.TextBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.numTime = new System.Windows.Forms.NumericUpDown();
            this.dgvRoutes = new System.Windows.Forms.DataGridView();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoutes)).BeginInit();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(10, 10);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(200, 20);
            this.txtName.TabIndex = 0;
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.Location = new System.Drawing.Point(220, 10);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(120, 21);
            this.cmbType.TabIndex = 1;
            // 
            // numTime
            // 
            this.numTime.Location = new System.Drawing.Point(350, 10);
            this.numTime.Name = "numTime";
            this.numTime.Size = new System.Drawing.Size(60, 20);
            this.numTime.TabIndex = 2;
            // 
            // dgvRoutes
            // 
            this.dgvRoutes.AllowUserToAddRows = false;
            this.dgvRoutes.AllowUserToDeleteRows = false;
            this.dgvRoutes.Location = new System.Drawing.Point(10, 40);
            this.dgvRoutes.Name = "dgvRoutes";
            this.dgvRoutes.ReadOnly = true;
            this.dgvRoutes.Size = new System.Drawing.Size(660, 380);
            this.dgvRoutes.TabIndex = 3;
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(10, 430);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 4;
            this.btnBack.Text = "< Назад";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(593, 430);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Сохранить";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // FormRouteResults
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(680, 470);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.numTime);
            this.Controls.Add(this.dgvRoutes);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnSave);
            this.Name = "FormRouteResults";
            this.Text = "Маршруты";
            ((System.ComponentModel.ISupportInitialize)(this.numTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoutes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}