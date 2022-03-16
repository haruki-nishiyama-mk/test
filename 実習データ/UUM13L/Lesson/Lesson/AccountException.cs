using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson
{
    class AccountException:Exception 
    {// 継承
        public AccountException(string message):base(message){}
    
    }
}
