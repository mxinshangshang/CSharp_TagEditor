namespace TagEditor
{
    partial class FindDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.CaseMatch = new System.Windows.Forms.CheckBox();
            this.findcancel = new System.Windows.Forms.Button();
            this.findnext = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.FindContentBox = new System.Windows.Forms.TextBox();
            this.WholeMatch = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 17);
            this.label1.TabIndex = 2;
            // 
            // CaseMatch
            // 
            this.CaseMatch.AutoSize = true;
            this.CaseMatch.Location = new System.Drawing.Point(98, 57);
            this.CaseMatch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CaseMatch.Name = "CaseMatch";
            this.CaseMatch.Size = new System.Drawing.Size(87, 21);
            this.CaseMatch.TabIndex = 9;
            this.CaseMatch.Text = "区分大小写";
            this.CaseMatch.UseVisualStyleBackColor = true;
            this.CaseMatch.CheckedChanged += new System.EventHandler(this.CaseMatch_CheckedChanged);
            // 
            // findcancel
            // 
            this.findcancel.Location = new System.Drawing.Point(314, 76);
            this.findcancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.findcancel.Name = "findcancel";
            this.findcancel.Size = new System.Drawing.Size(103, 29);
            this.findcancel.TabIndex = 8;
            this.findcancel.Text = "取消";
            this.findcancel.UseVisualStyleBackColor = true;
            this.findcancel.Click += new System.EventHandler(this.findcancel_Click);
            // 
            // findnext
            // 
            this.findnext.Location = new System.Drawing.Point(314, 16);
            this.findnext.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.findnext.Name = "findnext";
            this.findnext.Size = new System.Drawing.Size(103, 29);
            this.findnext.TabIndex = 7;
            this.findnext.Text = "查找下一个";
            this.findnext.UseVisualStyleBackColor = true;
            this.findnext.Click += new System.EventHandler(this.findnext_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "查找内容：";
            // 
            // FindContentBox
            // 
            this.FindContentBox.Location = new System.Drawing.Point(98, 17);
            this.FindContentBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FindContentBox.Name = "FindContentBox";
            this.FindContentBox.Size = new System.Drawing.Size(198, 23);
            this.FindContentBox.TabIndex = 5;
            this.FindContentBox.TextChanged += new System.EventHandler(this.FindContentBox_TextChanged);
            // 
            // WholeMatch
            // 
            this.WholeMatch.AutoSize = true;
            this.WholeMatch.Location = new System.Drawing.Point(98, 86);
            this.WholeMatch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.WholeMatch.Name = "WholeMatch";
            this.WholeMatch.Size = new System.Drawing.Size(87, 21);
            this.WholeMatch.TabIndex = 10;
            this.WholeMatch.Text = "全字符匹配";
            this.WholeMatch.UseVisualStyleBackColor = true;
            this.WholeMatch.CheckedChanged += new System.EventHandler(this.WholeMatch_CheckedChanged);
            // 
            // FindDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 123);
            this.Controls.Add(this.WholeMatch);
            this.Controls.Add(this.CaseMatch);
            this.Controls.Add(this.findcancel);
            this.Controls.Add(this.findnext);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FindContentBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FindDialog";
            this.Text = "查找";
            this.Load += new System.EventHandler(this.FindDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox CaseMatch;
        private System.Windows.Forms.Button findcancel;
        private System.Windows.Forms.Button findnext;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox FindContentBox;
        private System.Windows.Forms.CheckBox WholeMatch;
    }
}