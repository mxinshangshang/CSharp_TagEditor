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
    public partial class ReplaceDialog : Form
    {
        public ReplaceDialog()
        {
            InitializeComponent();
        }
        public RichTextBox rtb;
        int start = 0;           // 查找的起始位置
        string str = "";         // 要替换的内容
        string str2 = "";        // 要替换为的字符串
        RichTextBoxFinds f;
        int i = 0;
        int flag = 0;            //用来标识是否找到过内容，1表示有，0表示无


        private void ReplaceDialog_Load(object sender, EventArgs e)
        {

        }

        private void replacefindnext_Click( object sender, EventArgs e )
        {
            str = this.ReplaceFindContentBox.Text;  // 取得要替换的字符
            start = rtb.Find( str, start, f );
            if (start != 0 && start != -1)
                flag = 1;
            if (start == -1 && flag == 0)
            {
                MessageBox.Show("已到达文件尾部！没有找到与 “ " + str + " ” 相匹配的内容！点击确定从文件开始继续查找！", "查找提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                start = 0;
            }
            else
                if (start == -1 && flag == 1)
                {
                    MessageBox.Show("已到达文件尾部！点击确定从文件开始继续查找！", "查找提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    start = 0;
                }
                else
                {
                    start = start + str.Length; //找到后从找到位置之后开始下一次
                    rtb.Focus(); //给予焦点
                }

            //str = this.ReplaceFindContentBox.Text;  // 取得要替换的字符
            //start = rtb.Find( str, start, f );
            //if ( start == -1 )
            //{
            //    //MessageBox.Show("已到达文件结尾！查找不到与 “ " + str + " ” 内容相匹配的信息！", "替换提示信息", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //    MessageBox.Show("已到达文件结尾！点击确定从文件开始继续查找！", "查找提示信息", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //    start = 0;
            //}
            //else
            //{
            //    start = start + str.Length; // 找到后从找到位置之后开始下一次
            //    rtb.Focus();                // 给予焦点
            //}

        }

        private void replaceone_Click(object sender, EventArgs e)
        {
            str = this.ReplaceFindContentBox.Text;  // 找的内容
            str2 = this.ReplaceContentBoxtBox.Text; // 替换的内容
            if ( start == -1 )
            {
                MessageBox.Show("已到达文件结尾！点击确定从文件开始继续替换！", "替换提示信息", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                start = 0;
            }
            else
            {
                rtb.SelectedText = str2;
                start = start + str.Length; // 找到后从找到位置之后开始下一次
                start = rtb.Find( str, start, f );
                rtb.Focus(); //给予焦点
            }
        }
        private void replaceall_Click(object sender, EventArgs e)
        {
            str = this.ReplaceFindContentBox.Text;  //找的内容
            str2 = this.ReplaceContentBoxtBox.Text;
            start = rtb.Find( str, start, f );
            while ( start != -1 )
            {
                rtb.SelectedText = str2;
                start = start + str.Length;
                start = rtb.Find( str, start, f );
                i++;
            }
            MessageBox.Show("全部替换完毕！全部一共替换了 " + i.ToString() + " 次", "替换完毕信息！");
            start = 0;
        }

        private void replacecancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ReplaceCaseMatch_CheckedChanged(object sender, EventArgs e)
        {

            if (this.ReplaceCaseMatch.Checked == false && this.ReplaceWholeMatch.Checked == false )
                f = RichTextBoxFinds.None;
            else
                f = RichTextBoxFinds.MatchCase;
        
        }

        private void ReplaceWholeMatch_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ReplaceWholeMatch.Checked == false)
                f = RichTextBoxFinds.None;
            else
                f = RichTextBoxFinds.WholeWord;

        }
      
    }
}
