using System.Windows.Forms;

namespace GENERATORpr
{
    partial class FormBuildRoute
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.ListBox lstStart;
        private System.Windows.Forms.ListBox lstEnd;
        private System.Windows.Forms.ListBox lstBanLines;
        private System.Windows.Forms.ListBox lstBanPoints;
        private System.Windows.Forms.ListBox lstRequiredLines;
        private System.Windows.Forms.Button btnBuild;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtName = new System.Windows.Forms.TextBox();
            this.lstStart = new System.Windows.Forms.ListBox();
            this.lstEnd = new System.Windows.Forms.ListBox();
            this.lstBanLines = new System.Windows.Forms.ListBox();
            this.lstBanPoints = new System.Windows.Forms.ListBox();
            this.lstRequiredLines = new System.Windows.Forms.ListBox();
            this.btnBuild = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(10, 25);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(200, 20);
            this.txtName.TabIndex = 0;
            // 
            // lstStart
            // 
            this.lstStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstStart.Location = new System.Drawing.Point(10, 74);
            this.lstStart.Name = "lstStart";
            this.lstStart.Size = new System.Drawing.Size(310, 93);
            this.lstStart.TabIndex = 3;
            // 
            // lstEnd
            // 
            this.lstEnd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstEnd.Location = new System.Drawing.Point(330, 74);
            this.lstEnd.Name = "lstEnd";
            this.lstEnd.Size = new System.Drawing.Size(310, 93);
            this.lstEnd.TabIndex = 4;
            // 
            // lstBanLines
            // 
            this.lstBanLines.Location = new System.Drawing.Point(10, 193);
            this.lstBanLines.Name = "lstBanLines";
            this.lstBanLines.Size = new System.Drawing.Size(150, 95);
            this.lstBanLines.TabIndex = 5;
            // 
            // lstBanPoints
            // 
            this.lstBanPoints.Location = new System.Drawing.Point(170, 193);
            this.lstBanPoints.Name = "lstBanPoints";
            this.lstBanPoints.Size = new System.Drawing.Size(150, 95);
            this.lstBanPoints.TabIndex = 6;
            // 
            // lstRequiredLines
            // 
            this.lstRequiredLines.Location = new System.Drawing.Point(331, 193);
            this.lstRequiredLines.Name = "lstRequiredLines";
            this.lstRequiredLines.Size = new System.Drawing.Size(150, 95);
            this.lstRequiredLines.TabIndex = 8;
            // 
            // btnBuild
            // 
            this.btnBuild.Location = new System.Drawing.Point(570, 430);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(98, 23);
            this.btnBuild.TabIndex = 9;
            this.btnBuild.Text = "Сформировать";
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Название";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(327, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Конец маршрута";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Начало маршрута";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 177);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Запрещенные пути";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(167, 177);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Запрещенные стрелки";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(328, 177);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Обязательные пути";
            // 
            // FormBuildRoute
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(680, 470);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lstStart);
            this.Controls.Add(this.lstEnd);
            this.Controls.Add(this.lstBanLines);
            this.Controls.Add(this.lstBanPoints);
            this.Controls.Add(this.lstRequiredLines);
            this.Controls.Add(this.btnBuild);
            this.Name = "FormBuildRoute";
            this.Text = "Построение маршрута";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label7;
    }
}