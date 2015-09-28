namespace TagEditor
{
    partial class ReplaceDialog
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
            this.ReplaceFindContentBox = new System.Windows.Forms.TextBox();
            this.ReplaceContentBoxtBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ReplaceCaseMatch = new System.Windows.Forms.CheckBox();
            this.replacefindnext = new System.Windows.Forms.Button();
            this.replaceone = new System.Windows.Forms.Button();
            this.replaceall = new System.Windows.Forms.Button();
            this.replacecancel = new System.Windows.Forms.Button();
            this.ReplaceWholeMatch = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "查找内容：";
            // 
            // ReplaceFindContentBox
            // 
            this.ReplaceFindContentBox.Location = new System.Drawing.Point(94, 22);
            this.ReplaceFindContentBox.Name = "ReplaceFindContentBox";
            this.ReplaceFindContentBox.Size = new System.Drawing.Size(227, 23);
            this.ReplaceFindContentBox.TabIndex = 1;
            // 
            // ReplaceContentBoxtBox
            // 
            this.ReplaceContentBoxtBox.Location = new System.Drawing.Point(94, 86);
            this.ReplaceContentBoxtBox.Name = "ReplaceContentBoxtBox";
            this.ReplaceContentBoxtBox.Size = new System.Drawing.Size(227, 23);
            this.ReplaceContentBoxtBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "替换内容：";
            // 
            // ReplaceCaseMatch
            // 
            this.ReplaceCaseMatch.AutoSize = true;
            this.ReplaceCaseMatch.Location = new System.Drawing.Point(94, 121);
            this.ReplaceCaseMatch.Name = "ReplaceCaseMatch";
            this.ReplaceCaseMatch.Size = new System.Drawing.Size(87, 21);
            this.ReplaceCaseMatch.TabIndex = 4;
            this.ReplaceCaseMatch.Text = "区分大小写";
            this.ReplaceCaseMatch.UseVisualStyleBackColor = true;
            this.ReplaceCaseMatch.CheckedChanged += new System.EventHandler(this.ReplaceCaseMatch_CheckedChanged);
            // 
            // replacefindnext
            // 
            this.replacefindnext.Location = new System.Drawing.Point(346, 18);
            this.replacefindnext.Name = "replacefindnext";
            this.replacefindnext.Size = new System.Drawing.Size(87, 33);
            this.replacefindnext.TabIndex = 5;
            this.replacefindnext.Text = "查找下一个";
            this.replacefindnext.UseVisualStyleBackColor = true;
            this.replacefindnext.Click += new System.EventHandler(this.replacefindnext_Click);
            // 
            // replaceone
            // 
            this.replaceone.Location = new System.Drawing.Point(346, 58);
            this.replaceone.Name = "replaceone";
            this.replaceone.Size = new System.Drawing.Size(87, 33);
            this.replaceone.TabIndex = 6;
            this.replaceone.Text = "替换";
            this.replaceone.UseVisualStyleBackColor = true;
            this.replaceone.Click += new System.EventHandler(this.replaceone_Click);
            // 
            // replaceall
            // 
            this.replaceall.Location = new System.Drawing.Point(346, 99);
            this.replaceall.Name = "replaceall";
            this.replaceall.Size = new System.Drawing.Size(87, 33);
            this.replaceall.TabIndex = 7;
            this.replaceall.Text = "全部替换";
            this.replaceall.UseVisualStyleBackColor = true;
            this.replaceall.Click += new System.EventHandler(this.replaceall_Click);
            // 
            // replacecancel
            // 
            this.replacecancel.Location = new System.Drawing.Point(346, 138);
            this.replacecancel.Name = "replacecancel";
            this.replacecancel.Size = new System.Drawing.Size(87, 33);
            this.replacecancel.TabIndex = 8;
            this.replacecancel.Text = "取消";
            this.replacecancel.UseVisualStyleBackColor = true;
            this.replacecancel.Click += new System.EventHandler(this.replacecancel_Click);
            // 
            // ReplaceWholeMatch
            // 
            this.ReplaceWholeMatch.AutoSize = true;
            this.ReplaceWholeMatch.Location = new System.Drawing.Point(94, 151);
            this.ReplaceWholeMatch.Name = "ReplaceWholeMatch";
            this.ReplaceWholeMatch.Size = new System.Drawing.Size(87, 21);
            this.ReplaceWholeMatch.TabIndex = 9;
            this.ReplaceWholeMatch.Text = "全字符匹配";
            this.ReplaceWholeMatch.UseVisualStyleBackColor = true;
            this.ReplaceWholeMatch.CheckedChanged += new System.EventHandler(this.ReplaceWholeMatch_CheckedChanged);
            // 
            // ReplaceDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(448, 181);
            this.Controls.Add(this.ReplaceWholeMatch);
            this.Controls.Add(this.replacecancel);
            this.Controls.Add(this.replaceall);
            this.Controls.Add(this.replaceone);
            this.Controls.Add(this.replacefindnext);
            this.Controls.Add(this.ReplaceCaseMatch);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ReplaceContentBoxtBox);
            this.Controls.Add(this.ReplaceFindContentBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReplaceDialog";
            this.Text = "替换";
            this.Load += new System.EventHandler(this.ReplaceDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ReplaceFindContentBox;
        private System.Windows.Forms.TextBox ReplaceContentBoxtBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox ReplaceCaseMatch;
        private System.Windows.Forms.Button replacefindnext;
        private System.Windows.Forms.Button replaceone;
        private System.Windows.Forms.Button replaceall;
        private System.Windows.Forms.Button replacecancel;
        private System.Windows.Forms.CheckBox ReplaceWholeMatch;
    }
}