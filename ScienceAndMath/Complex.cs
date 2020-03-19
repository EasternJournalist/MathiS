using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScienceAndMath
{
    /// <summary>
    /// 复数类型
    /// </summary>
    public struct Complex
    {
        /// <summary>
        /// 实部
        /// </summary>
        public double Re;
        /// <summary>
        /// 虚部
        /// </summary>
        public double Im;
        /// <summary>
        /// 初始化一个复数
        /// </summary>
        /// <param name="re">实部</param>
        /// <param name="im">虚部</param>
        public Complex(double re, double im)
        {
            Re = re;
            Im = im;
        }
        /// <summary>
        /// 返回共轭
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Complex operator !(Complex a)//
        {
            return new Complex(a.Re, -a.Im);
        }
        /// <summary>
        /// 模长
        /// </summary>
        /// <returns></returns>
        public double Modulus
        {
            get { return Math.Sqrt(Re * Re + Im * Im); }
            set
            {
                double M = value / Math.Sqrt(Re * Re + Im * Im);
                Re *= M;
                Im *= M;
            }
        }
        /// <summary>
        /// 返回模长的平方
        /// </summary>
        /// <returns></returns>
        public double ModulusSquare()
        {
            return Re * Re + Im * Im;
        }
        /// <summary>
        /// 幅角
        /// </summary>
        /// <returns></returns>
        public double Arg//幅角
        {
            get
            {
                if (Im > 0)
                    return Math.Acos(Re / (Modulus * 1.000000000001));
                else
                    return -Math.Acos(Re / (Modulus * 1.000000000001));
            }
            set
            {
                double M = Modulus;
                Re = M * Math.Cos(value);
                Im = M * Math.Sin(value);
            }
        }
        /*
         * 复数的四则运算
         */
        //a+b
        public static Complex operator +(Complex a, Complex b)
        {
            return new Complex(a.Re + b.Re, a.Im + b.Im);
        }
        public static Complex operator +(double a, Complex b)
        {
            return new Complex(a + b.Re, b.Im);
        }
        public static Complex operator +(Complex a, double b)
        {
            return new Complex(a.Re + b, a.Im);
        }
        //a-b
        public static Complex operator -(Complex a, Complex b)
        {
            return new Complex(a.Re - b.Re, a.Im - b.Im);
        }
        public static Complex operator -(double a, Complex b)
        {
            return new Complex(a - b.Re, b.Im);
        }
        public static Complex operator -(Complex a, double b)
        {
            return new Complex(a.Re - b, a.Im);
        }
        public static Complex operator -(Complex a)
        {
            return new Complex(-a.Re, -a.Im);
        }
        //a*b
        public static Complex operator *(Complex a, Complex b)
        {
            return new Complex(a.Re * b.Re - a.Im * b.Im, a.Re * b.Im + a.Im * b.Re);
        }
        public static Complex operator *(double a, Complex b)
        {
            return new Complex(a * b.Re, a * b.Im);
        }
        public static Complex operator *(Complex a, double b)
        {
            return new Complex(a.Re * b, a.Im * b);
        }
        //a/b
        public static Complex operator /(Complex a, Complex b)
        {
            double m = b.ModulusSquare();
            return new Complex((a.Re * b.Re + a.Im * b.Im) / m, (a.Im * b.Re - a.Re * b.Im) / m);
        }
        public static Complex operator /(double a, Complex b)
        {
            double m = b.ModulusSquare();
            return new Complex(a * b.Re / m, -a * b.Im / m);
        }
        public static Complex operator /(Complex a, double b)
        {
            return new Complex(a.Re / b, a.Im / b);
        }
        /// <summary>
        /// 将复数类型强制转换为平面向量类型
        /// </summary>
        /// <param name="a"></param>
        public static implicit operator Vector2(Complex a)//
        {
            return new Vector2(a.Re, a.Im);
        }
        /// <summary>
        /// 将一个实数类型强制转换为一个仅有实部的复数类型(事实上一般是不必要的，因为所有复数四则运算都重载了以实数为参数之一)
        /// </summary>
        public static implicit operator Complex(double a)
        {
            return new Complex(a, 0);
        }
        /// <summary>
        /// 返回复数的字符串形式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Im >= 0)
            {
                return Re.ToString() + "+" + Im.ToString() + "i";
            }
            else
            {
                return Re.ToString() + Im.ToString() + "i";
            }

        }
    }

    /// <summary>
    /// 复数域的数学函数类
    /// </summary>
    public class MathX
    {
        /// <summary>
        /// 指数函数，返回以e为底的指定复数次幂
        /// </summary>
        /// <param name="a">一个复数</param>
        /// <returns></returns>
        public static Complex Exp(Complex a)
        {
            return Math.Exp(a.Re) * new Complex(Math.Cos(a.Im), Math.Sin(a.Im));
        }
        /// <summary>
        /// 余弦函数，返回指定复数的余弦值
        /// </summary>
        /// <param name="a">一个复数</param>
        /// <returns></returns>
        public static Complex Cos(Complex a)
        {
            return new Complex(Math.Cos(a.Re) * Math.Cosh(a.Im), Math.Sin(a.Re) * Math.Sinh(a.Im));

        }
        /// <summary>
        /// 正弦函数，返回指定复数的正弦值
        /// </summary>
        /// <param name="a">一个复数</param>
        /// <returns></returns>
        public static Complex Sin(Complex a)
        {
            return new Complex(Math.Sin(a.Re) * Math.Cosh(a.Im), Math.Cos(a.Re) * Math.Sinh(a.Im));
        }
        /// <summary>
        /// 正切函数，返回指定复数的正切值
        /// </summary>
        /// <param name="a">一个复数</param>
        /// <returns></returns>
        public static Complex Tan(Complex a)
        {
            return Sin(a) / Cos(a);
        }
        /// <summary>
        /// 余切函数，返回指定复数的余切值
        /// </summary>
        /// <param name="a">一个复数</param>
        /// <returns></returns>
        public static Complex Cot(Complex a)
        {
            return Cos(a) / Sin(a);
        }
        /// <summary>
        /// 正割函数，返回指定复数的正割值
        /// </summary>
        /// <param name="a">一个复数</param>
        /// <returns></returns>
        public static Complex Sec(Complex a)
        {
            return 1 / Cos(a);
        }
        /// <summary>
        /// 余割函数，返回指定复数的余割值
        /// </summary>
        /// <param name="a">一个复数</param>
        /// <returns></returns>
        public static Complex Csc(Complex a)
        {
            return 1 / Csc(a);
        }
        /// <summary>
        /// 对数函数，返回以e为底的指定复数的对数
        /// </summary>
        /// <param name="a">一个复数</param>
        /// <returns></returns>
        public static Complex Log(Complex a)
        {
            double m = a.Modulus;
            if (m > 0)
            {
                return new Complex(Math.Log(m), a.Arg);
            }
            else
            {
                return new Complex(0, a.Arg);
            }
        }
        /// <summary>
        /// 返回指定复数的平方根
        /// </summary>
        /// <param name="a">一个复数</param>
        /// <returns></returns>
        public static Complex Sqrt(Complex a)
        {
            double m = a.Modulus;
            return new Complex(Math.Sqrt((m + a.Re) / 2), a.Im > 0 ? Math.Sqrt((m - a.Re) / 2) : -Math.Sqrt((m - a.Re) / 2));
        }
        /// <summary>
        /// 指数函数，返回指定复数为底，指定复数次幂
        /// </summary>
        /// <param name="a">指数</param>
        /// <param name="b">底数</param>
        /// <returns></returns>
        public static Complex Pow(Complex a, Complex b)
        {
            return Exp(b * Log(a));
        }
        /// <summary>
        /// 返回指定底的对数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Complex Log(Complex a, Complex b)
        {
            return Log(a) / Log(b);
        }
        /// <summary>
        /// 反正弦函数
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Complex Asin(Complex a)
        {
            Complex i = new Complex(0, 1);
            return -i * Log(Sqrt(1 - a * a) + i * a);
        }
        /// <summary>
        /// 反余弦函数
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Complex Acos(Complex a)
        {
            Complex i = new Complex(0, 1);
            return -i * Log(a + i * Sqrt(1 - a * a));
        }
        /// <summary>
        /// 反正切函数
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Complex Atan(Complex a)
        {
            Complex i = new Complex(0, 1);
            return 0.5 * i * Log((i + a) / (i - a));
        }
    }
}
