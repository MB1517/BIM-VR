namespace RevitExportCollada
{
    partial class ColladaExporter_Form
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
            this.tDirectoryPath = new System.Windows.Forms.TextBox();
            this.bBrowser = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tPrefix = new System.Windows.Forms.TextBox();
            this.tSuffix = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tContainer = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tBCat = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tFamily = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tType = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tInstGUID = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label9 = new System.Windows.Forms.Label();
            this.rtbRes = new System.Windows.Forms.RichTextBox();
            this.rtbFullName = new System.Windows.Forms.RichTextBox();
            this.bCreateName = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tDirectoryPath
            // 
            this.tDirectoryPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tDirectoryPath.Location = new System.Drawing.Point(101, 49);
            this.tDirectoryPath.Name = "tDirectoryPath";
            this.tDirectoryPath.Size = new System.Drawing.Size(639, 22);
            this.tDirectoryPath.TabIndex = 0;
            this.tDirectoryPath.TextChanged += new System.EventHandler(this.tPrefix_TextChanged);
            // 
            // bBrowser
            // 
            this.bBrowser.Location = new System.Drawing.Point(12, 49);
            this.bBrowser.Name = "bBrowser";
            this.bBrowser.Size = new System.Drawing.Size(75, 23);
            this.bBrowser.TabIndex = 1;
            this.bBrowser.Text = "...";
            this.bBrowser.UseVisualStyleBackColor = true;
            this.bBrowser.Click += new System.EventHandler(this.bBrowser_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(98, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Directory path";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Prefix name";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(235, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Suffix name";
            // 
            // tPrefix
            // 
            this.tPrefix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tPrefix.Location = new System.Drawing.Point(12, 110);
            this.tPrefix.Name = "tPrefix";
            this.tPrefix.Size = new System.Drawing.Size(196, 22);
            this.tPrefix.TabIndex = 4;
            // 
            // tSuffix
            // 
            this.tSuffix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tSuffix.Location = new System.Drawing.Point(238, 110);
            this.tSuffix.Name = "tSuffix";
            this.tSuffix.Size = new System.Drawing.Size(196, 22);
            this.tSuffix.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 212);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Container";
            // 
            // tContainer
            // 
            this.tContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tContainer.Location = new System.Drawing.Point(12, 235);
            this.tContainer.Name = "tContainer";
            this.tContainer.Size = new System.Drawing.Size(196, 22);
            this.tContainer.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 259);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(119, 17);
            this.label5.TabIndex = 2;
            this.label5.Text = "Building Category";
            // 
            // tBCat
            // 
            this.tBCat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tBCat.Location = new System.Drawing.Point(12, 282);
            this.tBCat.Name = "tBCat";
            this.tBCat.Size = new System.Drawing.Size(196, 22);
            this.tBCat.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 307);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 17);
            this.label6.TabIndex = 2;
            this.label6.Text = "Family Name";
            // 
            // tFamily
            // 
            this.tFamily.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tFamily.Location = new System.Drawing.Point(12, 330);
            this.tFamily.Name = "tFamily";
            this.tFamily.Size = new System.Drawing.Size(196, 22);
            this.tFamily.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 354);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 17);
            this.label7.TabIndex = 2;
            this.label7.Text = "Type";
            // 
            // tType
            // 
            this.tType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tType.Location = new System.Drawing.Point(12, 377);
            this.tType.Name = "tType";
            this.tType.Size = new System.Drawing.Size(196, 22);
            this.tType.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 398);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(99, 17);
            this.label8.TabIndex = 2;
            this.label8.Text = "Instance GUID";
            // 
            // tInstGUID
            // 
            this.tInstGUID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tInstGUID.Location = new System.Drawing.Point(12, 421);
            this.tInstGUID.Name = "tInstGUID";
            this.tInstGUID.Size = new System.Drawing.Size(196, 22);
            this.tInstGUID.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 135);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 17);
            this.label9.TabIndex = 5;
            this.label9.Text = "Full Name";
            // 
            // rtbRes
            // 
            this.rtbRes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbRes.Location = new System.Drawing.Point(268, 275);
            this.rtbRes.Name = "rtbRes";
            this.rtbRes.Size = new System.Drawing.Size(471, 243);
            this.rtbRes.TabIndex = 10;
            this.rtbRes.Text = "";
            // 
            // rtbFullName
            // 
            this.rtbFullName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbFullName.Location = new System.Drawing.Point(12, 156);
            this.rtbFullName.Name = "rtbFullName";
            this.rtbFullName.Size = new System.Drawing.Size(728, 53);
            this.rtbFullName.TabIndex = 11;
            this.rtbFullName.Text = "";
            // 
            // bCreateName
            // 
            this.bCreateName.Location = new System.Drawing.Point(440, 109);
            this.bCreateName.Name = "bCreateName";
            this.bCreateName.Size = new System.Drawing.Size(120, 23);
            this.bCreateName.TabIndex = 12;
            this.bCreateName.Text = "Create Name";
            this.bCreateName.UseVisualStyleBackColor = true;
            this.bCreateName.Click += new System.EventHandler(this.bCreateName_Click);
            // 
            // ColladaExporter_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 521);
            this.Controls.Add(this.bCreateName);
            this.Controls.Add(this.rtbFullName);
            this.Controls.Add(this.rtbRes);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tSuffix);
            this.Controls.Add(this.tInstGUID);
            this.Controls.Add(this.tType);
            this.Controls.Add(this.tFamily);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tBCat);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tContainer);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tPrefix);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bBrowser);
            this.Controls.Add(this.tDirectoryPath);
            this.Name = "ColladaExporter_Form";
            this.Text = "ColladaExtractor_Form";
            this.Load += new System.EventHandler(this.ColladaExporter_Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tDirectoryPath;
        private System.Windows.Forms.Button bBrowser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tPrefix;
        private System.Windows.Forms.TextBox tSuffix;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tContainer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tBCat;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tFamily;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tInstGUID;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RichTextBox rtbRes;
        private System.Windows.Forms.RichTextBox rtbFullName;
        private System.Windows.Forms.Button bCreateName;
    }
}