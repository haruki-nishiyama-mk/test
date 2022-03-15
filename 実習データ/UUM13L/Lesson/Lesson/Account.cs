using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson
{
    class Account
    {
        public string Name { get; set; } //口座名
        public string Number { get; set; } //口座番号
        public int Balance { get; set; } //残高

        private string telValue; //電話番号

        public string Tel　　 //電話番号プロパティの定義（設定時に入力チェック
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
    }
}
