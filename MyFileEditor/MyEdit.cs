using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;
using System.Collections;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Data.OleDb;
using System.Drawing.Drawing2D;

namespace TagEditor
{
    public partial class MyEdit : Form
    {
        private int tabNum = 0;                // 标签数
        RichTextBox r;
        private string[] fontFamilyNames;

        private string SamplePath;
        private string ExcelPath;
        private DataSet myDataSet;

        public string id = null;

        public MyEdit()
        {
            InitializeComponent();
            GetFontFamilies();
            ts_addItems();
        }

        public RichTextBox GetCurrentRichTextBox()
        {
            r = new RichTextBox(); //初始化RichTextBox实例

            TabPage tabPage = new TabPage();  // 新建标签
            tabPage = tabControl1.SelectedTab;
            if (tabControl1.TabCount > 0)    //选项卡数量大于0
            {

                foreach (Control c in tabPage.Controls)
                {
                    if (c.GetType() == typeof(Child))
                    {
                        Child form = c as Child;
                        return form.getRichTextBox();
                    }
                }
            }
            return r;
        }


        public Child getChildForm()
        {
            Child form1 = new Child();
            TabPage tabPage = new TabPage();
            tabPage = tabControl1.SelectedTab;
            if (tabControl1.TabCount > 0)
            {
                foreach (Control c in tabPage.Controls)
                {
                    if (c.GetType() == typeof(Child))
                    {

                        Child form = c as Child;
                        return form;
                    }
                }
            }
            return form1;
        }

        private string GetPrn(string[] Prm)                                                   //整合Prn内容
        {
            SamplePath = Properties.Settings.Default.SamplePathSetting;
            string path = SamplePath + "/";
            string[] text = File.ReadAllLines(path + "/" + "area.txt");
            try
            {
                for (int u = 0; u < text.Length; u++)
                {
                    if (text[u].Contains("area"))
                    {
                        int s = Int32.Parse(Regex.Match(text[u], @"area([\s\S]*?)""").Groups[1].Value);
                        text[u] = text[u].Replace(Regex.Match(text[u], @"""([\s\S]*?)""").Groups[1].ToString(), Prm[s - 1]);
                    }
                    if (text[u].Contains("111111118"))
                    {
                        text[u] = text[u].Replace(Regex.Match(text[u], @"""([\s\S]*?)""").Groups[1].ToString(), Prm[2]);
                    }
                    else continue;
                }
                return string.Join("\r\n", text);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        private string[] GetExcel(string target)                                              //读入Excel内容
        {
            int index = 0;
            int findit = 0;
            string[] Prm;
            ExcelPath = Properties.Settings.Default.ExcelPathSetting;
            string path = ExcelPath + "/";
            string strCon = " Provider = Microsoft.Jet.OLEDB.4.0 ; Data Source = " + path + "AREA.xls;Extended Properties='Excel 8.0;HDR=NO;IMEX=1;'";    //创建一个数据链接
            OleDbConnection myConn = new OleDbConnection(strCon);
            string strCom = " SELECT * FROM [Sheet1$] ";
            try
            {
                myConn.Open();
                OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);                  //打开数据链接，得到一个数据集
                myDataSet = new DataSet();                                                          //创建一个 DataSet对象
                myCommand.Fill(myDataSet, "[Sheet1$]");                                             //得到自己的DataSet对象
                myConn.Close();                                                                     //关闭此数据链接

                Prm = new string[myDataSet.Tables[0].Columns.Count];

                for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
                {
                    if (target == myDataSet.Tables[0].Rows[i].ItemArray[1].ToString())              //获取要打印的ID号所在的行号
                    {
                        index = i;
                        findit = 1;
                        break;
                    }
                }

                if (findit != 0)
                {
                    for (int i = 0; i < myDataSet.Tables[0].Columns.Count; i++)                         //获取ID对应的所有需要打印的信息
                    {
                        Prm[i] = myDataSet.Tables[0].Rows[index].ItemArray[i].ToString();
                    }
                    return Prm;
                }
                else
                {
                    MessageBox.Show("未找到对应ID的标签！！");
                    return null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        public Child getChildForm1(int i)
        {
            Child form1 = new Child();
            TabPage tabPage = new TabPage();
            tabPage = tabControl1.TabPages[i];
            if (tabControl1.TabCount > 0)
            {
                foreach (Control c in tabPage.Controls)
                {
                    if (c.GetType() == typeof(Child))
                    {

                        Child form = c as Child;

                        return form;
                    }
                }
            }
            return form1;
        }
        private void 新建NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";    //第一个在对话框中显示的文件或最后一个选取的文件,开始设置为空
            Child child = new Child();             //建立子窗口
            child.setF1(this);
            child.openFileDialog1.FileName = openFileDialog1.FileName; // 将现有属性添加到子窗口
            child.saveFileDialog1.FileName = saveFileDialog1.FileName;

            //关键语句
            child.TopLevel = false;
            child.TopMost = false;
            child.ControlBox = true;
            child.FormBorderStyle = FormBorderStyle.None;
            child.Dock = DockStyle.Fill;
            TabPage newPage = new TabPage();
            tabNum += 1;     //标签数加1

            newPage.Text = "未命名 " + tabNum.ToString();
            newPage.Parent = tabControl1;
            child.Parent = newPage;
            child.Show();
            tabControl1.SelectedTab = newPage;

        }

        public void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "打开文件...";
            openFileDialog1.Filter = "富格式文件(*.rtf)|*.rtf|文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.InitialDirectory = "桌面";
            openFileDialog1.ShowReadOnly = true;
            openFileDialog1.ReadOnlyChecked = false;
            openFileDialog1.FileName = "";


            Child child = new Child();
            child.setF1(this);
            //关键语句
            child.TopLevel = false;
            child.TopMost = false;
            child.ControlBox = true;
            child.FormBorderStyle = FormBorderStyle.None;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                int i = 0;
                Text = openFileDialog1.FileName;
                for (i = 0; i < tabControl1.TabCount; i++)
                {
                    Child child2 = getChildForm1(i);

                    if (openFileDialog1.FileName.Equals(child2.openFileDialog1.FileName))
                    {
                        break;
                    }
                }
                if (i == tabControl1.TabCount)
                {
                    RichTextBox f = child.getRichTextBox();


                    if (Path.GetExtension(openFileDialog1.FileName) == ".rtf")
                        f.LoadFile(openFileDialog1.FileName.ToString(), RichTextBoxStreamType.RichText);
                    else
                        f.LoadFile(openFileDialog1.FileName.ToString(), RichTextBoxStreamType.PlainText);

                    child.openFileDialog1.FileName = openFileDialog1.FileName.ToString();

                    TabPage newPage = new TabPage();

                    newPage.Text = openFileDialog1.SafeFileName.ToString();
                    newPage.Parent = tabControl1;
                    child.Parent = newPage;
                    child.Show();
                    tabControl1.SelectedTab = newPage;
                }
                else
                {
                    tabControl1.SelectedTab = tabControl1.TabPages[i];
                }
            }
            openFileDialog1.FileName = "";
        }

        private void 文件FToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            r = GetCurrentRichTextBox();
            Child f = getChildForm();

            if (r.Text == "")
            {
                MessageBox.Show("当前文档内容为空，不允许保存空文件！", "提示");
                return;
            }

            if (f.openFileDialog1.FileName == "")//当前编辑的文件是新建的新文件
            {
                TabPage tabPage3 = tabControl1.SelectedTab;
                saveFileDialog1.Title = "保存";
                saveFileDialog1.DefaultExt = "*.rtf";
                saveFileDialog1.Filter = "文本文件(*.txt)|*.txt|富格式文件(*.rtf)|*.rtf|所有文件(*.*)|*.*";
                if (f.saveFileDialog1.FileName == "") //当前编辑的文件没有保存过
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK && saveFileDialog1.FileName.Length > 0)
                    {
                        tabPage3.Text = saveFileDialog1.FileName;
                        f.saveFileDialog1.FileName = saveFileDialog1.FileName.ToString();

                        // 如果扩展名为rtf格式，则以RTF格式保存文件，否则以普通文本格式保存文件
                        if (Path.GetExtension(f.saveFileDialog1.FileName) == ".rtf")
                            f.richTextBox1.SaveFile(f.saveFileDialog1.FileName, RichTextBoxStreamType.RichText);
                        else
                            f.richTextBox1.SaveFile(f.saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);

                        r.Modified = false;

                    }
                    else
                    {
                        MessageBox.Show("您的文件尚未保存！", "提示");
                        return;
                    }
                }
                else //文件已经保存过了
                {
                    // 如果扩展名为rtf格式，则以RTF格式保存文件，否则以普通文本格式保存文件
                    if (Path.GetExtension(f.saveFileDialog1.FileName) == ".rtf")
                        f.richTextBox1.SaveFile(f.saveFileDialog1.FileName, RichTextBoxStreamType.RichText);
                    else
                        f.richTextBox1.SaveFile(f.saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                }
            }
            else//当前编辑的文件是打开的已存在文件
            {
                // 如果扩展名为rtf格式，则以RTF格式保存文件，否则以普通文本格式保存文件
                if (Path.GetExtension(f.openFileDialog1.FileName) == ".rtf")
                    f.richTextBox1.SaveFile(f.openFileDialog1.FileName, RichTextBoxStreamType.RichText);
                else
                    f.richTextBox1.SaveFile(f.openFileDialog1.FileName, RichTextBoxStreamType.PlainText);
            }
            saveFileDialog1.FileName = "";
            openFileDialog1.FileName = "";
        }

        private void 另存为AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox r = GetCurrentRichTextBox();
            Child f = getChildForm();
            saveFileDialog1.Title = "另存为...";
            saveFileDialog1.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            saveFileDialog1.InitialDirectory = "桌面";
            saveFileDialog1.FileName = "";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                f.saveFileDialog1.FileName = saveFileDialog1.FileName.ToString();
                f.richTextBox1.SaveFile(f.saveFileDialog1.FileName, RichTextBoxStreamType.RichText);
            }
            saveFileDialog1.FileName = "";
            openFileDialog1.FileName = "";
        }

        private void 打印PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox r = GetCurrentRichTextBox();
            IDlogin ID = new IDlogin();
            ID.GetForm(this);
            ID.ShowDialog();

            PrintDocument.DefaultPageSettings.PaperSize = new PaperSize("Custum", 315, 236);
            PrintDocument.PrintPage += new PrintPageEventHandler(MyPrintDocument_PrintPage);

            printDialog1.AllowPrintToFile = true;
            printDialog1.Document = PrintDocument;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                PrintDocument.Print();
            }
        }

        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = 0;
            for (i = tabControl1.TabCount - 1; i >= 0; i--)
            {
                Child child2 = getChildForm1(i);
                RichTextBox n = child2.getRichTextBox();
                if (n.Modified == true)  //文档被修改了
                {
                    DialogResult f = MessageBox.Show("当前标签中的文档内容尚未保存！\n是否要保存?", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                    n.Focus();      //给予焦点
                    if (f == DialogResult.Yes)
                    {
                        保存SToolStripMenuItem_Click(sender, e);
                        tabControl1.TabPages.RemoveAt(i);
                    }
                    else if (f == DialogResult.No)
                    {
                        tabControl1.TabPages.RemoveAt(i);
                    }
                    else
                    {
                        break;
                    }
                }
                else//文档没有被修改，直接关闭所在的标签
                {
                    tabControl1.TabPages.RemoveAt(i);
                }

            }

            if (i == -1)
            {
                Application.Exit();
            }
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            r = GetCurrentRichTextBox();

            toolStripStatusLabel3.Text = "正在执行：文件读写操作    ";
            toolStripStatusLabel4.Text = "当前文档字数合计：" + r.Text.Length;
            新建NToolStripMenuItem_Click(sender, e);               //打开编辑器时，首先建立一个标签
        }
        private void MyEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.Cancel == false)
                退出XToolStripMenuItem_Click(sender, e);
        }

        private void 全选AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            r = GetCurrentRichTextBox();
            r.SelectAll();
        }

        public void 替换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //RichTextBox r = new RichTextBox();
            r = GetCurrentRichTextBox();

            ReplaceDialog f = new ReplaceDialog();
            f.rtb = r; //传值（从主窗口传到FindForm）
            f.Owner = this; //悬浮于当前窗体
            f.Show();
        }

        public void 查找ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            r = GetCurrentRichTextBox();
            FindDialog f = new FindDialog();
            f.rtb = r;                     //传值（从主窗口传到FindForm）
            f.Owner = this;               //悬浮于当前窗体
            f.Show();

        }

        public void 剪切TToolStripMenuItem_Click(object sender, EventArgs e)
        {

            r = GetCurrentRichTextBox();
            if (r.SelectedText == "")
                return;
            else
                r.Cut();
        }


        public void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            r = GetCurrentRichTextBox();
            if (r.SelectedText == "")
                return;
            else
                r.Copy();
        }

        public void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            r = GetCurrentRichTextBox();
            r.Paste();
        }

        public void 撤销ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            r = GetCurrentRichTextBox();
            r.Undo();
        }

        private void 打印预览VToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //pageSetupDialog1.Document = PrintDocument;
            //pageSetupDialog1.ShowDialog();

            // printDocument1 为 打印控件
            //设置打印用的纸张 当设置为Custom的时候，可以自定义纸张的大小，还可以选择A4,A5等常用纸型
            PrintDocument.DefaultPageSettings.PaperSize = new PaperSize("Custum", 262, 197);
            PrintDocument.PrintPage += new PrintPageEventHandler(MyPrintDocument_PrintPage);
            //将写好的格式给打印预览控件以便预览
            printPreviewDialog1.Document = PrintDocument;
            //显示打印预览
            DialogResult result = printPreviewDialog1.ShowDialog();
        }

        private void MyPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            string[] Prm;
            string[] Prm1;
            r = GetCurrentRichTextBox();
            Prm = GetExcel(id);
            Prm1 = GetExcel("area2");

            /*如果需要改变自己 可以在new Font(new FontFamily("黑体"),11）中的“黑体”改成自己要的字体就行了，黑体 后面的数字代表字体的大小
             System.Drawing.Brushes.Blue , 170, 10 中的 System.Drawing.Brushes.Blue 为颜色，后面的为输出的位置 ，第一个10是左边距，第二个35是上边距*/
            //e.Graphics.DrawString(GetCurrentRichTextBox().Text, new Font(new FontFamily("Arial"), 11), Brushes.Black, 0, 0);
            for (int j = Prm1.Length-1; j >=0; j--)
            {
                if (r.Text.Contains(Prm1[j]))
                {
                    r.Text = r.Text.Replace(Prm1[j], Prm[j]);
                }
            }
            e.Graphics.DrawString(r.Text, r.SelectionFont, Brushes.Black, 0, 0);
            Code39 _Code39 = new Code39();
            _Code39.Height = 19;
            _Code39.Magnify = 0x00;
            _Code39.ViewFont = new Font("Arial", 20);
            PointF point = new Point(286, 90);
            //e.Graphics.TranslateTransform(0, 197);
            //e.Graphics.RotateTransform(-90.0F);
            e.Graphics.DrawImage(_Code39.GetCodeImage(Prm[2], Code39.Code39Model.Code39Normal, true), point);
        }

        public void 字体_Click(object sender, EventArgs e)
        {
            r = GetCurrentRichTextBox();
            fontDialog1.ShowEffects = true;
            fontDialog1.Font = r.SelectionFont;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                r.SelectionFont = fontDialog1.Font;
            }
        }

        public void 颜色_Click(object sender, EventArgs e)
        {
            r = GetCurrentRichTextBox();
            colorDialog1.AnyColor = true;
            colorDialog1.Color = r.SelectionColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                r.SelectionColor = colorDialog1.Color;
            }
        }

        private void 自动换行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            r = GetCurrentRichTextBox();
            if (自动换行ToolStripMenuItem.Checked == false)
            {
                自动换行ToolStripMenuItem.Checked = true;
                r.WordWrap = true;
            }
            else
            {
                自动换行ToolStripMenuItem.Checked = false;
                r.WordWrap = false;
            }
        }

        public void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)  //关闭当前标签
        {
            if (tabNum == 0)  // 当有0个标签是，不能关闭
                return;
            r = GetCurrentRichTextBox();
            if (r.Modified == true)
            {
                DialogResult z = MessageBox.Show("您正要关闭的标签中，文件内容已经修改！\n是否要保存当前文件?", "提示！", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

                if (z == DialogResult.Yes)
                {
                    保存SToolStripMenuItem_Click(sender, e);

                    tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
                    tabNum--;
                }
                else if (z == DialogResult.No)
                {
                    tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
                    tabNum--;
                }
                else  //其他操作不响应
                {

                }


            }
            else
            {
                tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
                tabNum--;
            }

        }



        private void GetFontFamilies()
        {
            Graphics g = CreateGraphics();
            FontFamily[] ffs = FontFamily.Families;
            fontFamilyNames = new string[ffs.Length];
            for (int i = 0; i < ffs.Length; i++)
            {
                fontFamilyNames[i] = ffs[i].Name;
                //tSComboBoxFont.Items.Add(fontFamilyNames[i]);  // 逐个添加字体
            }
            tf.Items.AddRange(fontFamilyNames);      //一次性添加所有字体
        }

        private void ts_addItems()
        {

            for (int i = 0; i <= 50; i++)
            {
                ts.Items.Add(i.ToString());
            }
        }


        private void tSBtnLeft_Click(object sender, EventArgs e)
        {
            r = GetCurrentRichTextBox();
            r.SelectionAlignment = HorizontalAlignment.Left;
            setAlign(HorizontalAlignment.Left);
        }

        private void tSBtnCenter_Click(object sender, EventArgs e)
        {
            r = GetCurrentRichTextBox();
            r.SelectionAlignment = HorizontalAlignment.Center;
            setAlign(HorizontalAlignment.Center);
        }

        private void tSBtnRight_Click(object sender, EventArgs e)
        {
            r = GetCurrentRichTextBox();
            r.SelectionAlignment = HorizontalAlignment.Right;
            setAlign(HorizontalAlignment.Right);
        }

        private void setAlign(HorizontalAlignment align)
        {
            tSBtnLeft.Checked = false;
            tSBtnCenter.Checked = false;
            tSBtnRight.Checked = false;
            switch (align)
            {
                case HorizontalAlignment.Left:
                    tSBtnLeft.Checked = true;
                    break;
                case HorizontalAlignment.Center:
                    tSBtnCenter.Checked = true;
                    break;
                case HorizontalAlignment.Right:
                    tSBtnRight.Checked = true;
                    break;
            }
        }

        private void tf_SelectedIndexChanged(object sender, EventArgs e)
        {
            r = GetCurrentRichTextBox();
            float fontSize;
            if (ts.SelectedIndex == -1)
                fontSize = 14;
            else
                fontSize = float.Parse(ts.SelectedItem.ToString());
            if (r.SelectedText.Length > 0)
            {
                r.SelectionFont = new Font(tf.Text, fontSize);
            }
        }

        private void ts_SelectedIndexChanged(object sender, EventArgs e)
        {
            r = GetCurrentRichTextBox();
            string fontname;
            if (tf.SelectedIndex == -1)
                fontname = "Arial";
            else
                fontname = tf.Text;
            if (r.SelectedText.Length > 0)
                r.SelectionFont = new Font(fontname, float.Parse(ts.SelectedItem.ToString()));
        }

        public void r_SelectionChanged(object sender, EventArgs e)
        {
            r = GetCurrentRichTextBox();

            Font currFont;
            //说明：判断有无选择字符串可根据SelectionLength来判断；
            //SelectionFont 等于null 并不等于没有选择字符串，可能是由于所选择的
            //   字符串中同时包含多个字体
            if (r.SelectionFont != null)
            {
                currFont = r.SelectionFont;
                tf.SelectedIndex = tf.FindString(currFont.Name);

                //设置粗体按钮
                //if ((editor.SelectionFont.Style & FontStyle.Bold) == FontStyle.Bold) tSBtnBold.Checked = true; else tSBtnBold.Checked = false;
                tSBtnBold.Checked = r.SelectionFont.Bold;
                tSBtnItalic.Checked = r.SelectionFont.Italic;
                tSBtnUnderline.Checked = r.SelectionFont.Underline;
            }
            else
                tf.SelectedIndex = -1;

            toolStripStatusLabel3.Text = "（光标）当前位置：行：" + r.GetLineFromCharIndex(r.SelectionStart).ToString();
            toolStripStatusLabel3.Text += "  列：" + (r.SelectionStart - r.GetFirstCharIndexOfCurrentLine()).ToString();
            setAlign(r.SelectionAlignment);  //设置对齐方式按钮的多选一效果
        }

        private void tSBtnBold_Click(object sender, EventArgs e)
        {
            r = GetCurrentRichTextBox();
            if (r.SelectionFont.Bold)
                r.SelectionFont = new Font(r.SelectionFont, r.SelectionFont.Style ^ FontStyle.Bold);
            else
                r.SelectionFont = new Font(r.SelectionFont, r.SelectionFont.Style | FontStyle.Bold);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tSBtnItalic_Click(object sender, EventArgs e)
        {
            r = GetCurrentRichTextBox();
            if (r.SelectionFont.Italic)
                r.SelectionFont = new Font(r.SelectionFont, r.SelectionFont.Style ^ FontStyle.Italic);
            else
                r.SelectionFont = new Font(r.SelectionFont, r.SelectionFont.Style | FontStyle.Italic);

        }

        private void tSBtnUnderline_Click(object sender, EventArgs e)
        {
            r = GetCurrentRichTextBox();
            if (r.SelectionFont.Underline)
                r.SelectionFont = new Font(r.SelectionFont, r.SelectionFont.Style ^ FontStyle.Underline);
            else
                r.SelectionFont = new Font(r.SelectionFont, r.SelectionFont.Style | FontStyle.Underline);

        }

        public void 恢复RToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            r = GetCurrentRichTextBox();
            r.Redo();

        }

        public void 清空内容ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            r = GetCurrentRichTextBox();
            r.Clear();
            r.Modified = true;  //默认情况下，Clear()后，修改标志也被清除，这里设为true

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void TabRightClick_Opening(object sender, CancelEventArgs e)
        {

        }

        private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {

        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 文件FToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void 关于本程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutDialog about = new AboutDialog();
            if (about.ShowDialog() == DialogResult.OK)
                return;
        }

        private void 帮助HToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Code39 code = new Code39();
            //code.saveFile()
        }

        private void toolStripStatusLabel3_Click(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;
            Activate();
        }

        private void MyEdit_Resize(object sender, EventArgs e)
        {
            //if (WindowState == FormWindowState.Minimized)
                //Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, EventArgs e)
        {
            notifyIcon1_MouseDoubleClick(sender, e);
        }

        private void 标签内容表格路径设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "请选择标签内容表格所在路径";
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                ExcelPath = folderBrowserDialog.SelectedPath;
            }
            Properties.Settings.Default.ExcelPathSetting = ExcelPath;
            Properties.Settings.Default.Save();
        }

        public static Bitmap GetThumbnail(Bitmap b, int destHeight, int destWidth)
        {
            System.Drawing.Image imgSource = b;
            System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
            int sW = 0, sH = 0;
            // 按比例缩放           
            int sWidth = imgSource.Width;
            int sHeight = imgSource.Height;
            if (sHeight > destHeight || sWidth > destWidth)
            {
                if ((sWidth * destHeight) > (sHeight * destWidth))
                {
                    sW = destWidth;
                    sH = (destWidth * sHeight) / sWidth;
                }
                else
                {
                    sH = destHeight;
                    sW = (sWidth * destHeight) / sHeight;
                }
            }
            else
            {
                sW = sWidth;
                sH = sHeight;
            }
            Bitmap outBmp = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.Transparent);
            // 设置画布的描绘质量         
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgSource, new Rectangle((destWidth - sW) / 2, (destHeight - sH) / 2, sW, sH), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            // 以下代码为保存图片时，设置压缩质量     
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            imgSource.Dispose();
            return outBmp;
        }
    }

    /// <summary>
    /// Code39编码
    /// </summary>
    public class Code39
    {
        private Hashtable m_Code39 = new Hashtable();
        private byte m_Magnify = 0;
        /// <summary>
        /// 放大倍数
        /// </summary>
        public byte Magnify { get { return m_Magnify; } set { m_Magnify = value; } }

        private int m_Height = 40;
        /// <summary>
        /// 图形高
        /// </summary>
        public int Height { get { return m_Height; } set { m_Height = value; } }

        private Font m_ViewFont = null;
        /// <summary>
        /// 字体大小
        /// </summary>
        public Font ViewFont { get { return m_ViewFont; } set { m_ViewFont = value; } }

        public Code39()
        {

            m_Code39.Add("A", "1101010010110");
            m_Code39.Add("B", "1011010010110");
            m_Code39.Add("C", "1101101001010");
            m_Code39.Add("D", "1010110010110");
            m_Code39.Add("E", "1101011001010");
            m_Code39.Add("F", "1011011001010");
            m_Code39.Add("G", "1010100110110");
            m_Code39.Add("H", "1101010011010");
            m_Code39.Add("I", "1011010011010");
            m_Code39.Add("J", "1010110011010");
            m_Code39.Add("K", "1101010100110");
            m_Code39.Add("L", "1011010100110");
            m_Code39.Add("M", "1101101010010");
            m_Code39.Add("N", "1010110100110");
            m_Code39.Add("O", "1101011010010");
            m_Code39.Add("P", "1011011010010");
            m_Code39.Add("Q", "1010101100110");
            m_Code39.Add("R", "1101010110010");
            m_Code39.Add("S", "1011010110010");
            m_Code39.Add("T", "1010110110010");
            m_Code39.Add("U", "1100101010110");
            m_Code39.Add("V", "1001101010110");
            m_Code39.Add("W", "1100110101010");
            m_Code39.Add("X", "1001011010110");
            m_Code39.Add("Y", "1100101101010");
            m_Code39.Add("Z", "1001101101010");
            m_Code39.Add("0", "1010011011010");
            m_Code39.Add("1", "1101001010110");
            m_Code39.Add("2", "1011001010110");
            m_Code39.Add("3", "1101100101010");
            m_Code39.Add("4", "1010011010110");
            m_Code39.Add("5", "1101001101010");
            m_Code39.Add("6", "1011001101010");
            m_Code39.Add("7", "1010010110110");
            m_Code39.Add("8", "1101001011010");
            m_Code39.Add("9", "1011001011010");
            m_Code39.Add("+", "1001010010010");
            m_Code39.Add("-", "1001010110110");
            m_Code39.Add("*", "1001011011010");
            m_Code39.Add("/", "1001001010010");
            m_Code39.Add("%", "1010010010010");
            m_Code39.Add("$", "1001001001010");
            m_Code39.Add(".", "1100101011010");
            m_Code39.Add(" ", "1001101011010");
        }

        public enum Code39Model
        {
            /// <summary>
            /// 基本类别 1234567890ABC
            /// </summary>
            Code39Normal,
            /// <summary>
            /// 全ASCII方式 +A+B 来表示小写
            /// </summary>
            Code39FullAscII
        }
        /// <summary>
        /// 获得条码图形
        /// </summary>
        /// <param name="p_Text">文字信息</param>
        /// <param name="p_Model">类别</param>
        /// <param name="p_StarChar">是否增加前后*号</param>
        /// <returns>图形</returns>
        public Bitmap GetCodeImage(string p_Text, Code39Model p_Model, bool p_StarChar)
        {
            string _ValueText = "";
            string _CodeText = "";
            char[] _ValueChar = null;
            switch (p_Model)
            {
                case Code39Model.Code39Normal:
                    _ValueText = p_Text.ToUpper();
                    break;
                default:
                    _ValueChar = p_Text.ToCharArray();
                    Array.Reverse(_ValueChar);
                    for (int i = 0; i != _ValueChar.Length; i++)
                    {
                        if ((int)_ValueChar[i] >= 97 && (int)_ValueChar[i] <= 122)
                        {
                            _ValueText += "+" + _ValueChar[i].ToString().ToUpper();

                        }
                        else
                        {
                            _ValueText += _ValueChar[i].ToString();
                        }
                    }
                    break;
            }
            _ValueChar = _ValueText.ToCharArray();
            if (p_StarChar == true) _CodeText += m_Code39["*"];
            for (int i = 0; i != _ValueChar.Length; i++)
            {
                if (p_StarChar == true && _ValueChar[i] == '*') throw new Exception("带有起始符号不能出现*");
                object _CharCode = m_Code39[_ValueChar[i].ToString()];
                if (_CharCode == null) throw new Exception("不可用的字符" + _ValueChar[i].ToString());
                _CodeText += _CharCode.ToString();
            }
            if (p_StarChar == true) _CodeText += m_Code39["*"];

            Bitmap _CodeBmp = GetImage(_CodeText);
            GetViewImage(_CodeBmp, p_Text);
            return _CodeBmp;
        }

        /// <summary>
        /// 绘制编码图形
        /// </summary>
        /// <param name="p_Text">编码</param>
        /// <returns>图形</returns>
        private Bitmap GetImage(string p_Text)
        {
            char[] _Value = p_Text.ToCharArray();

            Array.Reverse(_Value);                  // 将数组反转

            //宽 == 需要绘制的数量*放大倍数 + 两个字的宽   
            //Bitmap _CodeImage = new Bitmap(_Value.Length * ((int)m_Magnify + 1), (int)m_Height);
            Bitmap _CodeImage = new Bitmap((int)m_Height, _Value.Length * ((int)m_Magnify + 1));
            Graphics _Garphics = Graphics.FromImage(_CodeImage);
            _Garphics.FillRectangle(Brushes.White, new Rectangle(0, 0, _CodeImage.Width, _CodeImage.Height));

            int _LenEx = 0;
            for (int i = 0; i != _Value.Length; i++)
            {
                int _DrawWidth = m_Magnify + 1;
                if (_Value[i] == '1')
                {
                    //_Garphics.FillRectangle(Brushes.Black, new Rectangle(_LenEx, 0, _DrawWidth, m_Height));
                    _Garphics.FillRectangle(Brushes.Black, new Rectangle(0, _LenEx,  m_Height, _DrawWidth));
                }
                else
                {
                    //_Garphics.FillRectangle(Brushes.White, new Rectangle(_LenEx, 0, _DrawWidth, m_Height));
                    _Garphics.FillRectangle(Brushes.White, new Rectangle(0, _LenEx, m_Height, _DrawWidth));
                }
                _LenEx += _DrawWidth;
                //_LenEx += m_Height;
            }

            _Garphics.Dispose();
            return _CodeImage;
        }
        /// <summary>
        /// 绘制文字
        /// </summary>
        /// <param name="p_CodeImage">图形</param>
        /// <param name="p_Text">文字</param>
        private void GetViewImage(Bitmap p_CodeImage, string p_Text)
        {
            if (m_ViewFont == null) return;
            Graphics _Graphics = Graphics.FromImage(p_CodeImage);
            SizeF _FontSize = _Graphics.MeasureString(p_Text, m_ViewFont);
            if (_FontSize.Width > p_CodeImage.Width || _FontSize.Height > p_CodeImage.Height - 20)
            {
                _Graphics.Dispose();
                return;
            }
            int _StarHeight = p_CodeImage.Height - (int)_FontSize.Height;
            _Graphics.FillRectangle(Brushes.White, new Rectangle(0, _StarHeight, p_CodeImage.Width, (int)_FontSize.Height));
            int _StarWidth = (p_CodeImage.Width - (int)_FontSize.Width) / 2;
            _Graphics.DrawString(p_Text, m_ViewFont, Brushes.Black, _StarWidth, _StarHeight);
            _Graphics.Dispose();
        }
    }
}
