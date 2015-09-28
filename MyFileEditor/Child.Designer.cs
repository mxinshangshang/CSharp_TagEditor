namespace TagEditor
{
    partial class Child
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Child));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.RTBRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.RTBRightClickClear = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.RTBRightClickUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.RTBRightClickRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.RTBRightClickCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.RTBRightClickCut = new System.Windows.Forms.ToolStripMenuItem();
            this.RTBRightClickPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.RTBRightClickFont = new System.Windows.Forms.ToolStripMenuItem();
            this.RTBRightClickColor = new System.Windows.Forms.ToolStripMenuItem();
            this.颜色ToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.RTBRightClickReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.RTBRightClickFind = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.RTBRightClickClose = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.RTBRightClick.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.AcceptsTab = true;
            this.richTextBox1.AutoWordSelection = true;
            this.richTextBox1.ContextMenuStrip = this.RTBRightClick;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.EnableAutoDragDrop = true;
            this.richTextBox1.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBox1.HideSelection = false;
            this.richTextBox1.ImeMode = System.Windows.Forms.ImeMode.On;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(299, 197);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.richTextBox1_DragDrop);
            this.richTextBox1.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox1_LinkClicked);
            this.richTextBox1.SelectionChanged += new System.EventHandler(this.richTextBox1_SelectionChanged);
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            this.richTextBox1.DoubleClick += new System.EventHandler(this.richTextBox1_DoubleClick);
            // 
            // RTBRightClick
            // 
            this.RTBRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RTBRightClickClear,
            this.toolStripSeparator3,
            this.RTBRightClickUndo,
            this.RTBRightClickRedo,
            this.toolStripSeparator1,
            this.RTBRightClickCopy,
            this.RTBRightClickCut,
            this.RTBRightClickPaste,
            this.toolStripSeparator4,
            this.RTBRightClickFont,
            this.RTBRightClickColor,
            this.颜色ToolStripMenuItem,
            this.RTBRightClickReplace,
            this.RTBRightClickFind,
            this.toolStripSeparator2,
            this.RTBRightClickClose});
            this.RTBRightClick.Name = "RTBRightClick";
            this.RTBRightClick.Size = new System.Drawing.Size(125, 276);
            this.RTBRightClick.Opening += new System.ComponentModel.CancelEventHandler(this.RTBRightClick_Opening);
            // 
            // RTBRightClickClear
            // 
            this.RTBRightClickClear.Name = "RTBRightClickClear";
            this.RTBRightClickClear.Size = new System.Drawing.Size(124, 22);
            this.RTBRightClickClear.Text = "清空内容";
            this.RTBRightClickClear.Click += new System.EventHandler(this.RTBRightClick_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(121, 6);
            // 
            // RTBRightClickUndo
            // 
            this.RTBRightClickUndo.Image = ((System.Drawing.Image)(resources.GetObject("RTBRightClickUndo.Image")));
            this.RTBRightClickUndo.Name = "RTBRightClickUndo";
            this.RTBRightClickUndo.Size = new System.Drawing.Size(124, 22);
            this.RTBRightClickUndo.Text = "撤销";
            this.RTBRightClickUndo.Click += new System.EventHandler(this.RTBRightClickUndo_Click);
            // 
            // RTBRightClickRedo
            // 
            this.RTBRightClickRedo.Name = "RTBRightClickRedo";
            this.RTBRightClickRedo.Size = new System.Drawing.Size(124, 22);
            this.RTBRightClickRedo.Text = "恢复";
            this.RTBRightClickRedo.Click += new System.EventHandler(this.RTBRightClickRedo_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(121, 6);
            // 
            // RTBRightClickCopy
            // 
            this.RTBRightClickCopy.Image = ((System.Drawing.Image)(resources.GetObject("RTBRightClickCopy.Image")));
            this.RTBRightClickCopy.Name = "RTBRightClickCopy";
            this.RTBRightClickCopy.Size = new System.Drawing.Size(124, 22);
            this.RTBRightClickCopy.Text = "复制";
            this.RTBRightClickCopy.Click += new System.EventHandler(this.RTBRightClickCopy_Click);
            // 
            // RTBRightClickCut
            // 
            this.RTBRightClickCut.Image = ((System.Drawing.Image)(resources.GetObject("RTBRightClickCut.Image")));
            this.RTBRightClickCut.Name = "RTBRightClickCut";
            this.RTBRightClickCut.Size = new System.Drawing.Size(124, 22);
            this.RTBRightClickCut.Text = "剪切";
            this.RTBRightClickCut.Click += new System.EventHandler(this.RTBRightClickCut_Click);
            // 
            // RTBRightClickPaste
            // 
            this.RTBRightClickPaste.Image = ((System.Drawing.Image)(resources.GetObject("RTBRightClickPaste.Image")));
            this.RTBRightClickPaste.Name = "RTBRightClickPaste";
            this.RTBRightClickPaste.Size = new System.Drawing.Size(124, 22);
            this.RTBRightClickPaste.Text = "粘贴";
            this.RTBRightClickPaste.Click += new System.EventHandler(this.RTBRightClickPaste_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(121, 6);
            // 
            // RTBRightClickFont
            // 
            this.RTBRightClickFont.Image = ((System.Drawing.Image)(resources.GetObject("RTBRightClickFont.Image")));
            this.RTBRightClickFont.Name = "RTBRightClickFont";
            this.RTBRightClickFont.Size = new System.Drawing.Size(124, 22);
            this.RTBRightClickFont.Text = "字体";
            this.RTBRightClickFont.Click += new System.EventHandler(this.RTBRightClickFont_Click);
            // 
            // RTBRightClickColor
            // 
            this.RTBRightClickColor.Image = ((System.Drawing.Image)(resources.GetObject("RTBRightClickColor.Image")));
            this.RTBRightClickColor.Name = "RTBRightClickColor";
            this.RTBRightClickColor.Size = new System.Drawing.Size(124, 22);
            this.RTBRightClickColor.Text = "字体颜色";
            this.RTBRightClickColor.Click += new System.EventHandler(this.RTBRightClickColor_Click);
            // 
            // 颜色ToolStripMenuItem
            // 
            this.颜色ToolStripMenuItem.Name = "颜色ToolStripMenuItem";
            this.颜色ToolStripMenuItem.Size = new System.Drawing.Size(121, 6);
            // 
            // RTBRightClickReplace
            // 
            this.RTBRightClickReplace.Name = "RTBRightClickReplace";
            this.RTBRightClickReplace.Size = new System.Drawing.Size(124, 22);
            this.RTBRightClickReplace.Text = "替换";
            this.RTBRightClickReplace.Click += new System.EventHandler(this.RTBRightClickReplace_Click);
            // 
            // RTBRightClickFind
            // 
            this.RTBRightClickFind.Name = "RTBRightClickFind";
            this.RTBRightClickFind.Size = new System.Drawing.Size(124, 22);
            this.RTBRightClickFind.Text = "查找";
            this.RTBRightClickFind.Click += new System.EventHandler(this.RTBRightClickFind_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(121, 6);
            // 
            // RTBRightClickClose
            // 
            this.RTBRightClickClose.Name = "RTBRightClickClose";
            this.RTBRightClickClose.Size = new System.Drawing.Size(124, 22);
            this.RTBRightClickClose.Text = "关闭";
            this.RTBRightClickClose.Click += new System.EventHandler(this.RTBRightClickClose_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // Child
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(299, 197);
            this.Controls.Add(this.richTextBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Child";
            this.Text = "Child";
            this.Load += new System.EventHandler(this.Child_Load);
            this.RTBRightClick.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ContextMenuStrip RTBRightClick;
        private System.Windows.Forms.ToolStripMenuItem RTBRightClickCopy;
        private System.Windows.Forms.ToolStripMenuItem RTBRightClickCut;
        private System.Windows.Forms.ToolStripMenuItem RTBRightClickPaste;
        private System.Windows.Forms.ToolStripMenuItem RTBRightClickFont;
        private System.Windows.Forms.ToolStripMenuItem RTBRightClickClose;
        public System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem RTBRightClickReplace;
        private System.Windows.Forms.ToolStripMenuItem RTBRightClickFind;
        private System.Windows.Forms.ToolStripMenuItem RTBRightClickUndo;
        public System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem RTBRightClickClear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem RTBRightClickRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator 颜色ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem RTBRightClickColor;

    }
}