using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            else
                pictureBox1.SizeMode = PictureBoxSizeMode.Normal;

        }

        private void showButton_Click(object sender, EventArgs e)
        {
if(openFileDialog1.ShowDialog() == DialogResult.OK)
{ pictureBox1.Load(openFileDialog1.FileName);
}
}

        private void clearbutton_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;

        }

        private void backgroundbutton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                pictureBox1.BackColor = colorDialog1.Color;
        }

        private void closebutton_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
