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
        double t = 1.08;    // 税率計算用

        List<string> itemList = new List<string>();    // 2-7　商品名格納用

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var i = 200;    // 単価
            var b = 5;    // 個数
            var t = 0;

            t = i * b;

            MessageBox.Show("合計金額:" + t.ToString() + "円");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var i = 200;    // 単価
            var b = 5;    // 個数
            var c = 0.00;    // 税込み金額

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

        private void button5_Click(object sender, EventArgs e)
        {
            var a = 0; // 乳児 0～3
            var b = 600; // 小人 4～12
            var c = 1200; // 大人 13以上

            var age = int.Parse(textBox1.Text);
            var price = 0;

           if(age >= 0 && age<=3)
           {
               price = a;
           }
           else if(age >=4 && age <=12)
           {
               price = b;
           }
           else
           {
               price = c;
           }

           MessageBox.Show("入場料:" + price.ToString()+"円");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var score = new int[] { 80, 60, 60, 10, 50 }; // 科目点数
            var total = 0; // 合計点数格納
            var succese =0; // 合格科目数格納

            foreach(var value in score) // 反復変数
            {
                if(value>= 0 && value <=100)
                {
                 if(value >= 60 )
                 {
                     succese++;
                 }

                }
                else
                {
                    MessageBox.Show("無効な数値です");
                    return;
                }
            }

           if(succese >= 3)
           {
               MessageBox.Show("合格");
           }

           else
           {
               MessageBox.Show("不合格");
           }
          
        }

        private void button7_Click(object sender, EventArgs e)
        {
  
            itemList.Add(textBox1.Text); // 商品名の入力追加
            
            foreach(var value in itemList)
                {
                    MessageBox.Show(value.ToString());
                }


            }


//ここからLesson 4



        private void button8_Click(object sender, EventArgs e)
        {
            var suzukiAccount = new Account();    // アカウントのインスタンス生成

            suzukiAccount.Name = "鈴木ハナコ";    // 名前
            suzukiAccount.Tel = "08012345678";    // 電話番号

            MessageBox.Show("口座名は" + suzukiAccount.Name + "です");
            MessageBox.Show("電話番号は" + suzukiAccount.Tel + "です");


        }

        private void button9_Click(object sender, EventArgs e)
        {
            var nadeshikoBank = new Bank();

            nadeshikoBank.Name = "なでしこ銀行";
            nadeshikoBank.Address = "東京都港区湊南";

            nadeshikoBank.Code = "555";

            MessageBox.Show("この銀行は" + nadeshikoBank.Name + "です");
            MessageBox.Show("住所は" + nadeshikoBank.Address + "です");
            MessageBox.Show("店舗コードは" + nadeshikoBank.Code + "です");
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var suzukiAccount = new Account();    // オブジェクト変数の定義とインスタンス生成及び格納
            suzukiAccount.ChangeInfo("12345", "鈴木ハナコ");    // 口座情報を変更

            MessageBox.Show(suzukiAccount.ReturnInfo());    // 口座番号を表示

            var reslut = suzukiAccount.Deposit(int.Parse(textBox1.Text));    // 預金結果
            if(reslut)
            {
                MessageBox.Show("預金成功");
            }
            else
            {
                MessageBox.Show("預金失敗");
            }
            MessageBox.Show(suzukiAccount.ReturnInfo());
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var suzukiAcconut = new Account();    // オブジェクト変数の定義とインスタンス生成及び格納
            suzukiAcconut.Number = "12345";
            suzukiAcconut.Name = "鈴木ハナコ";
            suzukiAcconut.Balance = 0;

            MessageBox.Show(suzukiAcconut.ReturnInfo());    // 口座情報を表示

            MessageBox.Show("電話番号:"+suzukiAcconut.Tel);    // 電話番号を表示

            suzukiAcconut.ChangeInfo("98765", "佐藤ハナコ","09012345678");    // 口座情報を変更

            MessageBox.Show(suzukiAcconut.ReturnInfo());
            MessageBox.Show("電話番号:" + suzukiAcconut.Tel);    


        }
        }

    }


