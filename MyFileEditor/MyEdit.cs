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
using System.Runtime.InteropServices;
using OnBarcode.Barcode;

namespace TagEditor
{
    public partial class MyEdit : Form
    {

        #region 修改行间距
        public const int WM_USER = 0x0400;
        public const int EM_GETPARAFORMAT = WM_USER + 61;
        public const int EM_SETPARAFORMAT = WM_USER + 71;
        public const long MAX_TAB_STOPS = 32;
        public const uint PFM_LINESPACING = 0x00000100;
        [StructLayout(LayoutKind.Sequential)]
        private struct PARAFORMAT2
        {
            public int cbSize;
            public uint dwMask;
            public short wNumbering;
            public short wReserved;
            public int dxStartIndent;
            public int dxRightIndent;
            public int dxOffset;
            public short wAlignment;
            public short cTabCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public int[] rgxTabs;
            public int dySpaceBefore;
            public int dySpaceAfter;
            public int dyLineSpacing;
            public short sStyle;
            public byte bLineSpacingRule;
            public byte bOutlineLevel;
            public short wShadingWeight;
            public short wShadingStyle;
            public short wNumberingStart;
            public short wNumberingStyle;
            public short wNumberingTab;
            public short wBorderSpace;
            public short wBorderWidth;
            public short wBorders;
        }

        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, ref PARAFORMAT2 lParam);

        /// <summary>
        /// 设置行距
        /// </summary>
        /// <param name="ctl">控件</param>
        /// <param name="dyLineSpacing">间距</param>
        public static void SetLineSpace(Control ctl, int dyLineSpacing)
        {
            PARAFORMAT2 fmt = new PARAFORMAT2();
            fmt.cbSize = Marshal.SizeOf(fmt);
            fmt.bLineSpacingRule = 4;// bLineSpacingRule;
            fmt.dyLineSpacing = dyLineSpacing;
            fmt.dwMask = PFM_LINESPACING;
            try
            {
                SendMessage(new HandleRef(ctl, ctl.Handle), EM_SETPARAFORMAT, 0, ref fmt);
            }
            catch
            {

            }
        }
        #endregion

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
            //for (int j = Prm1.Length-1; j >=0; j--)
            //{
            //    if (r.Text.Contains(Prm1[j]))
            //    {
            //        r.Text = r.Text.Replace(Prm1[j], Prm[j]);
            //    }
            //}
            //e.Graphics.DrawString(r.Text, r.SelectionFont, Brushes.Black, 0, 0);

            Barcode bar = new Barcode();
            e.Graphics.ScaleTransform(1, 0.5f);
            e.Graphics.DrawImage(RotateImg(bar.GetBarCode(Prm[2]), 90), 275.0F, 238.0F);

            e.Graphics.ScaleTransform(1, 2);
            Font font1 = new Font("Arial Narrow", 11);
            Font font2 = new Font("Arial Narrow", 12);
            Font font3 = new Font("Arial Narrow", 8);
            int xcurrent = 315;
            int ycurrent = 236;
            for (int j = Prm1.Length - 1; j >= 0; j--)
            {
                if (r.Text.Contains(Prm1[j]))
                {
                    switch (j)
                    {
                        case 0:
                            e.Graphics.DrawString(Prm[0], font2, Brushes.Black, (float)1 / (float)80 * xcurrent, (float)15 / (float)60 * ycurrent);
                            break;
                        case 1:
                            e.Graphics.DrawString(Prm[1], font2, Brushes.Black, (float)((float)16 / (float)80)* xcurrent, (float)((float)15 / (float)60) * ycurrent);
                            break;
                        case 2:
                            e.Graphics.DrawString(Prm[2], font3, Brushes.Black, (float)59 / (float)80 * xcurrent, (float)54.5 / (float)60 * ycurrent);
                            break;
                        case 3:
                            e.Graphics.DrawString(Prm[3], font1, Brushes.Black, (float)1 / 80 * xcurrent, (float)0 / 60 * ycurrent);
                            break;
                        case 4:
                            e.Graphics.DrawString(Prm[4], font1, Brushes.Black, (float)1 / 80 * xcurrent, (float)((float)5 / (float)60) * ycurrent);
                            break;
                        case 5:
                            e.Graphics.DrawString(Prm[5], font1, Brushes.Black, (float)1 / 80 * xcurrent, (float)10 / 60 * ycurrent);
                            break;
                        case 6:
                            e.Graphics.DrawString(Prm[6], font2, Brushes.Black, (float)1 / 80 * xcurrent, (float)21 / 60 * ycurrent);
                            break;
                        case 7:
                            e.Graphics.DrawString(Prm[7], font2, Brushes.Black, (float)12 / 80 * xcurrent, (float)21 / 60 * ycurrent);
                            break;
                        case 8:
                            e.Graphics.DrawString(Prm[8], font2, Brushes.Black, (float)28 / 80 * xcurrent, (float)27 / 60 * ycurrent);
                            break;
                        case 9:
                            e.Graphics.DrawString(Prm[9], font2, Brushes.Black, (float)1 / 80 * xcurrent, (float)31.5 / 60 * ycurrent);
                            break;
                        case 10:
                            e.Graphics.DrawString(Prm[10], font2, Brushes.Black, (float)15 / (float)80 * xcurrent, (float)31.5 / 60 * ycurrent);
                            break;
                        case 11:
                            e.Graphics.DrawString(Prm[11], font2, Brushes.Black, (float)28 / 80 * xcurrent, (float)37 / 60 * ycurrent);
                            break;
                        case 12:
                            e.Graphics.DrawString(Prm[12], font2, Brushes.Black, (float)((float)28 / (float)80) * xcurrent, (float)41.5 / 60 * ycurrent);
                            break;
                        case 13:
                            e.Graphics.DrawString(Prm[13], font2, Brushes.Black, (float)28 / 80 * xcurrent, (float)46 / 60 * ycurrent);
                            break;
                        case 14:
                            //e.Graphics.DrawString(r.Lines[14], font2, Brushes.Black, 3 / 80 * xcurrent, 0);
                            break;
                        case 15:
                            e.Graphics.DrawString(Prm[15], font2, Brushes.Black, (float)28 / 80 * xcurrent, (float)48.5 / 60 * ycurrent);
                            break;
                        case 16:
                            //e.Graphics.DrawString(r.Lines[16], font2, Brushes.Black, 3 / 80 * xcurrent, 0);
                            break;
                        case 17:
                            //e.Graphics.DrawString(r.Lines[17], font2, Brushes.Black, 3 / 80 * xcurrent, 0);
                            break;
                        default:
                            MessageBox.Show("area缺失");
                            break;
                    }
                    //r.Text = r.Text.Replace(Prm1[j], Prm[j]);
                }
            }

            for (int j = Prm1.Length - 1; j >= 0; j--)
            {
                if (r.Text.Contains(Prm1[j]))
                {
                    r.Text = r.Text.Replace(Prm1[j], Prm[j]);
                }
            }


            //Barcode bar = new Barcode();
            //e.Graphics.ScaleTransform(1, 0.5f);
            //e.Graphics.DrawImage(RotateImg(bar.GetBarCode(Prm[2]), 90), 275.0F, 238.0F);

            //Linear code39 = new Linear();
            //// Barcode data to encode
            //code39.Data = Prm[2];
            //code39.Type = BarcodeType.CODE39;
            ////code39.AddCheckSum = true;
            //code39.ShowText = false;
            //// The space between 2 characters in code 39; This a multiple of X; The default is 1.
            //code39.I = 1.0f;
            //// Wide/narrow ratio, 2.0 - 3.0 inclusive, default is 2.
            //code39.N = 2.0f;
            //// If true, display a * in the beginning and end of barcode text
            //code39.ShowStartStopInText = false;

            ///*
            //* Barcode Image Related Settings
            //*/
            //// Unit of meature for all size related setting in the library. 
            ////code39.UOM = UnitOfMeasure.PIXEL;
            //// Bar module width (X), default is 1 pixel;
            //code39.X = 1;
            //// Bar module height (Y), default is 60 pixel;
            //code39.Y = 60;

            //// Image resolution in dpi, default is 72 dpi.
            //code39.Resolution = 50;

            //code39.Format = ImageFormat.Emf;

            ////e.Graphics.TranslateTransform(100, 100);
            ////e.Graphics.TranslateTransform(0.0F, -320.0F); // 平移
            ////e.Graphics.ScaleTransform(0.65f, 1);
            ////e.Graphics.RotateTransform(90.0F);
            ////e.Graphics.ScaleTransform(1, 0.67f);
            //e.Graphics.ScaleTransform(0.5f, 1);
            ////e.Graphics.DrawImage(DrawImg39(new EnCodeString().code39(Prm[2])), 200.0F, 200.0F);
            ////e.Graphics.DrawImage(RotateImg(code39.drawBarcode(), 90), 287.0F, 190.0F);
            //e.Graphics.DrawImage(code39.drawBarcode(), 200.0F, 200.0F);
        }

        private Image DrawImg39(String Encoded_Value)
        {
            int x = 0; //左边界
            int y = 0; //上边界
            int WidLength = 2; //粗BarCode长度
            int NarrowLength = 1; //细BarCode长度
            int BarCodeHeight = 30; //BarCode高度
            int intSourceLength = 8;

            Bitmap objBitmap = new Bitmap(((WidLength * 3 + NarrowLength * 7) * (intSourceLength + 2)) + (x * 2), BarCodeHeight + (y * 2));
            //Bitmap objBitmap = new Bitmap(BarCodeHeight + (y * 2), ((WidLength * 3 + NarrowLength * 7) * (intSourceLength + 2)) + (x * 2));
            Graphics objGraphics = Graphics.FromImage(objBitmap);
            objGraphics.FillRectangle(Brushes.White, 0, 0, objBitmap.Width, objBitmap.Height);
            int intEncodeLength = Encoded_Value.Length; //编码后长度
            int intBarWidth;
            //double intBarWidth;
            for (int i = 0; i < intEncodeLength; i++) //依码Code39 BarCode
            {
                intBarWidth = Encoded_Value[i] == '1' ? (WidLength *1) : (NarrowLength * 1);
                objGraphics.FillRectangle(i % 2 == 0 ? Brushes.Black : Brushes.White, x, y, intBarWidth, BarCodeHeight);
                //objGraphics.FillRectangle(i % 2 == 0 ? Brushes.Black : Brushes.White, (float)y, (float)x, (float)(BarCodeHeight), (float)(intBarWidth));
                x += (int)intBarWidth;
            }
            return objBitmap;
            //return RotateImg(objBitmap, 90);
            //return KiRotate(objBitmap, 90, Color.Black);
        }

        public static Bitmap KiRotate(Bitmap bmp, float angle, Color bkColor)
        {
            int w = bmp.Width + 2;
            int h = bmp.Height + 2;

            PixelFormat pf;

            if (bkColor == Color.Transparent)
            {
                pf = PixelFormat.Format32bppArgb;
            }
            else
            {
                pf = bmp.PixelFormat;
            }

            Bitmap tmp = new Bitmap(w, h, pf);
            Graphics g = Graphics.FromImage(tmp);
            g.Clear(bkColor);
            g.DrawImageUnscaled(bmp, 1, 1);
            g.Dispose();

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, w, h));
            Matrix mtrx = new Matrix();
            mtrx.Rotate(angle);
            RectangleF rct = path.GetBounds(mtrx);

            Bitmap dst = new Bitmap((int)rct.Width, (int)rct.Height, pf);
            g = Graphics.FromImage(dst);
            g.Clear(bkColor);
            g.TranslateTransform(-rct.X, -rct.Y);
            g.RotateTransform(angle);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(tmp, 0, 0);
            g.Dispose();

            tmp.Dispose();

            return dst;
        }

        public Image RotateImg(Image b, int angle)
        {
            angle = angle % 360;
            //弧度转换
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);
            //原图的宽和高
            int w = b.Width;
            int h = b.Height;
            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));
            //目标位图
            Bitmap dsImage = new Bitmap(W, H);
            Graphics g = Graphics.FromImage(dsImage);
            g.InterpolationMode = InterpolationMode.Bilinear;
            g.SmoothingMode = SmoothingMode.HighQuality;
            //计算偏移量
            Point Offset = new Point((W - w) / 2, (H - h) / 2);
            //构造图像显示区域：让图像的中心与窗口的中心点一致
            Rectangle rect = new Rectangle(Offset.X, Offset.Y, w, h);
            Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(360 - angle);
            //恢复图像在水平和垂直方向的平移
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawImage(b, rect);
            //重至绘图的所有变换
            g.ResetTransform();
            g.Save();
            g.Dispose();
            //保存旋转后的图片
            b.Dispose();
            //dsImage.Save("FocusPoint.jpg", ImageFormat.Jpeg);
            return dsImage;
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
    }
}

class EnCodeString
{
    public string code39(string RawData)
    {
        EnCoder39 coder39 = new EnCoder39();
        coder39.Raw_Data = RawData;
        return coder39.Encode_Code39();
    }
}

class EnCoder39
{
    public String Raw_Data = "";
    public string Encode_Code39()
    {
        string strEncode = "010010100"; //编码初始字符
        string AlphaBet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%*"; //Code39的字母
        string[] Code39 = //Code39的各字母对应码
        {    
                /* 0 */ "000110100", 
                /* 1 */ "100100001",        
                /* 2 */ "001100001", 
                /* 3 */ "101100000",
                /* 4 */ "000110001", 
                /* 5 */ "100110000", 
                /* 6 */ "001110000", 
                /* 7 */ "000100101",
                /* 8 */ "100100100",   
                /* 9 */ "001100100",  
                /* A */ "100001001",   
                /* B */ "001001001", 
                /* C */ "101001000", 
                /* D */ "000011001", 
                /* E */ "100011000",        
                /* F */ "001011000",       
                /* G */ "000001101",       
                /* H */ "100001100",        
                /* I */ "001001100",        
                /* J */ "000011100",
                /* K */ "100000011", 
                /* L */ "001000011", 
                /* M */ "101000010",       
                /* N */ "000010011",      
                /* O */ "100010010",        
                /* P */ "001010010",       
                /* Q */ "000000111", 
                /* R */ "100000110",       
                /* S */ "001000110",        
                /* T */ "000010110",       
                /* U */ "110000001",        
                /* V */ "011000001",       
                /* W */ "111000000", 
                /* X */ "010010001",       
                /* Y */ "110010000",       
                /* Z */ "011010000",      
                /* - */ "010000101",        
                /* . */ "110000100",       
                /*' '*/ "011000100",
                /* $ */ "010101000",      
                /* / */ "010100010",       
                /* + */ "010001010",        
                /* % */ "000101010",       
                /* * */ "010010100"
            };

        Raw_Data = Raw_Data.ToUpper();
        for (int i = 0; i < Raw_Data.Length; i++)
        {
            strEncode = string.Format("{0}0{1}", strEncode, Code39[AlphaBet.IndexOf(Raw_Data[i])]);
        }
        strEncode = string.Format("{0}0010010100", strEncode); //补上结束符号
        return strEncode;
    }
}

public class Barcode
{
    //对应码表
    private Hashtable Decode;
    private Hashtable CheckCode;
    //每个字符间的间隔符
    private string SPARATOR = "0";
    //float WidthUNIT= 0.25f;//宽度单位 mm
    private int WidthCU = 3;  //粗线和宽间隙宽度
    private int WidthXI = 1;  //细线和窄间隙宽度
    private int xCoordinate = 25;//75;  //条码起始坐标
    private int LineHeight = 60;
    private int Height = 0;
    private int Width = 0;

    #region 加载对应码表
    public Barcode()
    {
        Decode = new Hashtable();
        Decode.Add("0", "000110100");
        Decode.Add("1", "100100001");
        Decode.Add("2", "001100001");
        Decode.Add("3", "101100000");
        Decode.Add("4", "000110001");
        Decode.Add("5", "100110000");
        Decode.Add("6", "001110000");
        Decode.Add("7", "000100101");
        Decode.Add("8", "100100100");
        Decode.Add("9", "001100100");
        Decode.Add("A", "100001001");
        Decode.Add("B", "001001001");
        Decode.Add("C", "101001000");
        Decode.Add("D", "000011001");
        Decode.Add("E", "100011000");
        Decode.Add("F", "001011000");
        Decode.Add("G", "000001101");
        Decode.Add("H", "100001100");
        Decode.Add("I", "001001101");
        Decode.Add("J", "000011100");
        Decode.Add("K", "100000011");
        Decode.Add("L", "001000011");
        Decode.Add("M", "101000010");
        Decode.Add("N", "000010011");
        Decode.Add("O", "100010010");
        Decode.Add("P", "001010010");
        Decode.Add("Q", "000000111");
        Decode.Add("R", "100000110");
        Decode.Add("S", "001000110");
        Decode.Add("T", "000010110");
        Decode.Add("U", "110000001");
        Decode.Add("V", "011000001");
        Decode.Add("W", "111000000");
        Decode.Add("X", "010010001");
        Decode.Add("Y", "110010000");
        Decode.Add("Z", "011010000");
        Decode.Add("-", "010000101");
        Decode.Add("%", "000101010");
        Decode.Add("$", "010101000");
        Decode.Add("*", "010010100");

        CheckCode = new Hashtable();
        CheckCode.Add("0", "0");
        CheckCode.Add("1", "1");
        CheckCode.Add("2", "2");
        CheckCode.Add("3", "3");
        CheckCode.Add("4", "4");
        CheckCode.Add("5", "5");
        CheckCode.Add("6", "6");
        CheckCode.Add("7", "7");
        CheckCode.Add("8", "8");
        CheckCode.Add("9", "9");
        CheckCode.Add("A", "10");
        CheckCode.Add("B", "11");
        CheckCode.Add("C", "12");
        CheckCode.Add("D", "13");
        CheckCode.Add("E", "14");
        CheckCode.Add("F", "15");
        CheckCode.Add("G", "16");
        CheckCode.Add("H", "17");
        CheckCode.Add("I", "18");
        CheckCode.Add("J", "19");
        CheckCode.Add("K", "20");
        CheckCode.Add("L", "21");
        CheckCode.Add("M", "22");
        CheckCode.Add("N", "23");
        CheckCode.Add("O", "24");
        CheckCode.Add("P", "25");
        CheckCode.Add("Q", "26");
        CheckCode.Add("R", "27");
        CheckCode.Add("S", "28");
        CheckCode.Add("T", "29");
        CheckCode.Add("U", "30");
        CheckCode.Add("V", "31");
        CheckCode.Add("W", "32");
        CheckCode.Add("X", "33");
        CheckCode.Add("Y", "34");
        CheckCode.Add("Z", "35");
        CheckCode.Add("-", "36");
        CheckCode.Add(".", "37");
        CheckCode.Add(",", "38");
        CheckCode.Add("$", "39");
        CheckCode.Add("/", "40");
        CheckCode.Add("+", "41");
        CheckCode.Add("%", "42");
    }
    #endregion

    #region 保存文件

    public Boolean saveFile(string Code, string Title, int UseCheck)
    {
        string code39 = Encode39(Code, UseCheck);
        if (code39 != null)
        {
            Bitmap saved = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(saved);
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, this.Width, this.Height);
            this.DrawBarCode39(code39, Title, g);
            string path = @"c:\";
            string filename = path + Code + ".jpg";
            saved.Save(filename, ImageFormat.Jpeg);
            saved.Dispose();
            return true;
        }
        return false;
    }
    #endregion

    #region 转换编码
    /***
    * Code:未经编码的字符串
    * 
    * **/
    private string Encode39(string Code, int UseCheck)
    {
        int UseStand = 1;  //检查输入待编码字符是否为标准格式（是否以*开始结束）

        //保存备份数据
        string originalCode = Code;

        //为空不进行编码
        if (null == Code || Code.Trim().Equals(""))
        {
            return null;
        }
        //检查错误字符
        Code = Code.ToUpper();  //转为大写
        Regex rule = new Regex(@"[^0-9A-Z%$\-*]");
        if (rule.IsMatch(Code))
        {
            //MessageBox.Show("编码中包含非法字符，目前仅支持字母,数字及%$-*符号!!");
            return null;
        }
        //计算检查码
        if (UseCheck == 1)
        {
            int Check = 0;
            //累计求和
            for (int i = 0; i < Code.Length; i++)
            {
                Check += int.Parse((string)CheckCode[Code.Substring(i, 1)]);
            }
            //取模
            Check = Check % 43;
            //附加检测码
            foreach (DictionaryEntry de in CheckCode)
            {
                if ((string)de.Value == Check.ToString())
                {
                    Code = Code + (string)de.Key;
                    break;
                }
            }
        }
        //标准化输入字符，增加起始标记
        if (UseStand == 1)
        {
            if (Code.Substring(0, 1) != "*")
            {
                Code = "*" + Code;
            }
            if (Code.Substring(Code.Length - 1, 1) != "*")
            {
                Code = Code + "*";
            }
        }
        //转换成39编码
        string Code39 = "";
        for (int i = 0; i < Code.Length; i++)
        {
            Code39 = Code39 + (string)Decode[Code.Substring(i, 1)] + SPARATOR;
        }

        int height = 30 + LineHeight;//定义图片高度      
        int width = xCoordinate;
        for (int i = 0; i < Code39.Length; i++)
        {
            if ("0".Equals(Code39.Substring(i, 1)))
            {
                width += WidthXI;
            }
            else
            {
                width += WidthCU;
            }
        }
        this.Width = width + xCoordinate;
        this.Height = height;

        return Code39;
    }
    #endregion

    #region 绘制图像
    private void DrawBarCode39(string Code39, string Title, Graphics g)
    {
        //int UseTitle = 0;  //条码上端显示标题
                           //int UseTTF = 1;  //使用TTF字体，方便显示中文，需要$UseTitle=1时才能生效
        //if (Title.Trim().Equals(""))
        //{
        //    UseTitle = 0;
        //}
        Pen pWhite = new Pen(Color.White, 1);
        Pen pBlack = new Pen(Color.Black, 1);
        SolidBrush brush = new SolidBrush(Color.Black);
        int position = xCoordinate;
        //显示标题
        //if (UseTitle == 1)
        //{
        //    Font TitleFont = new Font("宋体", 10, FontStyle.Bold);
        //    SizeF sf = g.MeasureString(Title, TitleFont);
        //    g.DrawString(Title, TitleFont, brush, (Width - sf.Width) / 2, Height - sf.Height);
        //}
        for (int i = 0; i < Code39.Length; i++)
        {
            //绘制条线
            if ("0".Equals(Code39.Substring(i, 1)))
            {
                for (int j = 0; j < WidthXI; j++)
                {
                    g.DrawLine(pBlack, position + j, 12, position + j, 12 + LineHeight);
                }
                position += WidthXI;
            }
            else
            {
                for (int j = 0; j < WidthCU; j++)
                {
                    g.DrawLine(pBlack, position + j, 12, position + j, 12 + LineHeight);
                }
                position += WidthCU;
            }
            i++;
            //绘制间隔线
            if ("0".Equals(Code39.Substring(i, 1)))
            {
                position += WidthXI;
            }
            else
            {
                position += WidthCU;
            }
        }
        return;
    }
    #endregion

    #region 获得生成后条码图像
    /// <summary>
    /// 获得生成后条码图像
    /// </summary>
    /// <param name="Code">条码值</param>
    /// <returns>Image对象</returns>
    public Bitmap GetBarCode(string Code)
    {
        string Code39 = Encode39(Code, 1);
        Bitmap barCode = new Bitmap(this.Width, this.Height);
        Graphics g = Graphics.FromImage(barCode);
        g.FillRectangle(new SolidBrush(Color.White), 0, 0, this.Width, this.Height);
        this.DrawBarCode39(Code39, Code, g);
        return barCode;
    }
    #endregion
}