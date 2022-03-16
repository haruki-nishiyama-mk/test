using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson
{
    class Bank
    {
        public string Name { get; set; }    // 銀行名
        public string Address { get; set; }    // 住所

        private string ShopCode;    // ショップコード

        public string Code
        {
            get
            {
                return ShopCode;
            }
            set
            {
                if(value.Length == 3)
                {
                    ShopCode = value;
                }
                else
                {
                    ShopCode = "不明";
                }
            }
        }

        public Bank(): this("", "", "未定") { }    // デフォルトコンタクターの定義
        public Bank(string name,string shopCode): this(name,shopCode, "未定") { }
        public Bank(string name, string shopCode, string address)
        {
            this.Name = name;
            this.ShopCode = shopCode;
            this.Address = address;

        }

      public string ReturnInfo()    //銀行情報を返却するメソッド
        {
            return "銀行名：" + Name + "店舗コード：" + ShopCode + "住所：" + Address.ToString();

        }
      private Account Account { get; set; }    // 口座

        public void CreateAccount(string number,string name)
      {
          Account = new Account(number, name);
      }
        public void CreateAccount(string number,string name,int balance)
        {
            Account = new Account(number, name, balance);
        }

        public void CreateAccount(string number,string name,int balance,string tel)
        {
            Account = new Account(number, name, balance, tel);
        }
        public string ReturnAccountInfo()    // 銀行が管理している口座の口座情報を返却するメソッド
        {
            return Account.ReturnInfo();    // アカウントプロパティが保持するAccountオブジェクトのReturnInfoメソッドを実行
        }
        public bool Deposit(int maney)    //　銀行が管理している口座への預金メソッド
        {
            return Account.Deposit(maney);    // Accountプロパティが保持するAccountオブジェクトのDepositメソッドを実行
        }
    }

}
