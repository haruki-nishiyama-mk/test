using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson
{
  abstract  class Card
    {
        private string pinNumbeValue;    // 暗証番号

        public string Number { get; set; }    // カード番号
        public string Name { get; set; }    // カード名義

        public abstract string CardType{get;}    // カード種別

    public Card(string number,string name):this(number,name,""){}

        public Card(string number,string name,string pinNumber)
        {
            this.Number = number;
            this.Name = name;
            this.PinNumber =pinNumber;
        }

        public abstract string ReturnCardInfo();    // カード情報を返す

        public string PinNumber
        {
            get
            {
                return pinNumbeValue;
            }
            private set
            {
                if(System.Text.RegularExpressions.Regex.IsMatch(value,@"^\d{4}$"))
                {
                    pinNumbeValue = value;
                }
            }
        }
    }
}
