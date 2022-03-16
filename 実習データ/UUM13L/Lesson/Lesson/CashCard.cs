using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson
{
    class CashCard:Card
    {
        public bool Substitute { get; set; }    // 代理人カードかどうかを示す

        public CashCard(string number,string name,string pinNumber):base(number,name,pinNumber)
        {
            Substitute = false;
        }

        public override string ReturnCardInfo()
        {
            if(Substitute)
            {
                return CardType + "名義" + Name + "(代理人カード)";
            }
            else
            {
                return CardType + "名義: " + Name;
            }
        }
        public override string CardType
        {
            get 
            {
                return "キャッシュカード";
            }
        }
    }
}
