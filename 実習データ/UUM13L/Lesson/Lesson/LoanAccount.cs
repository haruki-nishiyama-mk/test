using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson
{
    class LoanAccount : Account    // 口座クラスの継承
    {
        public int LimitOverDraft { get; private set; }    // 借入限度額
        public int OverDraft { get; private set; }    // 借入額


        public LoanAccount(string number, string name) : this(number, name, 0, 10000) { }
        public LoanAccount(string number, string name, int balance, int limitOverDraft)
            : base(number, name, balance)    // 基本クラスのコンストラクター呼び出し
        {
            this.LimitOverDraft = limitOverDraft;
            OverDraft = 0;
        }

        public bool Loan(int money)
        {
            if (money <= LimitOverDraft)
            {
                OverDraft += money;    // 借入額を加算
                LimitOverDraft -= money;    // 借入限度額の減算

                return true;
            }
            else
            {
                return false;
            }
        }
        public override string ReturnInfo()
        {
            return base.ReturnInfo() + "借入限度額: " + LimitOverDraft.ToString() + "借入額; " + OverDraft.ToString();    // 口座情報を返却するメソッド、
                                                                                                                          // 基本クラスのメソッドをオバーライドする
        }
     

    }
}

