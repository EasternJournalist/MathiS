using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScieceAndMathTest
{

    
    public class Number
    {
        /// <summary>
        /// 欧几里得算法，返回a和b的最大公约数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public long Gcd(long a, long b)
        {
            long c;
            do
            {
                c = a % b;
                a = b;
                b = c;
            } while (c > 0);
            return b;
        }
        /// <summary>
        /// 拓展欧几里得算法
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="d"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Exgcd(long a,long b,ref long  d,ref long x,ref long y)
        {
            if (b == 0)
            {
                d = a; x = 1; y = 0;
            }
            else
            {
                Exgcd(b, a % b, ref d, ref y, ref x);
                y -= x * (a / b);
            }
        }
        /// <summary>
        /// 返回a和b最小公倍数
        /// </summary>
        /// <param name="a">一个长整型数</param>
        /// <param name="b">一个长整型数</param>
        /// <returns></returns>
        public long Lcs(long a, long b)
        {
            return a * b / Gcd(a,b);
        }
    }
}
