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
        double t = 1.08; //税率計算用

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

            MessageBox.Show("合計金額:" + t.ToString() + "円");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var i = 200; //単価
            var b = 5; //個数
            var c = 0.00;  //税込み金額

            c = i * b * t;

            MessageBox.Show("合計金額："+c.ToString()+"円");



        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var score = int.Parse(textBox1.Text);
            var result = "";

            if( score >= 80)
            {
                result = "合格";
            }
            else
            {
                result = "不合格";
            }

            MessageBox.Show("合否結果" + result);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            var points = new int[] { 81, 60, 90, 70, 50 };
            var total = 0;
            var average = 0;

            for(var i =0;i<points.Length;i++)
            {
           
                if(points[i]>= 0 && points[i]<=100)
            {

                     total += points[i];
            
               
            }

                      else
            {
                MessageBox.Show("無効な数値です");

                return;
            }
       average = (int)total/points.Length;

            MessageBox.Show("合計:"+total.ToString()+"点 平均:"+average.ToString());
        }}



        public int i { get; set; }
    }
}

