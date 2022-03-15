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
    
        public string ReturnInfo()    // 口座情報を返却するメソッド
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
    }



}
