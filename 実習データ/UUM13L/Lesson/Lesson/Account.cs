using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson
{
    class Account
    {
        public string Name { get; set; }    // 口座名
        public string Number { get; set; }    // 口座番号
        public int Balance { get; set; }    // 残高

        private string telValue;    // 電話番号

        public string Tel　　 // 電話番号プロパティの定義（設定時に入力チェック
        {
            get
            {
                return telValue;
            }
            set
            {
                if (value.Length <= 11)
                {
                    telValue = value;
                }
                else
                {
                    telValue = "不明";
                }
            }

        }
    
        public virtual string ReturnInfo()    // 口座情報を返却するメソッド
        {
            return "口座番号:" + Number + "口座名義:" + Name + "残高:" + Balance.ToString();
        }

        public bool Deposit(int Money)    // 預金と成否を返却するメソッド
        {
        if(Money > 0)
        {
            Balance += Money;
            return true;
        }
        else
        {
            return false;
        }
        }
        public void ChangeInfo(string number, string name)    // 口座情報を変更するメソッド
        {
            this.Number = number;
            this.Name = name; 
        }

        public void ChangeInfo(string number,string name,string tel)
        {
            this.Number = number;
            this.Name = name;
            this.Tel = tel;
        }
      public Account():this("","",0,""){}    // デフォルトコンストラクターの定義

      public Account(string number, string name) : this(number, name, 0, "") { }

      public Account(string number, string name,int balance) : this(number, name, balance, "") { }

      public Account(string number, string name,int balance,string tel)    //引数4つのコンストラクターの定義
      {
          this.Number = number;
          this.Name = name;
          this.Balance = balance;
          this.Tel = tel;

      }
      private static double rateVlaue = 0.02;    // 預金金利

        public static double Rate
      {
            get
          {
              return rateVlaue;
          }
            set
          {
              rateVlaue = value;
          }
      }

        public static string ReturnRate()
        {
            return "預金金利:" + Rate.ToString();
        }
        public bool WithDraw(int money)    // 預金を減算するメソッド
        {
            if(money <= Balance)
            {
                Balance -= money;
                return true;
            }
            else
            {
                return false;
            }
        }

        public string Confirm(string nameToAuth)
        {
            if (Name == nameToAuth)
            {
                return "ようこそ" + Name + "さん！ご利用のお手続きをお選びください";
            }
            else
            {
                return "お名前が見つかりません。　内容をお確かめください";
            }
        }
    }



}
