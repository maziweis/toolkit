namespace ResourceToolkit
{
    partial class ResouceSetForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResouceSetForm));
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btn_upt = new System.Windows.Forms.Button();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.Muti_upload = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ClearData = new System.Windows.Forms.Button();
            this.BREEL_cb = new System.Windows.Forms.ComboBox();
            this.GRADE_cb = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SUB_cb = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ED_cb = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_check_con = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBoxError = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lb_UploadNo = new System.Windows.Forms.Label();
            this.lbNew = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFileUrl = new System.Windows.Forms.TextBox();
            this.btn_OpenFile = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SelectAll = new System.Windows.Forms.CheckBox();
            this.Lb_sql = new System.Windows.Forms.Label();
            this.groupBox5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label2.Location = new System.Drawing.Point(137, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(236, 24);
            this.label2.TabIndex = 40;
            this.label2.Text = "资源导入工具(内网)";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btn_upt);
            this.groupBox5.Controls.Add(this.checkedListBox1);
            this.groupBox5.Controls.Add(this.Muti_upload);
            this.groupBox5.Location = new System.Drawing.Point(38, 200);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(442, 139);
            this.groupBox5.TabIndex = 53;
            this.groupBox5.TabStop = false;
            // 
            // btn_upt
            // 
            this.btn_upt.Location = new System.Drawing.Point(233, 86);
            this.btn_upt.Name = "btn_upt";
            this.btn_upt.Size = new System.Drawing.Size(102, 36);
            this.btn_upt.TabIndex = 18;
            this.btn_upt.Text = "批量更新";
            this.btn_upt.UseVisualStyleBackColor = true;
            this.btn_upt.Click += new System.EventHandler(this.btn_upt_Click);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.AllowDrop = true;
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(15, 17);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(195, 116);
            this.checkedListBox1.TabIndex = 0;
            // 
            // Muti_upload
            // 
            this.Muti_upload.Location = new System.Drawing.Point(233, 17);
            this.Muti_upload.Name = "Muti_upload";
            this.Muti_upload.Size = new System.Drawing.Size(102, 45);
            this.Muti_upload.TabIndex = 17;
            this.Muti_upload.Text = "点击上传";
            this.Muti_upload.UseVisualStyleBackColor = true;
            this.Muti_upload.Click += new System.EventHandler(this.Muti_upload_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ClearData);
            this.groupBox3.Controls.Add(this.BREEL_cb);
            this.groupBox3.Controls.Add(this.GRADE_cb);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.SUB_cb);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.ED_cb);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.ForeColor = System.Drawing.Color.Red;
            this.groupBox3.Location = new System.Drawing.Point(38, 523);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(448, 191);
            this.groupBox3.TabIndex = 49;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "清空数据库内容";
            // 
            // ClearData
            // 
            this.ClearData.Location = new System.Drawing.Point(9, 116);
            this.ClearData.Name = "ClearData";
            this.ClearData.Size = new System.Drawing.Size(83, 58);
            this.ClearData.TabIndex = 21;
            this.ClearData.Text = "确认清空";
            this.ClearData.UseVisualStyleBackColor = true;
            this.ClearData.Click += new System.EventHandler(this.ClearData_Click_1);
            // 
            // BREEL_cb
            // 
            this.BREEL_cb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BREEL_cb.FormattingEnabled = true;
            this.BREEL_cb.Location = new System.Drawing.Point(251, 81);
            this.BREEL_cb.Name = "BREEL_cb";
            this.BREEL_cb.Size = new System.Drawing.Size(121, 20);
            this.BREEL_cb.TabIndex = 20;
            // 
            // GRADE_cb
            // 
            this.GRADE_cb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GRADE_cb.FormattingEnabled = true;
            this.GRADE_cb.Location = new System.Drawing.Point(56, 81);
            this.GRADE_cb.Name = "GRADE_cb";
            this.GRADE_cb.Size = new System.Drawing.Size(123, 20);
            this.GRADE_cb.TabIndex = 19;
            this.GRADE_cb.DropDownClosed += new System.EventHandler(this.OnChange);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(12, 84);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 12);
            this.label9.TabIndex = 18;
            this.label9.Text = "年级:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(199, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 12);
            this.label8.TabIndex = 17;
            this.label8.Text = "册别:";
            // 
            // SUB_cb
            // 
            this.SUB_cb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SUB_cb.FormattingEnabled = true;
            this.SUB_cb.Location = new System.Drawing.Point(251, 28);
            this.SUB_cb.Name = "SUB_cb";
            this.SUB_cb.Size = new System.Drawing.Size(121, 20);
            this.SUB_cb.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(199, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 12);
            this.label7.TabIndex = 15;
            this.label7.Text = "学科:";
            // 
            // ED_cb
            // 
            this.ED_cb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ED_cb.FormattingEnabled = true;
            this.ED_cb.Location = new System.Drawing.Point(58, 28);
            this.ED_cb.Name = "ED_cb";
            this.ED_cb.Size = new System.Drawing.Size(121, 20);
            this.ED_cb.Sorted = true;
            this.ED_cb.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(12, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "版本:";
            // 
            // btn_check_con
            // 
            this.btn_check_con.Location = new System.Drawing.Point(44, 69);
            this.btn_check_con.Name = "btn_check_con";
            this.btn_check_con.Size = new System.Drawing.Size(105, 23);
            this.btn_check_con.TabIndex = 48;
            this.btn_check_con.Text = "测试数据库连接";
            this.btn_check_con.UseVisualStyleBackColor = true;
            this.btn_check_con.Click += new System.EventHandler(this.btn_check_con_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBoxError);
            this.groupBox2.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.groupBox2.Location = new System.Drawing.Point(38, 415);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(448, 102);
            this.groupBox2.TabIndex = 47;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "错误列表";
            // 
            // listBoxError
            // 
            this.listBoxError.AllowDrop = true;
            this.listBoxError.FormattingEnabled = true;
            this.listBoxError.HorizontalScrollbar = true;
            this.listBoxError.ItemHeight = 12;
            this.listBoxError.Location = new System.Drawing.Point(6, 16);
            this.listBoxError.MultiColumn = true;
            this.listBoxError.Name = "listBoxError";
            this.listBoxError.ScrollAlwaysVisible = true;
            this.listBoxError.Size = new System.Drawing.Size(427, 76);
            this.listBoxError.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.lb_UploadNo);
            this.groupBox1.Controls.Add(this.lbNew);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 9F);
            this.groupBox1.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.groupBox1.Location = new System.Drawing.Point(38, 355);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(448, 54);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "上传进度";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(24, 20);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(409, 23);
            this.progressBar1.TabIndex = 2;
            // 
            // lb_UploadNo
            // 
            this.lb_UploadNo.AutoSize = true;
            this.lb_UploadNo.Location = new System.Drawing.Point(18, 31);
            this.lb_UploadNo.Name = "lb_UploadNo";
            this.lb_UploadNo.Size = new System.Drawing.Size(0, 12);
            this.lb_UploadNo.TabIndex = 1;
            // 
            // lbNew
            // 
            this.lbNew.AutoSize = true;
            this.lbNew.ForeColor = System.Drawing.Color.Black;
            this.lbNew.Location = new System.Drawing.Point(12, 31);
            this.lbNew.Name = "lbNew";
            this.lbNew.Size = new System.Drawing.Size(0, 12);
            this.lbNew.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(45, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 43;
            this.label4.Text = "路 径：";
            // 
            // txtFileUrl
            // 
            this.txtFileUrl.Location = new System.Drawing.Point(94, 116);
            this.txtFileUrl.Name = "txtFileUrl";
            this.txtFileUrl.ReadOnly = true;
            this.txtFileUrl.Size = new System.Drawing.Size(250, 21);
            this.txtFileUrl.TabIndex = 42;
            // 
            // btn_OpenFile
            // 
            this.btn_OpenFile.Location = new System.Drawing.Point(366, 116);
            this.btn_OpenFile.Name = "btn_OpenFile";
            this.btn_OpenFile.Size = new System.Drawing.Size(120, 21);
            this.btn_OpenFile.TabIndex = 41;
            this.btn_OpenFile.Text = "选择excel模板";
            this.btn_OpenFile.UseVisualStyleBackColor = true;
            this.btn_OpenFile.Click += new System.EventHandler(this.btn_OpenFile_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(103, 163);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 54;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 171);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 55;
            this.label1.Text = "上传位置:";
            // 
            // SelectAll
            // 
            this.SelectAll.AutoSize = true;
            this.SelectAll.Location = new System.Drawing.Point(52, 189);
            this.SelectAll.Name = "SelectAll";
            this.SelectAll.Size = new System.Drawing.Size(102, 16);
            this.SelectAll.TabIndex = 56;
            this.SelectAll.Text = "全选\\取消全选";
            this.SelectAll.UseVisualStyleBackColor = true;
            this.SelectAll.CheckedChanged += new System.EventHandler(this.SelectAll_CheckedChanged);
            // 
            // Lb_sql
            // 
            this.Lb_sql.AutoSize = true;
            this.Lb_sql.Location = new System.Drawing.Point(168, 79);
            this.Lb_sql.Name = "Lb_sql";
            this.Lb_sql.Size = new System.Drawing.Size(65, 12);
            this.Lb_sql.TabIndex = 57;
            this.Lb_sql.Text = "数据库连接";
            // 
            // ResouceSetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 726);
            this.Controls.Add(this.Lb_sql);
            this.Controls.Add(this.SelectAll);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btn_check_con);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtFileUrl);
            this.Controls.Add(this.btn_OpenFile);
            this.Controls.Add(this.label2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ResouceSetForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "资源集导入";
            this.groupBox5.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button Muti_upload;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button ClearData;
        private System.Windows.Forms.ComboBox BREEL_cb;
        private System.Windows.Forms.ComboBox GRADE_cb;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox SUB_cb;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox ED_cb;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btn_check_con;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox listBoxError;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lb_UploadNo;
        private System.Windows.Forms.Label lbNew;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFileUrl;
        private System.Windows.Forms.Button btn_OpenFile;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_upt;
        private System.Windows.Forms.CheckBox SelectAll;
        private System.Windows.Forms.Label Lb_sql;
    }
}