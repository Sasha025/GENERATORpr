namespace GENERATORpr
{
    partial class FormRouteResults
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblInfo;
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
            this.lblInfo = new System.Windows.Forms.Label();
            this.dgvRoutes = new System.Windows.Forms.DataGridView();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvRoutes)).BeginInit();
            this.SuspendLayout();

            // txtName
            this.txtName.Location = new System.Drawing.Point(10, 10);
            this.txtName.Size = new System.Drawing.Size(400, 20);
            this.txtName.ReadOnly = true;

            // lblInfo
            this.lblInfo.Location = new System.Drawing.Point(10, 350);
            this.lblInfo.Size = new System.Drawing.Size(400, 20);
            this.lblInfo.Text = "Протяжённость маршрута:";

            // dgvRoutes
            this.dgvRoutes.Location = new System.Drawing.Point(10, 40);
            this.dgvRoutes.Size = new System.Drawing.Size(500, 300);
            this.dgvRoutes.ReadOnly = true;
            this.dgvRoutes.AllowUserToAddRows = false;
            this.dgvRoutes.AllowUserToDeleteRows = false;

            // btnBack
            this.btnBack.Text = "< Назад";
            this.btnBack.Location = new System.Drawing.Point(10, 380);
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);

            // btnSave
            this.btnSave.Text = "Сохранить";
            this.btnSave.Location = new System.Drawing.Point(430, 380);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // FormRouteResults
            this.ClientSize = new System.Drawing.Size(530, 420);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.dgvRoutes);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnSave);
            this.Name = "FormRouteResults";
            this.Text = "Построенный маршрут";

            ((System.ComponentModel.ISupportInitialize)(this.dgvRoutes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
