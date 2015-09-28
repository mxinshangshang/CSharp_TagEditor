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
    public partial class IDlogin : Form
    {
        public string id = null;
        public MyEdit form = null;
        public IDlogin()
        {
            InitializeComponent();
        }

        public void GetForm(MyEdit theform)
        {
            form = theform;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form.id = textBox1.Text;
            this.Close();
        }
    }
}
