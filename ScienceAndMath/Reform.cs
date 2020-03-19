using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScieceAndMathTest
{
    class Reform
    {
        static public  string ToStringSimplized(double x)
        { 
            List<int> id=new List<int>();
            string ret;
            int a, b, c, d;
            double y,t,f;
            if(x<0)
            {
                x = -1;
                ret = "-";
            }
            else
            {
                ret = "";
            }
            y = x;
            while(true)
            {
                f = Math.Floor(y);
                id.Add((int)f);
                if (id.Count > 20) return x.ToString();
                t = y - f;
                if(t<0.00000001)
                {
                    break;
                }
                y = 1 / t;
            }
            a = id[id.Count-1];
            b = 1;
            for(int i=id.Count-2;i>=0;i--)
            {
                c = a;
                d = b;
                b = c;
                a = d + id[i] * c;  
            }
            ret = a.ToString() + ( b == 1 ? "" :"/"+ b.ToString());
            return ret;
        }
    }
}
