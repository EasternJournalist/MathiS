using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScienceAndMath
{
    /// <summary>
    /// 符号函数转数值函数类
    /// </summary>
    class NumericSymbol
    {
        /// <summary>
        /// 组装器委托
        /// </summary>
        /// <param name="param">目标参变量</param>
        /// <param name="paramexp">所组装的函数的参数表达式</param>
        /// <returns></returns>
        public delegate Functional.Function Assembler(ref List<string> param, ref List<string> paramexp);

        /// <summary>
        /// 常数参考表
        /// </summary>
        public Dictionary<string, Tensor> ConstValue;

        /// <summary>
        /// 标准函数组装器映射表
        /// </summary>
        public Dictionary<string, Assembler> StandardAssembler;

        /// <summary>
        /// 由符号表达式组装函数的类
        /// </summary>
        public NumericSymbol()
        {
            //初始化常量表。首先会有预设的数学常量。
            ConstValue = new Dictionary<string, Tensor>();
            ConstValue.Add("Pi", new Tensor(3.1415926535897932384626));
            ConstValue.Add("E", new Tensor(2.71828182845904523536));
            //初始化标准函数
            StandardAssembler = new Dictionary<string, Assembler>();
            StandardAssembler["Sin"] = StandardAssembler_Sin;
            StandardAssembler["Cos"] = StandardAssembler_Cos;
            StandardAssembler["Tan"] = StandardAssembler_Tan;
            StandardAssembler["Sinh"] = StandardAssembler_Sinh;
            StandardAssembler["Cosh"] = StandardAssembler_Cosh;
            StandardAssembler["Tanh"] = StandardAssembler_Tanh;
            StandardAssembler["Asin"] = StandardAssembler_Asin;
            StandardAssembler["Acos"] = StandardAssembler_Acos;
            StandardAssembler["Atan"] = StandardAssembler_Atan;
            StandardAssembler["Exp"] = StandardAssembler_Exp;
            StandardAssembler["Log"] = StandardAssembler_Log;
            StandardAssembler["Pow"] = StandardAssembler_Pow;
            StandardAssembler["Sqrt"] = StandardAssembler_Sqrt;
            StandardAssembler["Sqr"] = StandardAssembler_Sqr;
            //StandardAssembler["Average"]=
            StandardAssembler["MatrixDet"] = StandardAssembler_MatrixDet;
        }
        /// <summary>
        /// 对给定的表达式，组装一个函数
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Functional.Function AssembleFunction(string exp, ref List<string> param)
        {
            string s;
            switch (Expression.ClassOf(ref exp))
            {
                case 0://组装一个张量
                    List<string> things = new List<string>();
                    if (!Expression.SplitList(ref exp, ref things)) throw new Exception("组装" + exp + "时发生错误" + "列表元素不合法");
                    List<Functional.Function> thingsfun = new List<Functional.Function>();
                    for (int i = 0; i < things.Count; i++)//对每个分量逐个组装
                    {
                        thingsfun.Add(AssembleFunction(things[i], ref param));
                    }
                    return
                        delegate (Tensor x)
                        {
                            Tensor[] ts = new Tensor[thingsfun.Count];
                            for (int i = 0; i < ts.Length; i++)
                            {
                                ts[i] = thingsfun[i](x);
                            }
                            return Tensor.FromTensors(ts);
                        };
                case 1:
                    //直接出现的方程和不等式，不能组装成函数
                    throw new Exception("");

                case 2://当前要组装一个多项式
                    List<string> Items = new List<string>();
                    Expression.SplitPolynomial(ref exp, ref Items);
                    List<Functional.Function> ItemsFun = new List<Functional.Function>();

                    for (int i = 0; i < Items.Count; i++)//对每个项逐个组装成函数
                    {
                        s = Items[i];
                        ItemsFun.Add(AssembleFunction(Items[i], ref param));
                    }

                    //用Lambda表达式，将每个项相加，组装
                    return delegate (Tensor x)
                    {
                        Tensor ans = ItemsFun[0](x); for (int j = 1; j < ItemsFun.Count; j++) ans += ItemsFun[j](x); return ans;
                    };

                case 3://当前要组装一个项

                    int si = 0;
                    double t = 1;
                    //处理前导负号
                    while (true)
                    {
                        if (exp[si] == '-')
                            t *= -1;
                        else if (exp[si] != '+')
                            break;
                        si++;
                    }
                    string sexp = exp.Substring(si);
                    List<string> Factors = new List<string>();
                    Expression.SplitItem(ref sexp, ref Factors);
                    List<Functional.Function> FactorsFun = new List<Functional.Function>();

                    for (int i = 0; i < Factors.Count; i++)//对每个因子逐个组装成函数
                    {
                        s = Factors[i];
                        FactorsFun.Add(AssembleFunction(s, ref param));
                    }
                    //用Lambda表达式将每个因子相乘
                    return delegate (Tensor x)
                    {
                        Tensor ans = t * FactorsFun[0](x); for (int i = 1; i < FactorsFun.Count; i++) ans *= FactorsFun[i](x); return ans;
                    };

                case 4://当前要组装一个因子（一个函数，或者一个显示定义的向量、矩阵)这是最核心的部分。
                    int k = 0;
                    bool reved = false;
                    //处理前导*和/
                    while (true)
                    {
                        if (exp[k] == '/')
                            reved = !reved;
                        else if (exp[k] != '*')
                            break;
                        k++;
                    }


                    Functional.Function ret;
                    //去除前导*和/之后，可能就是立即数或变量，我们不希望在这里处理，所以再递归一次，交给case 5
                    if (k > 0)
                    {
                        ret = AssembleFunction(exp.Substring(k), ref param);
                        return reved ? delegate (Tensor x) { return MathT.Inverse(ret(x)); } : ret;
                    }


                    if (exp[0] == '{')
                    {
                        int u = 0;
                        if (!Expression.GotoMatchedParen(ref exp, 0, ref u)) throw new Exception("在" + exp + "中括号不匹配");
                        if (u != exp.Length) throw new Exception("在" + exp + "中，不符合语法");

                        return AssembleFunction(exp.Substring(1, u - 2), ref param);
                    }
                    else if (exp[0] == '(')
                    {
                        s = exp.Substring(1, exp.Length - 2);
                        int cs = Expression.ClassOf(ref s);
                        if (2 <= cs && cs <= 6)
                        {
                            ret = AssembleFunction(s, ref param);
                            return reved ? delegate (Tensor x) { return MathT.Inverse(ret(x)); } : ret;
                        }

                    }
                    else if (exp[0] == '[')
                    {

                    }
                    else
                    {
                        string fname = "";
                        List<string> paramexp = new List<string>();
                        if (Expression.SplitFunctuionFactor(ref exp, ref fname, ref paramexp))
                        {
                            if (StandardAssembler.ContainsKey(fname))
                            {
                                ret = StandardAssembler[fname](ref param, ref paramexp);
                                return reved ? delegate (Tensor x) { return MathT.Inverse(ret(x)); } : ret;
                            }
                        }

                    }

                    break;
                case 5://组装一个立即数或者变量

                    if (Expression.IsArabic(exp[0]))
                    {
                        Tensor c = new Tensor(Convert.ToDouble(exp));
                        return delegate (Tensor x)
                        {
                            return c;
                        };
                    }
                    else//一个变量（或一个常量）
                    {
                        if (ConstValue.ContainsKey(exp))//先在常量表中找
                        {
                            Tensor c = new Tensor(ConstValue[exp]);
                            return
                                delegate (Tensor x)
                                {
                                    return c;
                                };
                        }
                        else//然后看是哪个参数表中的哪个变量
                        {
                            for (int i = 0; i < param.Count; i++)
                            {
                                if (exp == param[i])
                                {
                                    return
                                        delegate (Tensor x)
                                        {
                                            return new Tensor(x.X[i]);
                                        };
                                }
                            }
                        }
                        //在变量和常量中都没有找到，未定义的变量名，不可以组装函数
                        throw new Exception("组装函数错误：出现未定义的变量");
                    }
            }
            throw new Exception("未明确的错误，在组装 " + exp + " 时发生");
        }

        public Functional.Function StandardAssembler_Sin(ref List<string> param, ref List<string> paramexp)
        {
            if (paramexp.Count == 1)
            {
                string s = paramexp[0];
                int cs = Expression.ClassOf(ref s);
                if (2 <= cs && cs <= 5)
                {
                    Functional.Function fparam = AssembleFunction(s, ref param);
                    return
                        delegate (Tensor x)
                        {
                            return MathT.Sin(fparam(x));
                        };
                }
            }
            throw new Exception("组装Sin函数失败：参数个数不合法");
        }
        public Functional.Function StandardAssembler_Cos(ref List<string> param, ref List<string> paramexp)
        {
            if (paramexp.Count == 1)
            {
                string s = paramexp[0];
                int cs = Expression.ClassOf(ref s);
                if (2 <= cs && cs <= 5)
                {
                    Functional.Function fparam = AssembleFunction(s, ref param);
                    return
                        delegate (Tensor x)
                        {
                            return MathT.Cos(fparam(x));
                        };
                }
            }
            throw new Exception("组装Cos函数失败：参数个数不合法");
        }
        public Functional.Function StandardAssembler_Tan(ref List<string> param, ref List<string> paramexp)
        {
            if (paramexp.Count == 1)
            {
                string s = paramexp[0];
                int cs = Expression.ClassOf(ref s);
                if (2 <= cs && cs <= 5)
                {
                    Functional.Function fparam = AssembleFunction(s, ref param);
                    return
                        delegate (Tensor x)
                        {
                            return MathT.Tan(fparam(x));
                        };
                }
            }
            throw new Exception("组装Tan函数失败：参数个数不合法");
        }
        public Functional.Function StandardAssembler_Tanh(ref List<string> param, ref List<string> paramexp)
        {
            if (paramexp.Count == 1)
            {
                string s = paramexp[0];
                int cs = Expression.ClassOf(ref s);
                if (2 <= cs && cs <= 5)
                {
                    Functional.Function fparam = AssembleFunction(s, ref param);
                    return
                        delegate (Tensor x)
                        {
                            return MathT.Tanh(fparam(x));
                        };
                }
            }
            throw new Exception("组装Tanh函数失败：参数个数不合法");
        }
        public Functional.Function StandardAssembler_Sinh(ref List<string> param, ref List<string> paramexp)
        {
            if (paramexp.Count == 1)
            {
                string s = paramexp[0];
                int cs = Expression.ClassOf(ref s);
                if (2 <= cs && cs <= 5)
                {
                    Functional.Function fparam = AssembleFunction(s, ref param);
                    return
                        delegate (Tensor x)
                        {
                            return MathT.Sinh(fparam(x));
                        };
                }
            }
            throw new Exception("组装Sinh函数失败：参数个数不合法");
        }
        public Functional.Function StandardAssembler_Cosh(ref List<string> param, ref List<string> paramexp)
        {
            if (paramexp.Count == 1)
            {
                string s = paramexp[0];
                int cs = Expression.ClassOf(ref s);
                if (2 <= cs && cs <= 5)
                {
                    Functional.Function fparam = AssembleFunction(s, ref param);
                    return
                        delegate (Tensor x)
                        {
                            return MathT.Cosh(fparam(x));
                        };
                }
            }
            throw new Exception("组装Cosh函数失败：参数个数不合法");
        }
        public Functional.Function StandardAssembler_Exp(ref List<string> param, ref List<string> paramexp)
        {
            if (paramexp.Count == 1)
            {
                string s = paramexp[0];
                int cs = Expression.ClassOf(ref s);
                if (2 <= cs && cs <= 5)
                {
                    Functional.Function fparam = AssembleFunction(s, ref param);
                    return
                        delegate (Tensor x)
                        {
                            return MathT.Exp(fparam(x));
                        };
                }
            }
            throw new Exception("组装Exp函数失败：参数个数不合法");
        }
        public Functional.Function StandardAssembler_Log(ref List<string> param, ref List<string> paramexp)
        {
            if (paramexp.Count == 1)
            {
                string s = paramexp[0];
                int cs = Expression.ClassOf(ref s);
                if (2 <= cs && cs <= 5)
                {
                    Functional.Function fparam = AssembleFunction(s, ref param);
                    return
                        delegate (Tensor x)
                        {

                            return MathT.Log(fparam(x));
                        };
                }
            }
            throw new Exception("组装Log函数失败：参数个数不合法");
        }
        public Functional.Function StandardAssembler_Acos(ref List<string> param, ref List<string> paramexp)
        {
            if (paramexp.Count == 1)
            {
                string s = paramexp[0];
                int cs = Expression.ClassOf(ref s);
                if (2 <= cs && cs <= 5)
                {
                    Functional.Function fparam = AssembleFunction(s, ref param);
                    return
                        delegate (Tensor x)
                        {
                            return MathT.Acos(fparam(x));
                        };
                }
            }
            throw new Exception("组装Acos函数失败：参数个数不合法");
        }
        public Functional.Function StandardAssembler_Asin(ref List<string> param, ref List<string> paramexp)
        {
            if (paramexp.Count == 1)
            {
                string s = paramexp[0];
                int cs = Expression.ClassOf(ref s);
                if (2 <= cs && cs <= 5)
                {
                    Functional.Function fparam = AssembleFunction(s, ref param);
                    return
                        delegate (Tensor x)
                        {
                            return MathT.Asin(fparam(x));
                        };
                }
            }
            throw new Exception("组装Asin函数失败：参数个数不合法");
        }
        public Functional.Function StandardAssembler_Atan(ref List<string> param, ref List<string> paramexp)
        {
            if (paramexp.Count == 1)
            {
                string s = paramexp[0];
                int cs = Expression.ClassOf(ref s);
                if (2 <= cs && cs <= 5)
                {
                    Functional.Function fparam = AssembleFunction(s, ref param);
                    return
                        delegate (Tensor x)
                        {
                            return MathT.Atan(fparam(x));
                        };
                }
            }
            throw new Exception("组装Atan函数失败：参数个数不合法");
        }
        public Functional.Function StandardAssembler_Sqrt(ref List<string> param, ref List<string> paramexp)
        {
            if (paramexp.Count == 1)
            {
                string s = paramexp[0];
                int cs = Expression.ClassOf(ref s);
                if (2 <= cs && cs <= 5)
                {
                    Functional.Function fparam = AssembleFunction(s, ref param);
                    return
                        delegate (Tensor x)
                        {
                            return MathT.Sqrt(fparam(x));
                        };
                }
            }
            throw new Exception("组装Sqrt函数失败：参数个数不合法");
        }
        public Functional.Function StandardAssembler_Sqr(ref List<string> param, ref List<string> paramexp)
        {
            if (paramexp.Count == 1)
            {
                string s = paramexp[0];
                int cs = Expression.ClassOf(ref s);
                if (2 <= cs && cs <= 5)
                {
                    Functional.Function fparam = AssembleFunction(s, ref param);
                    return
                        delegate (Tensor x)
                        {
                            Tensor ans = new Tensor(fparam(x));
                            for (int i = 0; i < ans.Count; i++)
                            {
                                ans.X[i] *= ans.X[i];
                            }
                            return ans;
                        };
                }
            }
            throw new Exception("组装Sqr函数失败：参数个数不合法");
        }
        public Functional.Function StandardAssembler_Pow(ref List<string> param, ref List<string> paramexp)
        {
            if (paramexp.Count == 2)
            {
                string s = paramexp[0];
                int cs1 = Expression.ClassOf(ref s);
                s = paramexp[1];
                int cs2 = Expression.ClassOf(ref s);

                if (cs1 >= 2 && cs1 < 6 && cs2 >= 2 && cs2 < 6)
                {
                    Functional.Function fparam1 = AssembleFunction(paramexp[0], ref param);
                    Functional.Function fparam2 = AssembleFunction(paramexp[1], ref param);
                    return
                        delegate (Tensor x)
                        {
                            return MathT.Pow(fparam1(x),fparam2(x));
                        };
                }
            }
            throw new Exception("组装Pow函数失败：参数不合法");
        }
        /*public Functional.Function StandardAssembler_Average(ref List<string> param, ref List<string> paramexp)
        {
            //if(param)
        }*/
        public Functional.Function StandardAssembler_MatrixDet(ref List<string> param, ref List<string> paramexp)
        {
            if (paramexp.Count == 1)
            {
                string s = paramexp[0];
                int cs = Expression.ClassOf(ref s);

                if (cs == 4)
                {
                    Functional.Function fparam = AssembleFunction(paramexp[0], ref param);
                    return
                        delegate (Tensor x)
                        {
                            Tensor ans = fparam(x);
                            return MathT.MatrixDet(ans);
                        };
                }
            }
            throw new Exception("组装MatrixDet函数失败：参数不合法");
        }
    }
}
