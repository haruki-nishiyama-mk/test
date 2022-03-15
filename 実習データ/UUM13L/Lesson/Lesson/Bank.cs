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


      

    }
}
