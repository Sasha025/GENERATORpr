namespace GENERATORpr
{
    partial class FormConvertor
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConvertor));
            this.btnSelectInputFile = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.txtInputFilePath = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSelectInputFile
            // 
            this.btnSelectInputFile.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.btnSelectInputFile.Location = new System.Drawing.Point(126, 148);
            this.btnSelectInputFile.Name = "btnSelectInputFile";
            this.btnSelectInputFile.Size = new System.Drawing.Size(102, 48);
            this.btnSelectInputFile.TabIndex = 0;
            this.btnSelectInputFile.Text = "Загрузить файл";
            this.btnSelectInputFile.UseVisualStyleBackColor = false;
            this.btnSelectInputFile.Click += new System.EventHandler(this.btnSelectInputFile_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.btnProcess.Location = new System.Drawing.Point(429, 148);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(111, 49);
            this.btnProcess.TabIndex = 2;
            this.btnProcess.Text = "Выполнить обработку";
            this.btnProcess.UseVisualStyleBackColor = false;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // txtInputFilePath
            // 
            this.txtInputFilePath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(241)))));
            this.txtInputFilePath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtInputFilePath.Location = new System.Drawing.Point(12, 202);
            this.txtInputFilePath.Name = "txtInputFilePath";
            this.txtInputFilePath.Size = new System.Drawing.Size(337, 13);
            this.txtInputFilePath.TabIndex = 3;
            // 
            // FormConvertor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(669, 391);
            this.Controls.Add(this.txtInputFilePath);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.btnSelectInputFile);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormConvertor";
            this.Text = "Формирование xml файла для схемы ж/д станции ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectInputFile;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.TextBox txtInputFilePath;
    }
}

