using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace インデクサーサンプルコード
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var x = new Student();

            x.Name = "原田";
            x.ClassName = "2";
            x[0] = 70;  //インデクサーによる設定
            x[1] = 62;
            x[2] = 91;
            x.Compute();  //現在の値で平均点を算出
            MessageBox.Show("名前："+x.Name+"クラス："+x.ClassName+"組"+"担任:"+x.TeacherName);
            //インデクサーによる参照
            MessageBox.Show("英語"+x[0]+"点　国語:"+x[1]+"点　数学:"+x[2]+"点");
            MessageBox.Show("平均:"+x.Avarage+"点");

            x.ClassName = "5";
            x[0]=125;  //エラー値のため0に設定
            x[1]=-50;　//エラー値のため0に設定
            x[2]=50;
            x[3]=30;　//インデックス範囲外のため設定されず
            
            x.Compute();
            MessageBox.Show("名前："+x.Name+"クラス："+x.ClassName+"組"+"担任:"+x.ClassTeacher);
            //インデクサーによる参照
            MessageBox.Show("英語"+x[0]+"点　国語:"+x[1]+"点　数学:"+x[2]+"点");
            MessageBox.Show("平均:"+x.Avarage+"点");            
        }
    }
    class Student  //Studentクラスの定義
    {

        public string Name { get; set; }
        public int Total { get; private set; }
        public string TeacherName { get; private set; }
        public int Avarage { get; private set; }
        public string ClassTeacher {get; private set;}
        private string class_name;
        private int[] score;
        private static string[] class_names　= new string[]{"1","2","3"};
        private static string[] teacher = new string[]{"田中","鈴木","山田"};
        private static string[] subject = new string[]{"英語","国語","数学"};

        public Student()
        {
            Name ="[不明]";
            class_name ="[不明]";
            ClassTeacher ="[不明]";
            score = new int[3]{0,0,0};
        }

        public String ClassName
        {
            get
            {
                return class_name;
            }
            set
            {
                for (var i = 0; i < class_names.Length; i++)
                {
                    if (class_names[i] == value)
                    {
                        class_name = value;
                        TeacherName = teacher[i];
                        return;
                    }
                }
                class_name = "[不明]";
                TeacherName = "[不明]";
            }
        }

            public void Compute()
            {
                //総得点の計算
                Total = 0;
                foreach(var value in score)
                {
                    Total += value;
                }
                //平均点の計算
                Avarage = Total / score.Length;
            }
        public int this[int idx] //インデクサー定義
    {
      get
      {
        if(idx <0 || idx >= score.Length)　//インデックス範囲テェック
        {
            return 0;
        }
          return score[idx];
    }
            set
            {
                if(idx< 0 || idx >= score.Length)　//インデックス範囲テェック
                {
                    return;
                }
            if(value >= 0 && value <= 100)　//受け取り値範囲チェック
            {
                score[idx] = value;
            }
            else
            {
               score[idx] = 0;
            }
            }

    }
    
    }



}
