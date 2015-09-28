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
    public partial class FindDialog : Form
    {
        public FindDialog()
        {
            InitializeComponent();
        }

        public RichTextBox rtb;
        int start = 0;//查找的起始位置
        string str = "";//查找的内容
        int flag = 0;

        RichTextBoxFinds option1;

        public void findnext_Click(object sender, EventArgs e)
        {
            str = this.FindContentBox.Text;   //取得要查找的字符串

            start = rtb.Find(str, start, option1 );
            if (start != 0 && start != -1)
                flag = 1;
            if ( start == -1 &&flag == 0 )
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
        }

        private void findcancel_Click( object sender, EventArgs e )
        {
            this.Close();
        }

        private void FindDialog_Load(object sender, EventArgs e)
        {

        }

        private void CaseMatch_CheckedChanged(object sender, EventArgs e)
        {
            if ( this.CaseMatch.Checked == false )
                option1 = RichTextBoxFinds.None;
            else
                option1 = RichTextBoxFinds.MatchCase;
        }

        private void FindContentBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void WholeMatch_CheckedChanged(object sender, EventArgs e)
        {
            if (this.WholeMatch.Checked == false)
                option1 = RichTextBoxFinds.None;
            else
                option1 = RichTextBoxFinds.WholeWord;

        }

     
    }
}
