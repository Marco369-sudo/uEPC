namespace uEPC
{
    partial class uMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            panel2 = new Panel();
            dataGridView1 = new DataGridView();
            panel1 = new Panel();
            label10 = new Label();
            tEpcOutput = new TextBox();
            tCompanyPrefix = new TextBox();
            tSerialNumber = new TextBox();
            tUPC = new TextBox();
            cbxPartition = new ComboBox();
            cbxFilter = new ComboBox();
            tHeader = new TextBox();
            comboBox1 = new ComboBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            panel3 = new Panel();
            textBox1 = new TextBox();
            label7 = new Label();
            bClear = new Button();
            bExport = new Button();
            bReloadFromDb = new Button();
            bSaveRecord = new Button();
            bGenerateAll = new Button();
            bGenerate = new Button();
            tQty = new TextBox();
            label6 = new Label();
            label5 = new Label();
            tabPage2 = new TabPage();
            tDeGtin = new TextBox();
            tDeGtinNoCheck = new TextBox();
            tDeItemReference = new TextBox();
            tDeIndicator = new TextBox();
            tDeCompanyPrefix = new TextBox();
            tDePartition = new TextBox();
            tDeFilter = new TextBox();
            tDeHeader = new TextBox();
            tDeEPC = new TextBox();
            label19 = new Label();
            label18 = new Label();
            label17 = new Label();
            label16 = new Label();
            label15 = new Label();
            label14 = new Label();
            label13 = new Label();
            label12 = new Label();
            label11 = new Label();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1179, 653);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(panel2);
            tabPage1.Controls.Add(panel1);
            tabPage1.Location = new Point(4, 26);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1171, 623);
            tabPage1.TabIndex = 0;
            tabPage1.Text = " 编码EPC数据 ";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.Controls.Add(dataGridView1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 322);
            panel2.Name = "panel2";
            panel2.Size = new Size(1165, 298);
            panel2.TabIndex = 1;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(1165, 298);
            dataGridView1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(label10);
            panel1.Controls.Add(tEpcOutput);
            panel1.Controls.Add(tCompanyPrefix);
            panel1.Controls.Add(tSerialNumber);
            panel1.Controls.Add(tUPC);
            panel1.Controls.Add(cbxPartition);
            panel1.Controls.Add(cbxFilter);
            panel1.Controls.Add(tHeader);
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(panel3);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(1165, 319);
            panel1.TabIndex = 0;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(24, 231);
            label10.Name = "label10";
            label10.Size = new Size(65, 19);
            label10.TabIndex = 14;
            label10.Text = "公司前缀";
            // 
            // tEpcOutput
            // 
            tEpcOutput.Location = new Point(24, 275);
            tEpcOutput.Name = "tEpcOutput";
            tEpcOutput.Size = new Size(253, 25);
            tEpcOutput.TabIndex = 8;
            // 
            // tCompanyPrefix
            // 
            tCompanyPrefix.Location = new Point(132, 229);
            tCompanyPrefix.Name = "tCompanyPrefix";
            tCompanyPrefix.Size = new Size(145, 25);
            tCompanyPrefix.TabIndex = 13;
            // 
            // tSerialNumber
            // 
            tSerialNumber.Location = new Point(132, 182);
            tSerialNumber.Name = "tSerialNumber";
            tSerialNumber.Size = new Size(145, 25);
            tSerialNumber.TabIndex = 12;
            tSerialNumber.Text = "1";
            // 
            // tUPC
            // 
            tUPC.Location = new Point(132, 141);
            tUPC.Name = "tUPC";
            tUPC.Size = new Size(145, 25);
            tUPC.TabIndex = 11;
            tUPC.Text = "885259540510";
            tUPC.Leave += tUPC_Leave;
            // 
            // cbxPartition
            // 
            cbxPartition.FormattingEnabled = true;
            cbxPartition.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6" });
            cbxPartition.Location = new Point(132, 97);
            cbxPartition.Name = "cbxPartition";
            cbxPartition.Size = new Size(145, 25);
            cbxPartition.TabIndex = 10;
            cbxPartition.Text = "5";
            // 
            // cbxFilter
            // 
            cbxFilter.FormattingEnabled = true;
            cbxFilter.Items.AddRange(new object[] { "000", "001", "010", "011", "100", "101" });
            cbxFilter.Location = new Point(132, 56);
            cbxFilter.Name = "cbxFilter";
            cbxFilter.Size = new Size(145, 25);
            cbxFilter.TabIndex = 9;
            cbxFilter.Text = "001";
            // 
            // tHeader
            // 
            tHeader.Location = new Point(132, 17);
            tHeader.Name = "tHeader";
            tHeader.Size = new Size(145, 25);
            tHeader.TabIndex = 8;
            tHeader.Text = "30";
            // 
            // comboBox1
            // 
            comboBox1.FlatStyle = FlatStyle.Popup;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "UPC-A", "EAN13" });
            comboBox1.Location = new Point(24, 139);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(83, 25);
            comboBox1.TabIndex = 7;
            comboBox1.Tag = "";
            comboBox1.Text = "UPC-A";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(24, 188);
            label4.Name = "label4";
            label4.Size = new Size(51, 19);
            label4.TabIndex = 4;
            label4.Text = "序列号";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(24, 105);
            label3.Name = "label3";
            label3.Size = new Size(37, 19);
            label3.TabIndex = 3;
            label3.Text = "分区";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 63);
            label2.Name = "label2";
            label2.Size = new Size(37, 19);
            label2.TabIndex = 2;
            label2.Text = "滤值";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 23);
            label1.Name = "label1";
            label1.Size = new Size(51, 19);
            label1.TabIndex = 1;
            label1.Text = "标头值";
            // 
            // panel3
            // 
            panel3.Controls.Add(textBox1);
            panel3.Controls.Add(label7);
            panel3.Controls.Add(bClear);
            panel3.Controls.Add(bExport);
            panel3.Controls.Add(bReloadFromDb);
            panel3.Controls.Add(bSaveRecord);
            panel3.Controls.Add(bGenerateAll);
            panel3.Controls.Add(bGenerate);
            panel3.Controls.Add(tQty);
            panel3.Controls.Add(label6);
            panel3.Controls.Add(label5);
            panel3.Dock = DockStyle.Right;
            panel3.Location = new Point(331, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(834, 319);
            panel3.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(730, 274);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(66, 25);
            textBox1.TabIndex = 15;
            textBox1.Visible = false;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(214, 20);
            label7.Name = "label7";
            label7.Size = new Size(12, 19);
            label7.TabIndex = 7;
            label7.Text = ".";
            // 
            // bClear
            // 
            bClear.Location = new Point(21, 271);
            bClear.Name = "bClear";
            bClear.Size = new Size(162, 29);
            bClear.TabIndex = 7;
            bClear.Text = "清除";
            bClear.UseVisualStyleBackColor = true;
            bClear.Click += bClear_Click;
            // 
            // bExport
            // 
            bExport.Location = new Point(21, 182);
            bExport.Name = "bExport";
            bExport.Size = new Size(162, 29);
            bExport.TabIndex = 5;
            bExport.Text = "导出文件";
            bExport.UseVisualStyleBackColor = true;
            bExport.Click += bExport_Click;
            // 
            // bReloadFromDb
            // 
            bReloadFromDb.Location = new Point(21, 225);
            bReloadFromDb.Name = "bReloadFromDb";
            bReloadFromDb.Size = new Size(162, 29);
            bReloadFromDb.TabIndex = 6;
            bReloadFromDb.Text = "重新载入SQLite";
            bReloadFromDb.UseVisualStyleBackColor = true;
            bReloadFromDb.Click += bReloadFromDb_Click;
            // 
            // bSaveRecord
            // 
            bSaveRecord.Location = new Point(21, 138);
            bSaveRecord.Name = "bSaveRecord";
            bSaveRecord.Size = new Size(162, 29);
            bSaveRecord.TabIndex = 4;
            bSaveRecord.Text = "存储记录";
            bSaveRecord.UseVisualStyleBackColor = true;
            // 
            // bGenerateAll
            // 
            bGenerateAll.Location = new Point(21, 93);
            bGenerateAll.Name = "bGenerateAll";
            bGenerateAll.Size = new Size(162, 29);
            bGenerateAll.TabIndex = 3;
            bGenerateAll.Text = "批量生成EPC";
            bGenerateAll.UseVisualStyleBackColor = true;
            bGenerateAll.Click += bGenerateAll_Click;
            // 
            // bGenerate
            // 
            bGenerate.Location = new Point(21, 17);
            bGenerate.Name = "bGenerate";
            bGenerate.Size = new Size(162, 29);
            bGenerate.TabIndex = 2;
            bGenerate.Text = "生成EPC";
            bGenerate.UseVisualStyleBackColor = true;
            bGenerate.Click += bGenerate_Click;
            // 
            // tQty
            // 
            tQty.Location = new Point(77, 57);
            tQty.Name = "tQty";
            tQty.Size = new Size(106, 25);
            tQty.TabIndex = 1;
            tQty.Text = "100";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(21, 60);
            label6.Name = "label6";
            label6.Size = new Size(37, 19);
            label6.TabIndex = 0;
            label6.Text = "数量";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(622, 277);
            label5.Name = "label5";
            label5.Size = new Size(79, 19);
            label5.TabIndex = 5;
            label5.Text = "自定义尾数";
            label5.Visible = false;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(tDeGtin);
            tabPage2.Controls.Add(tDeGtinNoCheck);
            tabPage2.Controls.Add(tDeItemReference);
            tabPage2.Controls.Add(tDeIndicator);
            tabPage2.Controls.Add(tDeCompanyPrefix);
            tabPage2.Controls.Add(tDePartition);
            tabPage2.Controls.Add(tDeFilter);
            tabPage2.Controls.Add(tDeHeader);
            tabPage2.Controls.Add(tDeEPC);
            tabPage2.Controls.Add(label19);
            tabPage2.Controls.Add(label18);
            tabPage2.Controls.Add(label17);
            tabPage2.Controls.Add(label16);
            tabPage2.Controls.Add(label15);
            tabPage2.Controls.Add(label14);
            tabPage2.Controls.Add(label13);
            tabPage2.Controls.Add(label12);
            tabPage2.Controls.Add(label11);
            tabPage2.Location = new Point(4, 26);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1171, 623);
            tabPage2.TabIndex = 1;
            tabPage2.Text = " 解码EPC数据 ";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tDeGtin
            // 
            tDeGtin.Location = new Point(106, 414);
            tDeGtin.Name = "tDeGtin";
            tDeGtin.Size = new Size(145, 25);
            tDeGtin.TabIndex = 17;
            // 
            // tDeGtinNoCheck
            // 
            tDeGtinNoCheck.Location = new Point(106, 368);
            tDeGtinNoCheck.Name = "tDeGtinNoCheck";
            tDeGtinNoCheck.Size = new Size(145, 25);
            tDeGtinNoCheck.TabIndex = 16;
            // 
            // tDeItemReference
            // 
            tDeItemReference.Location = new Point(106, 323);
            tDeItemReference.Name = "tDeItemReference";
            tDeItemReference.Size = new Size(145, 25);
            tDeItemReference.TabIndex = 15;
            // 
            // tDeIndicator
            // 
            tDeIndicator.Location = new Point(106, 270);
            tDeIndicator.Name = "tDeIndicator";
            tDeIndicator.Size = new Size(145, 25);
            tDeIndicator.TabIndex = 14;
            // 
            // tDeCompanyPrefix
            // 
            tDeCompanyPrefix.Location = new Point(106, 224);
            tDeCompanyPrefix.Name = "tDeCompanyPrefix";
            tDeCompanyPrefix.Size = new Size(145, 25);
            tDeCompanyPrefix.TabIndex = 13;
            // 
            // tDePartition
            // 
            tDePartition.Location = new Point(106, 172);
            tDePartition.Name = "tDePartition";
            tDePartition.Size = new Size(145, 25);
            tDePartition.TabIndex = 12;
            // 
            // tDeFilter
            // 
            tDeFilter.Location = new Point(106, 122);
            tDeFilter.Name = "tDeFilter";
            tDeFilter.Size = new Size(145, 25);
            tDeFilter.TabIndex = 11;
            // 
            // tDeHeader
            // 
            tDeHeader.Location = new Point(106, 78);
            tDeHeader.Name = "tDeHeader";
            tDeHeader.Size = new Size(145, 25);
            tDeHeader.TabIndex = 10;
            // 
            // tDeEPC
            // 
            tDeEPC.Location = new Point(106, 30);
            tDeEPC.Name = "tDeEPC";
            tDeEPC.Size = new Size(235, 25);
            tDeEPC.TabIndex = 9;
            tDeEPC.Leave += tDeEPC_Leave;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(29, 33);
            label19.Name = "label19";
            label19.Size = new Size(33, 19);
            label19.TabIndex = 8;
            label19.Text = "EPC";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(29, 420);
            label18.Name = "label18";
            label18.Size = new Size(40, 19);
            label18.TabIndex = 7;
            label18.Text = "GTIN";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(29, 371);
            label17.Name = "label17";
            label17.Size = new Size(65, 19);
            label17.TabIndex = 6;
            label17.Text = "无校验码";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(29, 326);
            label16.Name = "label16";
            label16.Size = new Size(65, 19);
            label16.TabIndex = 5;
            label16.Text = "项目参考";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(29, 273);
            label15.Name = "label15";
            label15.Size = new Size(37, 19);
            label15.TabIndex = 4;
            label15.Text = "指示";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(29, 224);
            label14.Name = "label14";
            label14.Size = new Size(65, 19);
            label14.TabIndex = 3;
            label14.Text = "公司前缀";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(29, 178);
            label13.Name = "label13";
            label13.Size = new Size(51, 19);
            label13.TabIndex = 2;
            label13.Text = "分区值";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(29, 128);
            label12.Name = "label12";
            label12.Size = new Size(37, 19);
            label12.TabIndex = 1;
            label12.Text = "滤值";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(29, 84);
            label11.Name = "label11";
            label11.Size = new Size(51, 19);
            label11.TabIndex = 0;
            label11.Text = "标头值";
            // 
            // uMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1179, 653);
            Controls.Add(tabControl1);
            Font = new Font("Segoe UI", 10F);
            Name = "uMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UPC-A转换SGTIN-96 编码结构EPC";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private Panel panel2;
        private Panel panel1;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Panel panel3;
        private TabPage tabPage2;
        private DataGridView dataGridView1;
        private TextBox tCompanyPrefix;
        private TextBox tSerialNumber;
        private TextBox tUPC;
        private ComboBox cbxPartition;
        private ComboBox cbxFilter;
        private TextBox tHeader;
        private ComboBox comboBox1;
        private Label label6;
        private TextBox tQty;
        private Button bClear;
        private Button bExport;
        private Button bReloadFromDb;
        private Button bSaveRecord;
        private Button bGenerateAll;
        private Button bGenerate;
        private Label label7;
        private Label label8;
        private Label label9;
        private TextBox tEpcOutput;
        private Label label10;
        private TextBox textBox1;
        private TextBox tDeEPC;
        private Label label19;
        private Label label18;
        private Label label17;
        private Label label16;
        private Label label15;
        private Label label14;
        private Label label13;
        private Label label12;
        private Label label11;
        private TextBox tDeGtin;
        private TextBox tDeGtinNoCheck;
        private TextBox tDeItemReference;
        private TextBox tDeIndicator;
        private TextBox tDeCompanyPrefix;
        private TextBox tDePartition;
        private TextBox tDeFilter;
        private TextBox tDeHeader;
    }
}
