/*****************************************************************************
 * 
 * ReoGrid - .NET Spreadsheet Control
 * 
 * http://reogrid.net
 *
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
 * KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
 * PURPOSE.
 *
 * ReoGrid Demo project released under BSD license.
 * Copyright (c) 2012-2016 unvell.com, all rights reserved.
 * 
 ****************************************************************************/

namespace ResourceToolkit
{
	partial class UptResourceForm
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
            this.grid = new unvell.ReoGrid.ReoGridControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.txbCe = new System.Windows.Forms.TextBox();
            this.txbVersion = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_updatePath = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btn_UpdateData = new System.Windows.Forms.Button();
            this.txbList = new System.Windows.Forms.TextBox();
            this.btn_edit = new System.Windows.Forms.Button();
            this.btn_SelectData = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grid
            // 
            this.grid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.grid.ColumnHeaderContextMenuStrip = null;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.LeadHeaderContextMenuStrip = null;
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grid.Name = "grid";
            this.grid.RowHeaderContextMenuStrip = null;
            this.grid.Script = null;
            this.grid.SheetTabContextMenuStrip = null;
            this.grid.SheetTabNewButtonVisible = true;
            this.grid.SheetTabVisible = true;
            this.grid.SheetTabWidth = 400;
            this.grid.ShowScrollEndSpacing = true;
            this.grid.Size = new System.Drawing.Size(989, 489);
            this.grid.TabIndex = 0;
            this.grid.Text = "reoGridControl1";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.grid);
            this.panel1.Location = new System.Drawing.Point(0, 109);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(993, 493);
            this.panel1.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(185, 83);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "教材目录：";
            // 
            // txbCe
            // 
            this.txbCe.Location = new System.Drawing.Point(271, 45);
            this.txbCe.Margin = new System.Windows.Forms.Padding(2);
            this.txbCe.Name = "txbCe";
            this.txbCe.ReadOnly = true;
            this.txbCe.Size = new System.Drawing.Size(188, 21);
            this.txbCe.TabIndex = 9;
            // 
            // txbVersion
            // 
            this.txbVersion.Location = new System.Drawing.Point(271, 9);
            this.txbVersion.Margin = new System.Windows.Forms.Padding(2);
            this.txbVersion.Name = "txbVersion";
            this.txbVersion.ReadOnly = true;
            this.txbVersion.Size = new System.Drawing.Size(188, 21);
            this.txbVersion.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(185, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "册    别：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(185, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "版    本：";
            // 
            // panel2
            // 
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.btn_updatePath);
            this.panel2.Controls.Add(this.progressBar1);
            this.panel2.Controls.Add(this.btn_UpdateData);
            this.panel2.Controls.Add(this.txbList);
            this.panel2.Controls.Add(this.btn_edit);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.txbCe);
            this.panel2.Controls.Add(this.txbVersion);
            this.panel2.Controls.Add(this.btn_SelectData);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(992, 106);
            this.panel2.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(872, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 12);
            this.label3.TabIndex = 21;
            // 
            // btn_updatePath
            // 
            this.btn_updatePath.Location = new System.Drawing.Point(796, 11);
            this.btn_updatePath.Margin = new System.Windows.Forms.Padding(2);
            this.btn_updatePath.Name = "btn_updatePath";
            this.btn_updatePath.Size = new System.Drawing.Size(62, 41);
            this.btn_updatePath.TabIndex = 18;
            this.btn_updatePath.Text = "更新文件";
            this.btn_updatePath.UseVisualStyleBackColor = true;
            this.btn_updatePath.Click += new System.EventHandler(this.btn_updatePath_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(638, 66);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(220, 29);
            this.progressBar1.TabIndex = 15;
            // 
            // btn_UpdateData
            // 
            this.btn_UpdateData.Location = new System.Drawing.Point(638, 11);
            this.btn_UpdateData.Margin = new System.Windows.Forms.Padding(2);
            this.btn_UpdateData.Name = "btn_UpdateData";
            this.btn_UpdateData.Size = new System.Drawing.Size(63, 41);
            this.btn_UpdateData.TabIndex = 14;
            this.btn_UpdateData.Text = "更新属性";
            this.btn_UpdateData.UseVisualStyleBackColor = true;
            this.btn_UpdateData.Click += new System.EventHandler(this.btn_UpdateData_Click);
            // 
            // txbList
            // 
            this.txbList.Location = new System.Drawing.Point(271, 80);
            this.txbList.Margin = new System.Windows.Forms.Padding(2);
            this.txbList.Name = "txbList";
            this.txbList.ReadOnly = true;
            this.txbList.Size = new System.Drawing.Size(188, 21);
            this.txbList.TabIndex = 13;
            // 
            // btn_edit
            // 
            this.btn_edit.Location = new System.Drawing.Point(33, 2);
            this.btn_edit.Margin = new System.Windows.Forms.Padding(2);
            this.btn_edit.Name = "btn_edit";
            this.btn_edit.Size = new System.Drawing.Size(85, 93);
            this.btn_edit.TabIndex = 12;
            this.btn_edit.Text = "选择修改文件信息";
            this.btn_edit.UseVisualStyleBackColor = true;
            this.btn_edit.Click += new System.EventHandler(this.btn_edit_Click);
            // 
            // btn_SelectData
            // 
            this.btn_SelectData.Location = new System.Drawing.Point(493, 6);
            this.btn_SelectData.Name = "btn_SelectData";
            this.btn_SelectData.Size = new System.Drawing.Size(74, 89);
            this.btn_SelectData.TabIndex = 4;
            this.btn_SelectData.Text = "查询";
            this.btn_SelectData.UseVisualStyleBackColor = true;
            this.btn_SelectData.Click += new System.EventHandler(this.btn_SelectData_Click);
            // 
            // UptResourceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 602);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UptResourceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "更新资源集";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion
        
		private unvell.ReoGrid.ReoGridControl grid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txbCe;
        private System.Windows.Forms.TextBox txbVersion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btn_UpdateData;
        private System.Windows.Forms.TextBox txbList;
        private System.Windows.Forms.Button btn_edit;
        private System.Windows.Forms.Button btn_SelectData;
        private System.Windows.Forms.Button btn_updatePath;
        private System.Windows.Forms.Label label3;
	}
}