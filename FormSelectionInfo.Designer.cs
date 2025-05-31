namespace GENERATORpr
{
    partial class FormSelectionInfo
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.GroupBox groupBoxInfo;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label txtInfo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.groupBoxInfo = new System.Windows.Forms.GroupBox();
            this.txtInfo = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.groupBoxInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Controls.Add(this.txtInfo);
            this.groupBoxInfo.Controls.Add(this.lblHeader);
            this.groupBoxInfo.Location = new System.Drawing.Point(12, 12);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Size = new System.Drawing.Size(326, 100);
            this.groupBoxInfo.TabIndex = 0;
            this.groupBoxInfo.TabStop = false;
            // 
            // txtInfo
            // 
            this.txtInfo.AutoSize = true;
            this.txtInfo.Location = new System.Drawing.Point(6, 40);
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.Size = new System.Drawing.Size(0, 13);
            this.txtInfo.TabIndex = 0;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblHeader.Location = new System.Drawing.Point(6, 16);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(0, 15);
            this.lblHeader.TabIndex = 1;
            // 
            // FormSelectionInfo
            // 
            this.ClientSize = new System.Drawing.Size(350, 121);
            this.Controls.Add(this.groupBoxInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormSelectionInfo";
            this.Text = "Выбор элемента";
            this.groupBoxInfo.ResumeLayout(false);
            this.groupBoxInfo.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
