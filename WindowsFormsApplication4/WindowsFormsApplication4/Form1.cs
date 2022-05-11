using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           

        }

 

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
         if(e.KeyCode == Keys.Enter)
         {
             MessageBox.Show("「" + e.KeyCode + "」が入力されました。");
         };
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                MessageBox.Show("「" +e.KeyCode + "」が入力されました。");
            };


        }
    }
}
