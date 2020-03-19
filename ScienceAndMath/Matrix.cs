using System;
using System.Collections.Generic;

namespace ScienceAndMath
{
   
    /// <summary>
    /// 矩阵
    /// </summary>
    public struct Matrix
    {
        /// <summary>
        /// 行数
        /// </summary>
        private int _Row;
        /// <summary>
        /// 列数
        /// </summary>
        private int _Column;
        public int Row
        {
            get { return _Row; }
        }
        public int Column
        {
            get { return _Column; }
        }
         
        public double[,] X;
        /// <summary>
        /// 初始化一个矩阵
        /// </summary>
        /// <param name="M">行数</param>
        /// <param name="N">列数</param>
        public Matrix(int M, int N)
        {
            _Row = M;
            _Column = N;
            X = new double[M + 1, N + 1];
        }
        /// <summary>
        /// 初始化一个N阶单位阵
        /// </summary>
        /// <param name="N"></param>
        public Matrix(int N)
        {
            _Row = N;
            _Column = N;
            X = new double[N + 1, N + 1];
            for (int i = 1; i <= N; i++)
            {
                X[i, i] = 1;
            }
        }
       
        /// <summary>
        /// 矩阵加法a+b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Matrix operator +(Matrix a, Matrix b)
        {
            Matrix ret;
            if (a._Row == b._Row && a._Column == b._Column)
            {
                ret = new Matrix(a._Row, a._Column);
                for (int i = 1; i <= a._Row; i++)
                {
                    for (int j = 1; j <= a._Column; j++)
                    {
                        ret.X[i, j] = a.X[i, j] + b.X[i, j];
                    }
                }
            }
            else
            {
                throw new Exception("矩阵加法行列不相等");
            }
            return ret;
        }
        /// <summary>
        /// 矩阵减法a-b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Matrix operator -(Matrix a, Matrix b)
        {
            Matrix ret;
            if (a._Row == b._Row && a._Column == b._Column)
            {
                ret = new Matrix(a._Row, a._Column);
                for (int i = 1; i <= a._Row; i++)
                {
                    for (int j = 1; j <= a._Column; j++)
                    {
                        ret.X[i, j] = a.X[i, j] - b.X[i, j];
                    }
                }
            }
            else
            {
                throw new Exception("矩阵减法行列不相等");
            }
            return ret;
        }
        /// <summary>
        /// 矩阵数乘
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Matrix operator *(double a, Matrix b)
        {
            Matrix ret = new Matrix(b._Row, b._Column);
            for (int i = 1; i <= b._Row; i++)
            {
                for (int j = 1; j <= b._Column; j++)
                {
                    ret.X[i, j] = a * b.X[i, j];
                }
            }
            return ret;
           
        }
        /// <summary>
        /// 矩阵乘积
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Matrix operator *(Matrix a, Matrix b)
        {
            Matrix ret;
            if (a._Column == b._Row)
            {
                ret = new Matrix(a._Row, b._Column);
                for (int i = 1; i <= a._Row; i++)
                {
                    for (int j = 1; j <= b._Column; j++)
                    {
                        ret.X[i, j] = 0;
                        for (int k = 1; k <= a._Column; k++)
                        {
                            ret.X[i, j] += a.X[i, k] * b.X[k, j];
                        }
                    }
                }
            }
            else
            {
                throw new Exception("矩阵乘法左列数不等于右行数,左乘矩阵列数为"+a._Column.ToString()+"右乘矩阵列数为"+b._Row.ToString());
            }
            return ret;
        }
        /// <summary>
        /// 向量×矩阵(a.Dimesion==b._Row)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static VectorN operator *(VectorN a, Matrix b)
        {
            VectorN ret;
            if (a.Dimension == b._Row)
            {
                ret = new VectorN(b._Column);
                for (int j = 1; j <= b._Column; j++)
                {
                    ret.X[j] = 0;
                    for (int k = 1; k <= a.Dimension; k++)
                    {
                        ret.X[j] += a.X[k] * b.X[k, j];
                    }
                }
            }
            else
            {
                throw new Exception("矩阵乘法中向量维度与矩阵的行数不相等");
            }
            return ret;
        }
        /// <summary>
        /// 矩阵×向量(a._Column==b.Dimension，将结果转置），返回一个向量
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static VectorN operator *(Matrix a, VectorN b)
        {
            VectorN ret;
            if (a._Column == b.Dimension)
            {
                ret = new VectorN(a._Row);
                for (int i = 1; i <= a._Row; i++)
                {
                    ret.X[i] = 0;
                    for (int k = 1; k <= b.Dimension; k++)
                    {
                        ret.X[i] += a.X[i, k] * b.X[k];
                    }
                }
            }
            else
            {
                ret = new VectorN(0);
            }
            return ret;
        }
        /// <summary>
        /// 返回转置的矩阵
        /// </summary>
        /// <returns></returns>
        public Matrix Tanspose()
        {
            Matrix ret= new Matrix(_Column, _Row);
            for(int i=1;i<=_Row;i++)
                for(int j=1;j<=_Column;j++)
                {
                    ret.X[j, i] = X[i, j];
                }
            return ret;
        }
        public void SR(int i, int j)
        {
            double t;
            for(int k = 1; k <= _Column; k++)
            {
                t = X[i,k];
                X[i,k] = X[j,k];
                X[j,k] = t;
            }
        }
        public void SC(int i, int j)
        {
            double t;
            for (int k = 1; k <= _Column; k++)
            {
                t = X[i, k];
                X[i, k] = X[j, k];
                X[j, k] = t;
            }
        }
        public void TR(int i, int j, double l)
        {
            for (int k = 1; k <= _Column; k++)
            {
                X[i, k] += l * X[j, k];
            }
        }
        public void TC(int i, int j, double l)
        {
            for (int k = 1; k <= _Row; k++)
            {
                X[k, i] += l * X[k, j];
            }
        }
        public void DR(int i, double l)
        {
            for (int k = 1; k <= _Column; k++)
            {
                X[i, k] *= l;
            }
        }
        public void DC(int i, double l)
        {
            for (int k = 1; k <= _Row; k++)
            {
                X[k, i] *= l;
            }
        }
        /// <summary>
        /// 返回矩阵的行列式
        /// </summary>
        /// <returns></returns>
        ///    
        public double Determinate()
        {
            if (_Row != _Column)
            {
                throw new Exception("所求行列式不是方阵");
            }
            Matrix mat=this;
            int firstlinenot0, i, j;

            bool sign = false;//是否取相反数

            for (i = 1; i <= _Column; i++)
            {
                for (firstlinenot0 = i; firstlinenot0 <= _Column && 0 == mat.X[firstlinenot0,i]; firstlinenot0++) ;
                if (firstlinenot0 > _Column) return 0;

                if (firstlinenot0 != i)
                {
                    sign = !sign;
                    mat.SR(i, firstlinenot0);
                }
                for (j = i + 1; j <= _Column; j++)
                {
                    if (mat.X[j,i] != 0)
                    {
                        mat.TR(j, i, -mat.X[j,i] / mat.X[i,i]);
                    }
                }
            }
            double ans = mat.X[1,1];
            for (i = 2; i <= _Column; i++)
            {
                ans *= mat.X[i,i];
            }

            if (sign) ans = -ans;
            return ans;
        }
        public Matrix Inverse()
        {
            if (_Row != _Column)
            {
                throw new Exception("求逆的矩阵不是方阵");
            }
            Matrix ret = new Matrix(_Column);
            Matrix mat = this;
            int firstlinenot0, i, j;

            for (i = 1; i <= _Column; i++)
            {
                for (firstlinenot0 = i; firstlinenot0 <= _Column && 0 == mat.X[firstlinenot0, i]; firstlinenot0++) ;
                if (firstlinenot0 > _Column)
                {
                    throw new Exception("求逆的矩阵行列式为0");
                }
                if (firstlinenot0 != i)
                {
                    mat.SR(i, firstlinenot0);
                    ret.SR(i, firstlinenot0);
                }
                for (j = i + 1; j <= _Column; j++)
                {
                    if (mat.X[j, i] != 0)
                    {
                        mat.TR(j, i, -mat.X[j, i] / mat.X[i, i]);
                        ret.TR(j, i, -mat.X[j, i] / mat.X[i, i]);
                    }
                }
            }
            for (i = _Column; i >= 1; i--)
            {
                ret.DR(i, 1 / mat.X[i, i]);
                for (j = i - 1; j >= 1; j--)
                {
                    if (mat.X[j,i] != 0)
                    {
                        ret.TR(j, i, -mat.X[j, i]);
                    }
                }
            }
            return ret;
        }   
        /// <summary>
        /// 返回矩阵的迹
        /// </summary>
        /// <returns></returns>
        public double Trace()
        {
            double ans = 0;
            
            for(int i=1;i<=_Row;i++)
            {
                ans += X[i, i];
            }
            return ans;
        }
        /// <summary>
        /// 返回 从行L1列R1 到 行L2列R2 的子矩阵
        /// </summary>
        /// <param name="L1"></param>
        /// <param name="R1"></param>
        /// <param name="L2"></param>
        /// <param name="R2"></param>
        /// <returns></returns>
        public Matrix SubMatrix(int L1,int R1,int L2,int R2)
        {

            if(L1>L2||R1>R2)
            {
                throw new Exception("子矩阵下标不合法");
            }
            else
            {
                Matrix ret = new Matrix(L2 - L1 + 1, R2 - R1 + 1);
                for(int i = L1; i <= L2; i++)
                {
                    for(int j = R1; j <= R2; j++)
                    {
                        ret.X[i - L1 + 1, j - R1 + 1] = X[i, j];
                    }
                }
                return ret;
            }    
        }
        /// <summary>
        /// 返回矩阵的字符串形式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string ret = "MAT{{";
            for (int i = 1; i < _Row; i++)
            {
                for (int j = 1; j < _Column; j++)
                {
                    ret += X[i, j].ToString() + ",";
                }
                ret += X[i, _Column].ToString() + "},{";
            }
            for (int j = 1; j < _Column; j++)
            {
                ret += X[_Row, j].ToString() + ",";
            }
            ret += X[_Row, _Column].ToString()+"}}";
            return ret;
        }
    }
}
