namespace DataManager
{
    partial class DataSeekerForm
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
            this.rtbValue = new System.Windows.Forms.RichTextBox();
            this.cbCatName = new System.Windows.Forms.ComboBox();
            this.cbPropName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rtbValue
            // 
            this.rtbValue.Location = new System.Drawing.Point(12, 90);
            this.rtbValue.Name = "rtbValue";
            this.rtbValue.Size = new System.Drawing.Size(769, 352);
            this.rtbValue.TabIndex = 0;
            this.rtbValue.Text = "";
            // 
            // cbCatName
            // 
            this.cbCatName.FormattingEnabled = true;
            this.cbCatName.Location = new System.Drawing.Point(12, 43);
            this.cbCatName.Name = "cbCatName";
            this.cbCatName.Size = new System.Drawing.Size(244, 21);
            this.cbCatName.TabIndex = 1;
            // 
            // cbPropName
            // 
            this.cbPropName.FormattingEnabled = true;
            this.cbPropName.Location = new System.Drawing.Point(346, 43);
            this.cbPropName.Name = "cbPropName";
            this.cbPropName.Size = new System.Drawing.Size(244, 21);
            this.cbPropName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(93, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Category Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(413, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Property Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Values";
            // 
            // DataSeekerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbPropName);
            this.Controls.Add(this.cbCatName);
            this.Controls.Add(this.rtbValue);
            this.Name = "DataSeekerForm";
            this.Text = "DataSeekerForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbValue;
        private System.Windows.Forms.ComboBox cbCatName;
        private System.Windows.Forms.ComboBox cbPropName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}