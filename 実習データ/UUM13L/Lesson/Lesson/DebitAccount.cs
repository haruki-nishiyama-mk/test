using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson
{
    class DebitAccount:Account
    {
        public DebitAccount(string number, string name, int balance) : base(number, name, balance) { }

        public bool Debit(string number,int money)
        {
            if(WithDraw(money))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
