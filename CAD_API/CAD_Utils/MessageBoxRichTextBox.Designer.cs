namespace CAD_Utils
{
    partial class MessageBoxRichTextBox
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
            this.rtbRes = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtbRes
            // 
            this.rtbRes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbRes.Location = new System.Drawing.Point(12, 34);
            this.rtbRes.Name = "rtbRes";
            this.rtbRes.Size = new System.Drawing.Size(438, 213);
            this.rtbRes.TabIndex = 0;
            this.rtbRes.Text = "";
            // 
            // MessageBoxRichTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 274);
            this.Controls.Add(this.rtbRes);
            this.Name = "MessageBoxRichTextBox";
            this.Text = "MessageBoxRichTextBox";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbRes;
    }
}