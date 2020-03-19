using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScienceAndMath
{
    
    /// <summary>
    /// 泛函类
    /// </summary>
    class Functional
    {
        public delegate double Func11(double x);
        public delegate double FuncN1(VectorN x);
        public delegate VectorN Func1N(double x);
        public delegate VectorN FuncNN(VectorN x);

        /// <summary>
        /// 张量支持下的通用函数
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public delegate Tensor Function(Tensor x);
        /// <summary>
        /// 自动求导函数的值
        /// </summary>
        /// <param name="f">一元数量函数</param>
        /// <param name="x"></param>
        /// <returns></returns>
        static public double DerivativeAuto(Func11 f, double x)
        {
            
            double h = 0.01, d, dd, u;
            if (f(x) == double.NaN) return double.NaN;//是否在定义域内，若不在定义域内，则返回NaN
            d = (f(x + h) - f(x)) / h;
            while (h > 0.000000001)
            {
                h /= 2;
                dd = (f(x + h) - f(x)) / h;
                u = dd - d;
                if (u < 0) u = -u;
                if (u < 0.00000001) return dd;
                d = dd;
            }
            return double.NaN;
        }
        /// <summary>
        /// 求f的导函数
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        static public Func11 DerivativeAuto(Func11 f)
        {
            return delegate (double x) { return DerivativeAuto(f, x); };
        }
        /// <summary>
        /// 指定步长的求导，不会判断导函数是否存在
        /// </summary>
        /// <param name="f">一元数量函数f</param>
        /// <param name="x">求x处的导</param>
        /// <param name="Step">求导步长</param>
        /// <returns></returns>
        static public double DerivativeByStep(Func11 f, double x, double Step)
        {
            return (f(x + Step) - f(x)) / Step;
        }
        /// <summary>
        /// 指定精度的求导
        /// </summary>
        /// <param name="f"></param>
        /// <param name="x"></param>
        /// <param name="Precision"></param>
        /// <returns></returns>
        static public double DerivativeByPrecision(Func11 f,double x,double Precision)
        {
            double h = 0.125, d, dd, u;
            if (f(x) == double.NaN) return double.NaN;//是否在定义域内，若不在定义域内，则返回NaN
            d = (f(x + h) - f(x)) / h;
            while (h > 0.000000000001)
            {
                h /= 2;
                dd = (f(x + h) - f(x)) / h;
                u = dd - d;
                if (u < 0) u = -u;
                if (u < Precision) return dd;
                d = dd;
            }
            return double.NaN;
        }
    }
    class Numeric
    {
        
        
    }
}
