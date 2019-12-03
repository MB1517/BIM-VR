namespace CADViewer
{
    partial class CADVierIni
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bOpenCADApp = new System.Windows.Forms.Button();
            this.tFilePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bPickFilePath = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bOpenCADApp
            // 
            this.bOpenCADApp.Location = new System.Drawing.Point(12, 63);
            this.bOpenCADApp.Name = "bOpenCADApp";
            this.bOpenCADApp.Size = new System.Drawing.Size(206, 46);
            this.bOpenCADApp.TabIndex = 0;
            this.bOpenCADApp.Text = "Open CAD App";
            this.bOpenCADApp.UseVisualStyleBackColor = true;
            this.bOpenCADApp.Click += new System.EventHandler(this.bOpenCADApp_Click);
            // 
            // tFilePath
            // 
            this.tFilePath.Location = new System.Drawing.Point(52, 37);
            this.tFilePath.Name = "tFilePath";
            this.tFilePath.Size = new System.Drawing.Size(575, 20);
            this.tFilePath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "File File";
            // 
            // bPickFilePath
            // 
            this.bPickFilePath.Location = new System.Drawing.Point(12, 37);
            this.bPickFilePath.Name = "bPickFilePath";
            this.bPickFilePath.Size = new System.Drawing.Size(34, 20);
            this.bPickFilePath.TabIndex = 0;
            this.bPickFilePath.Text = "...";
            this.bPickFilePath.UseVisualStyleBackColor = true;
            // 
            // CADVierIni
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 133);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tFilePath);
            this.Controls.Add(this.bPickFilePath);
            this.Controls.Add(this.bOpenCADApp);
            this.Name = "CADVierIni";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bOpenCADApp;
        private System.Windows.Forms.TextBox tFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bPickFilePath;
    }
}

