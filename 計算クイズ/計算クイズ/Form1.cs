using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 計算クイズ
{
    public partial class Form1 : Form
    {
        Random randomizer = new Random();
        int addend1;
        int addend2;

        int minuend;
        int subtrahend;

        int multiplicand;
        int multiplier;

        int dividend;
        int divisor;

         public void StartTheQuiz()
        {    
        addend1 = randomizer.Next(51);
        addend2 = randomizer.Next(51);

        PlusLeftLabel.Text  addend1.ToString();
        PlusRightLabel.Text  addend2.ToSting();

        sum.Value = 0;}

        minuend  randomizer.Next(1,101);
        subtrahend = randomizer.Next(1.minuend);
        minusLeftLabel.Text = minuend.TosSring();
        minusRightLAbel1.Text = subtrahend.ToString();
        difference.Value = 0;

        multiplicand = randomizer.Next(2,11);
        multiplier = randomizer.Next(2,11);
        timesLeftLabel.Text = multiplicand.ToString();
        timesRightLabel.Text = multiplier.ToString();
        product.Valur = 0;

        divisor = randomizer.Next(2,11);
        int temporarQuotient = randomizer.Next(2,11);
        dividend = divisor * temporarQuotient;
        dividedLeftLabel.Text = dividend.ToString();
        dividRightLabel.Text = divisor.ToString();
        quotient.Value = 0;

       


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void startButton_Click(object sender, EventArgs e)
        {
            StartTheQuiz();
            startButton.Enabled = false;
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
        
        }
    }
}
