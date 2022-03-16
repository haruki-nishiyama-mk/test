using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson
{
    class CreditCard : Card
    {
        public DateTime ExpireDate { get; set; }    // 有効期限
        public string Rank { get; set; }    // カードランク

        public CreditCard(string number, string name, string rank)
            : this(number, name, rank, "") { }

        public CreditCard(string number, string name, string rank, string pinNumber)
            : base(number, name, pinNumber)
        {
            ExpireDate = DateTime.Now.AddYears(3);
            this.Rank = rank;
        }
        public override string ReturnCardInfo()
        {
            return CardType + "名義: " + Name + "有効期限" + ExpireDate.ToShortDateString() + "カードランク" + Rank;
        }

        public override string CardType
        {
            get
            {
                return "クレジットカード";
            }

        }
    }
}