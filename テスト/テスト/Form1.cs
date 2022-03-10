using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace テスト
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
         

        }

        private void label1_Click(object sender, EventArgs e)
        {
            var total = 0;
            var scores = new int[4] { 80, 60, 70, 90 };
            foreach(var value in scores)
            { total += value;
            }
            MessageBox.Show(total.ToString());
        }
    }
}
