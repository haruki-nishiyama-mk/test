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

        private void button1_Click(object sender, EventArgs e)
        {
            var xydata1 = new Xydata(8, 5);
            var xydata2 = new Xydata(2, 4);

            MessageBox.Show("(" + xydata1.X + "," + xydata1.Y + ")");
            MessageBox.Show("(" + xydata2.X + "," + xydata2.Y + ")");

            var xydata3 = xydata1 + xydata2;
            MessageBox.Show("(" + xydata3.X + "," + xydata3.Y + ")");  
        }

        class Xydata
        {
            public int X { get; private set; }
            public int Y { get; private set; }

            public Xydata(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public static Xydata operator +(Xydata obj1, Xydata obj2)
            {
                var result = new Xydata(0, 0);
                result.X = obj1.X + obj2.X;
                result.Y = obj1.Y + obj2.Y;
                return result;

            }
        }
        
    
    }
}
