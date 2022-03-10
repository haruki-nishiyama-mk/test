using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var numList = new List<int>();
            for( var i= 1; i<=100;i++)
            {
                numList.Add(i);
            }
            //11で割り切れる数値のみ変換ｑに格納
            var q = numList.Where(n => n % 50 == 0).Select(n=>n);

            foreach(var value in q)
            { MessageBox.Show(value.ToString()); }
            }
    }
}
