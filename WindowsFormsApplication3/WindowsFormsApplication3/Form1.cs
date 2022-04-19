using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
class Program {
    static int W = 10; // ヨコ
    static int H = 10; // タテ
    static void Main(string[] args) {
        var f = new int[H, W]; // フィールド
        var pf = new int[H, W]; // 前のフィールド
        string[] sc = null; // コマンド
        while ((sc = Console.ReadLine().Split(' '))[0] != "q")
            if (sc.Length == 2) pf[int.Parse(sc[1]), int.Parse(sc[0])] = 1;
            else if (sc[0] == "") {
                for (var y = 0; y < H; y++, Console.WriteLine())
                    for (var x = 0; x < W; x++) {
                        var c = 0;
                        for (var yy = y - 1; yy <= y + 1; yy++)
                            for (var xx = x - 1; xx <= x + 1; xx++) {
                                if (yy < 0 || yy >= H || xx < 0 || xx >= W || (x == xx && y == yy)) continue;
                                c += pf[yy, xx];
                            }
                        f[y, x] = ((c == 2 && pf[y, x] == 1) || c == 3) ? 1 : 0;
                        Console.Write(f[y, x] == 1 ? " o" : " .");
                    }
                pf = (int[,])f.Clone();
            }
    }
}
        }
    }

