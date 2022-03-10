using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace サンプルコード1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var name = "佐藤";
            var classNo = "2";
            string teacherName;
            var scores  = new int[]{80,60,90};
            var total = 0;
            var average = 0;
            var teaches = new string[]{"田中","鈴木","山田"};
            var subjects = new string[]{"英語","国語","数学"};

            //担任の設定
            switch(classNo)
            {
                case"1":
                    teacherName = teaches[0];
                    break;
                case"2":
                    teacherName = teaches[1];
                    break;
                case"3":
                    teacherName = teaches[2];
                    break;
                default:
                    teacherName="不明";
                    break;
                    }

            //得点のチェック
            var i = 0;
            while(i<scores.Length)
            {
                if(scores[i]<0||scores[i]>100)
                {MessageBox.Show(subjects[i]+":0～100の数値を指定してください");
                break;
                }
                i++;
            }
            //総得点の計算
            foreach(var value in scores)
            {
                total = value;
            
            }

        //平均点の計算
        //総得点/科目点は少数の値になる場合があるため、int型にキャストする
        average = (int)total/scores.Length;

       //結果の出力
            MessageBox.Show("名前:"+name+"クラス:"+classNo+"組 担任:"+teacherName);
            for(var j = 0;j<scores.Length;j++)
            {MessageBox.Show(subjects[j]+":"+scores[j].ToString()+"点");
            }
            MessageBox.Show("合計:"+total.ToString()+"点　平均:"+average.ToString()+"点");
        }
    }
}
