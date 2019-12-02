namespace InformationInput
{
    partial class FormReadExcel
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
            this.bData = new System.Windows.Forms.Button();
            this.tDataPath = new System.Windows.Forms.TextBox();
            this.rtbRes = new System.Windows.Forms.RichTextBox();
            this.bLocalOrig = new System.Windows.Forms.Button();
            this.tDataLocalOrig = new System.Windows.Forms.TextBox();
            this.OFD1 = new System.Windows.Forms.OpenFileDialog();
            this.bGoto1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tiX = new System.Windows.Forms.TextBox();
            this.tiY = new System.Windows.Forms.TextBox();
            this.tiR = new System.Windows.Forms.TextBox();
            this.tiV = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.cbSheet = new System.Windows.Forms.ComboBox();
            this.bWrite = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // bData
            // 
            this.bData.Location = new System.Drawing.Point(16, 10);
            this.bData.Margin = new System.Windows.Forms.Padding(2);
            this.bData.Name = "bData";
            this.bData.Size = new System.Drawing.Size(56, 29);
            this.bData.TabIndex = 0;
            this.bData.Text = "Data";
            this.bData.UseVisualStyleBackColor = true;
            this.bData.Click += new System.EventHandler(this.bData_Click);
            // 
            // tDataPath
            // 
            this.tDataPath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tDataPath.Location = new System.Drawing.Point(95, 15);
            this.tDataPath.Margin = new System.Windows.Forms.Padding(2);
            this.tDataPath.Name = "tDataPath";
            this.tDataPath.Size = new System.Drawing.Size(641, 20);
            this.tDataPath.TabIndex = 1;
            // 
            // rtbRes
            // 
            this.rtbRes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbRes.Location = new System.Drawing.Point(19, 100);
            this.rtbRes.Margin = new System.Windows.Forms.Padding(2);
            this.rtbRes.Name = "rtbRes";
            this.rtbRes.Size = new System.Drawing.Size(717, 43);
            this.rtbRes.TabIndex = 2;
            this.rtbRes.Text = "";
            // 
            // bLocalOrig
            // 
            this.bLocalOrig.Location = new System.Drawing.Point(19, 44);
            this.bLocalOrig.Margin = new System.Windows.Forms.Padding(2);
            this.bLocalOrig.Name = "bLocalOrig";
            this.bLocalOrig.Size = new System.Drawing.Size(56, 41);
            this.bLocalOrig.TabIndex = 0;
            this.bLocalOrig.Text = "Local Origin";
            this.bLocalOrig.UseVisualStyleBackColor = true;
            this.bLocalOrig.Click += new System.EventHandler(this.bLocalOrig_Click);
            // 
            // tDataLocalOrig
            // 
            this.tDataLocalOrig.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tDataLocalOrig.Location = new System.Drawing.Point(95, 50);
            this.tDataLocalOrig.Margin = new System.Windows.Forms.Padding(2);
            this.tDataLocalOrig.Name = "tDataLocalOrig";
            this.tDataLocalOrig.Size = new System.Drawing.Size(641, 20);
            this.tDataLocalOrig.TabIndex = 1;
            // 
            // OFD1
            // 
            this.OFD1.FileName = "OFD";
            // 
            // bGoto1
            // 
            this.bGoto1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bGoto1.Location = new System.Drawing.Point(657, 72);
            this.bGoto1.Margin = new System.Windows.Forms.Padding(2);
            this.bGoto1.Name = "bGoto1";
            this.bGoto1.Size = new System.Drawing.Size(78, 23);
            this.bGoto1.TabIndex = 0;
            this.bGoto1.Text = "Go to form";
            this.bGoto1.UseVisualStyleBackColor = true;
            this.bGoto1.Click += new System.EventHandler(this.bGoto1_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(512, 77);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Process Schedule non Excel";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 180);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Position X column #";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 202);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Position Y column #";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 224);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Rotate column #";
            // 
            // tiX
            // 
            this.tiX.Location = new System.Drawing.Point(138, 177);
            this.tiX.Margin = new System.Windows.Forms.Padding(2);
            this.tiX.Name = "tiX";
            this.tiX.Size = new System.Drawing.Size(70, 20);
            this.tiX.TabIndex = 1;
            this.tiX.TextChanged += new System.EventHandler(this.tiX_TextChanged);
            // 
            // tiY
            // 
            this.tiY.Location = new System.Drawing.Point(138, 200);
            this.tiY.Margin = new System.Windows.Forms.Padding(2);
            this.tiY.Name = "tiY";
            this.tiY.Size = new System.Drawing.Size(70, 20);
            this.tiY.TabIndex = 1;
            this.tiY.TextChanged += new System.EventHandler(this.tiY_TextChanged);
            // 
            // tiR
            // 
            this.tiR.Location = new System.Drawing.Point(138, 223);
            this.tiR.Margin = new System.Windows.Forms.Padding(2);
            this.tiR.Name = "tiR";
            this.tiR.Size = new System.Drawing.Size(70, 20);
            this.tiR.TabIndex = 1;
            this.tiR.TextChanged += new System.EventHandler(this.tiR_TextChanged);
            // 
            // tiV
            // 
            this.tiV.Location = new System.Drawing.Point(269, 177);
            this.tiV.Margin = new System.Windows.Forms.Padding(2);
            this.tiV.Name = "tiV";
            this.tiV.Size = new System.Drawing.Size(70, 20);
            this.tiV.TabIndex = 1;
            this.tiV.TextChanged += new System.EventHandler(this.tiV_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(220, 179);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Value";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(21, 267);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(714, 266);
            this.dataGridView1.TabIndex = 5;
            // 
            // cbSheet
            // 
            this.cbSheet.FormattingEnabled = true;
            this.cbSheet.Location = new System.Drawing.Point(22, 147);
            this.cbSheet.Margin = new System.Windows.Forms.Padding(2);
            this.cbSheet.Name = "cbSheet";
            this.cbSheet.Size = new System.Drawing.Size(244, 21);
            this.cbSheet.TabIndex = 6;
            this.cbSheet.SelectedIndexChanged += new System.EventHandler(this.cbSheet_SelectedIndexChanged);
            // 
            // bWrite
            // 
            this.bWrite.Location = new System.Drawing.Point(264, 214);
            this.bWrite.Name = "bWrite";
            this.bWrite.Size = new System.Drawing.Size(75, 23);
            this.bWrite.TabIndex = 8;
            this.bWrite.Text = "Write Data";
            this.bWrite.UseVisualStyleBackColor = true;
            this.bWrite.Click += new System.EventHandler(this.bWrite_Click);
            // 
            // FormReadExcel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 560);
            this.Controls.Add(this.bWrite);
            this.Controls.Add(this.cbSheet);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtbRes);
            this.Controls.Add(this.tDataLocalOrig);
            this.Controls.Add(this.tiV);
            this.Controls.Add(this.tiR);
            this.Controls.Add(this.tiY);
            this.Controls.Add(this.tiX);
            this.Controls.Add(this.tDataPath);
            this.Controls.Add(this.bGoto1);
            this.Controls.Add(this.bLocalOrig);
            this.Controls.Add(this.bData);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormReadExcel";
            this.Text = "FormReadExcel";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bData;
        private System.Windows.Forms.TextBox tDataPath;
        private System.Windows.Forms.RichTextBox rtbRes;
        private System.Windows.Forms.Button bLocalOrig;
        private System.Windows.Forms.TextBox tDataLocalOrig;
        private System.Windows.Forms.OpenFileDialog OFD1;
        private System.Windows.Forms.Button bGoto1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tiX;
        private System.Windows.Forms.TextBox tiY;
        private System.Windows.Forms.TextBox tiR;
        private System.Windows.Forms.TextBox tiV;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox cbSheet;
        private System.Windows.Forms.Button bWrite;
    }
}