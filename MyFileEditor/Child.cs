using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TagEditor
{
    public partial class Child : Form
    {
        MyEdit f1 = new MyEdit();
        public Child()
        {
            InitializeComponent();
        }

        public void setF1(MyEdit f1)
        {
            this.f1 = f1;
        }

        private void Child_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;  // 当加载Child窗口时填充全部画布
        }
        public RichTextBox getRichTextBox()  // 获取当前的编辑框
        {
            return this.richTextBox1;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            f1.toolStripStatusLabel3.Text = "正在执行：文件读写操作    ";
            f1.toolStripStatusLabel4.Text = "当前文档字数合计：" + this.richTextBox1.Text.Length;
        }

        private void RTBRightClickCopy_Click(object sender, EventArgs e)
        {
            f1.复制ToolStripMenuItem_Click(sender, e);
        }

        private void RTBRightClickCut_Click(object sender, EventArgs e)
        {
            f1.剪切TToolStripMenuItem_Click(sender, e);
        }

        private void RTBRightClickPaste_Click(object sender, EventArgs e)
        {
            f1.粘贴ToolStripMenuItem_Click(sender, e);
        }

        private void RTBRightClickReplace_Click(object sender, EventArgs e)
        {
            f1.替换ToolStripMenuItem_Click(sender, e);
        }

        private void RTBRightClickFind_Click(object sender, EventArgs e)
        {
            f1.查找ToolStripMenuItem_Click(sender, e);
        }

        private void RTBRightClickFont_Click(object sender, EventArgs e)
        {
            f1.字体_Click(sender, e);
        }

        private void RTBRightClickUndo_Click(object sender, EventArgs e)
        {
            f1.撤销ToolStripMenuItem_Click(sender, e);
        }

        private void RTBRightClickClose_Click(object sender, EventArgs e)
        {
            f1.关闭ToolStripMenuItem_Click(sender, e);
        }

        private void RTBRightClick_Click(object sender, EventArgs e)
        {
            f1.清空内容ToolStripMenuItem_Click(sender, e);
        }

        private void RTBRightClickRedo_Click(object sender, EventArgs e)
        {
            f1.恢复RToolStripMenuItem1_Click(sender, e);
        }
        private void RTBRightClickColor_Click(object sender, EventArgs e)
        {
            f1.颜色_Click(sender, e);
        }

        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            f1.r_SelectionChanged(sender, e);
        }

        private void RTBRightClick_Opening(object sender, CancelEventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void richTextBox1_DoubleClick(object sender, EventArgs e)// 双击空文件时，自动调用打开对话框
        {
            RichTextBox x = new RichTextBox();
            x = f1.GetCurrentRichTextBox();
            if (x.Text == "")
                f1.打开OToolStripMenuItem_Click(sender, e);
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e) // 用于打开文本中的超链接
        {
            DialogResult f = MessageBox.Show("确定要打开该连接吗？", "连接提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
            if (f == DialogResult.Yes)
                System.Diagnostics.Process.Start(e.LinkText);
        }
        private void richTextBox1_DragDrop(object sender, DragEventArgs e)
        {


        }
    }
}
