using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lesson
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var i = 200; //単価
            var b = 5; //個数
            var t = 0;

            t = i * b;

            MessageBox.Show("合計金額:"+t.ToString()+"円");

        }
    }
}
