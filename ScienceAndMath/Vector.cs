using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScienceAndMath
{
    /// <summary>
    /// 二维向量
    /// </summary>
    public struct Vector2
    {
        double X, Y;
        /// <summary>
        /// 初始化一个二维向量
        /// </summary>
        /// <param name="x">x分量</param>
        /// <param name="y">y分量</param>
        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }
        public static double operator *(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }
        public static Vector2 operator *(double a, Vector2 b)
        {
            return new Vector2(a * b.X, a * b.Y);
        }
        /// <summary>
        /// 返回向量的字符串形式
        /// </summary>
        /// <returns></returns>
        public override string ToString()//字符串形式
        {
            return "{" + X.ToString() + "," + Y.ToString() + "}";
        }
        /// <summary>
        /// 返回向量的模长
        /// </summary>
        /// <returns></returns>
        public double Modulus()//模长
        {
            return Math.Sqrt(X * X + Y * Y);
        }
        /// <summary>
        /// 返回向量的模长的平方
        /// </summary>
        /// <returns></returns>
        public double ModulusSquare()//平方
        {
            return Math.Sqrt(X * X + Y * Y);
        }
        /// <summary>
        /// 返回向量和X轴的夹角，∈[0,π]
        /// </summary>
        /// <returns></returns>
        public double Angle()
        {
            return Math.Acos(X / (Modulus() * 1.00000001));
        }
        /// <summary>
        /// 返回两个向量的夹角
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double AngleBetween(Vector2 a, Vector2 b)//夹角(0-π)
        {
            return Math.Acos(a * b / (a.Modulus() * b.Modulus()));
        }
        /// <summary>
        /// 返回两个向量间夹角的余弦值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double CosAngleBetween(Vector2 a, Vector2 b)
        {
            return a * b / a.Modulus() / b.Modulus();
        }
        /// <summary>
        /// 返回其单位向量
        /// </summary>
        /// <returns></returns>
        public Vector2 UnitVector()
        {
            double m = Modulus();
            if (m != 0)
            {
                return new Vector2(X / m, Y / m);
            }
            else
            {
                return new Vector2(0, 0);
            }
        }
        /// <summary>
        /// 自增
        /// </summary>
        /// <returns></returns>
        public void Increase(Vector2 a)
        {
            X += a.X;
            Y += a.Y;
        }
        /// <summary>
        /// 自减
        /// </summary>
        /// <param name="a"></param>
        public void Decrease(Vector2 a)
        {
            X -= a.X;
            Y -= a.Y;
        }
    }
    /// <summary>
    /// 三维向量
    /// </summary>
    public struct Vector3
    {
        public double X, Y, Z;
        /// <summary>
        /// 从坐标初始化一个向量
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        /// <summary>
        /// 根据经纬度初始化一个单位向量
        /// </summary>
        /// <param name="Longitude">经度</param>
        /// <param name="Latitude">纬度</param>
        public Vector3(double Longitude, double Latitude)
        {
            X = Math.Cos(Longitude) * Math.Cos(Latitude);
            Y = Math.Sin(Longitude) * Math.Cos(Latitude);
            Z = Math.Sin(Latitude);
        }
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }
        public static double operator *(Vector3 a, Vector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }
        public static Vector3 operator *(double a, Vector3 b)
        {
            return new Vector3(a * b.X, a * b.Y, a * b.Z);
        }
        /// <summary>
        /// 三维向量叉乘运算
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 CrossProduct(Vector3 a, Vector3 b)
        {
            return new Vector3(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        }
        /// <summary>
        /// 返回向量的字符串形式
        /// </summary>
        /// <returns></returns>
        public override string ToString()//字符串形式
        {
            return "(" + X.ToString() + "," + Y.ToString() + "," + Z.ToString() + ")";
        }
        /// <summary>
        /// 返回向量的模长
        /// </summary>
        /// <returns></returns>
        public double Modulus()//模长
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }
        /// <summary>
        /// 返回向量模长的平方
        /// </summary>
        /// <returns></returns>
        public double ModulusSquare()//模长平方
        {
            return X * X + Y * Y + Z * Z;
        }
        /// <summary>
        /// 返回向量与X轴正方向的夹角，∈[0,π]
        /// </summary>
        /// <returns></returns>
        public double Angle()
        {
            return Math.Acos(X / (Modulus() * 1.00000001));
        }
        /// <summary>
        /// 返回两个向量之间的夹角，∈[0,π]
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double AngleBetween(Vector3 a, Vector3 b)//夹角(0-π)
        {
            return Math.Acos(a * b / (a.Modulus() * b.Modulus() * 1.00000001));
        }

        /// <summary>
        /// 返回这个向量的单位向量
        /// </summary>
        /// <returns></returns>
        public Vector3 UnitVector()
        {
            double m = Modulus();
            if (m != 0)
            {
                return new Vector3(X / m, Y / m, Z / m);
            }
            else
            {
                return new Vector3(double.NaN, double.NaN, double.NaN);
            }
        }
        /// <summary>
        /// 返回这个向量的相反方向的单位向量
        /// </summary>
        /// <returns></returns>
        public Vector3 AntiUnitVector()
        {
            double m = -Modulus();
            if (m != 0)
            {
                return new Vector3(X / m, Y / m, Z / m);
            }
            else
            {
                return new Vector3(double.NaN, double.NaN, double.NaN);
            }
        }
        /// <summary>
        /// 自增
        /// </summary>
        /// <param name="a"></param>
        public void Increase(Vector3 a)
        {
            X += a.X;
            Y += a.Y;
            Z += a.Z;
        }
        // <summary>
        /// 自减
        /// </summary>
        /// <param name="a"></param>
        public void Decrease(Vector3 a)
        {
            X -= a.X;
            Y -= a.Y;
            Z -= a.Z;
        }
        /// <summary>
        /// 转换为数组
        /// </summary>
        /// <returns>一个长度为3的数组</returns>
        public double[] ToArray()
        {
            return new double[3] { X, Y, Z };
        }
    }
    /// <summary>
    /// N维向量
    /// </summary>
    public struct VectorN//
    {
        private int N;//维度
        /// <summary>
        /// 各维度分量
        /// </summary>
        public double[] X;
        /// <summary>
        /// 初始化一个N维向量
        /// </summary>
        /// <param name="N">维度</param>
        public VectorN(int _N)
        {
            N = _N;
            X = new double[N + 1];
        }
        public int Dimension
        {
            get { return N; }
        }
        public static VectorN operator +(VectorN a, VectorN b)
        {
            VectorN ret;
            if (a.N == b.N)//判断维度是否相同
            {//维度相同，相加合法
                ret = new VectorN(a.N);
                for (int i = 1; i <= a.N; i++)
                {
                    ret.X[i] = a.X[i] + b.X[i];
                }
            }
            else
            {//维度不同，向量相加不合法，返回一个0维向量
                ret = new VectorN(0);
            }
            return ret;
        }

        public static VectorN operator -(VectorN a, VectorN b)
        {
            VectorN ret;
            if (a.N == b.N)//判断维度是否相同
            {//维度相同，相减合法
                ret = new VectorN(a.N);
                for (int i = 1; i <= a.N; i++)
                {
                    ret.X[i] = a.X[i] - b.X[i];
                }
            }
            else
            {//维度不同，向量相减不合法，返回一个0维向量
                ret = new VectorN(0);
            }
            return ret;
        }
        public static VectorN operator *(VectorN a, VectorN b)
        {
            VectorN ret;
            if (a.N == b.N)//判断维度是否相同
            {//维度相同，点积合法
                ret = new VectorN(a.N);
                for (int i = 1; i <= a.N; i++)
                {
                    ret.X[i] = a.X[i] * b.X[i];
                }
            }
            else
            {//维度不同，向量点积不合法，返回一个0维向量
                ret = new VectorN(0);
            }
            return ret;
        }
        public static VectorN operator *(double a, VectorN b)//向量数乘
        {
            VectorN ret;
            ret = new VectorN(b.N);
            for (int i = 1; i <= b.N; i++)
            {
                ret.X[i] = a * b.X[i];
            }
            return ret;
        }
        /// <summary>
        /// 返回向量的模长
        /// </summary>
        /// <returns></returns>
        public double Modulus()//
        {
            double m = 0;
            for (int i = 1; i <= N; i++)
            {
                m += X[i] * X[i];
            }
            return Math.Sqrt(m);
        }
        /// <summary>
        /// 返回向量模长平方
        /// </summary>
        /// <returns></returns>
        public double ModulusSquare()
        {
            double m = 0;
            for (int i = 1; i <= N; i++)
            {
                m += X[i] * X[i];
            }
            return m;
        }
        /// <summary>
        /// 返回Matrix类型
        /// </summary>
        /// <returns></returns>
        public Matrix ToMatrix()
        {
            Matrix ret = new Matrix(1, N);
            for (int j = 1; j <= N; j++)
            {
                ret.X[1, j] = X[j];
            }
            return ret;
        }
        /// <summary>
        /// 返回数组类型
        /// </summary>
        /// <returns></returns>
        public double[] ToArray()
        {
            double[] ret = new double[N];
            for (int i = 0; i < N; i++)
                ret[i] = X[i+1];
            return ret;
        }
        /// <summary>
        /// 返回字符串格式
        /// </summary>
        /// <returns></returns>
        public override string ToString()//
        {
            string ret = "{";
            for (int i = 1; i < N; i++)
            {
                ret += X[i].ToString() + ",";
            }
            return ret + X[N].ToString() + "}";
        }
    }
}
