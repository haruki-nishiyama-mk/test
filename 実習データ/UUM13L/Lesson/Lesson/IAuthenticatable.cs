using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson
{
    interface IAuthenticatable    // 認証インターフェイス
    {
        string Confirm(string nameToAuth);    // 文字列を使用して認証を行うメソッドのシグニチャ
    }
}
