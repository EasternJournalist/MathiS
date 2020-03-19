using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScienceAndMath
{
    /// <summary>
    /// 提示：要避免两个张量之间直接的赋值。否则会赋值其分量数组的引用。
    /// </summary>
    public struct Tensor
    {
        public int Dimension;
        public double[] X;
        public int[] N;
        public int Count;
        public Tensor(double x)
        {
            Dimension = 0;
            Count = 1;
            X = new double[1];
            X[0] = x;
            N = new int[0];
        }
        /// <summary>
        /// 生成一个向量
        /// </summary>
        /// <param name="VecElement"></param>
        public Tensor(params double[] VecElement)
        {
            Dimension = 1;
            N = new int[1];
            N[0] = VecElement.Length;
            Count = VecElement.Length;
            X = new double[Count];
            VecElement.CopyTo(X,0);
        }
        public Tensor(int n)
        {
            Dimension = 1;
            Count = n;
            X = new double[n];
            N = new int[1];
            N[0] = n;
        }
        /*public Tensor(params int[] n)
        {
            Dimension = n.Length;
            N = new int[Dimension];
            Count = 1;
            for(int i=0;i<Dimension;i++)
            {
                N[i] = n[i];
                Count=
            }
            for(int i=0;i<Dimension)
            X = new double[Count];
            N = new int[2];
            N[0] = n1;
            N[1] = n2;
        }*/
        public Tensor(params int[] n)
        {
            Dimension = n.Length;
            N = new int[Dimension];
            Count = 1;
            for(int i=0;i<Dimension;i++)
            {
                N[i] = n[i];
                Count *= N[i];
            }
            X = new double[Count];
        }
        /// <summary>
        /// 生成a的一个拷贝
        /// </summary>
        /// <param name="a"></param>
        public Tensor(Tensor a)//tensor中的数组，在赋值过程中，不是逐个元素赋值的，会赋值其引用。所以一定避免tensor之间直接赋值！
        {
            Dimension = a.Dimension;
            Count = a.Count;
            X = new double[Count];
            for(int i=0;i<Count;i++)
            {
                X[i] = a.X[i];
            }
            N = new int[Dimension];
            for(int i=0;i<N.Length;i++)
            {
                N[i] = a.N[i];
            }
        }
        /// <summary>
        /// 返回一个单位阵
        /// </summary>
        /// <param name="n">阶数</param>
        /// <returns></returns>
        public static Tensor IdentityMatrix(int n)
        {
            Tensor ret = new Tensor(n, n);
            for (int i = 0; i < n; i++) 
            {
                ret.At(i, i) = 1;
            }
            return ret;
        }
        /// <summary>
        /// 将多个形状相同的张量组成一个更高阶的张量
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static Tensor FromTensors(params Tensor[] ts)
        {
            if (ts.Length == 0)
                throw new Exception("张量错误，空的张量列表");
            if (ts[0].Dimension==0)//生成一个一阶张量
            {
                Tensor c = new Tensor(ts.Length);
                for (int i = 0; i < ts.Length; i++)
                {
                    if (ts[i].Dimension != 0) throw new Exception("张量错误，向量的各分量不都是0阶张量");
                    c.X[i] = ts[i].X[0];
                }
                return c;
            }
            else//生成高阶张量
            {
                Tensor c = new Tensor(1);
                c.Dimension = 1 + ts[0].Dimension;
                c.N = new int[c.Dimension];
                c.N[0] = ts.Length;
                for(int i=1;i<c.Dimension;i++)
                {
                    c.N[i] = ts[0].N[i - 1];
                }
                c.X = new double[c.N[0] * ts[0].Count];
                for(int i=0;i<ts.Length;i++)
                {
                    if(CompareShape(ref ts[0],ref ts[i]))
                    {
                        for(int j=0;j<ts[0].Count;j++)
                        {
                            c.X[i * ts[0].Count + j] = ts[i].X[j];
                        }
                    }
                    else
                    {
                        throw new Exception("张量错误，生成高阶张量的低阶张量形状不同");
                    }
                }
                c.Count = ts[0].Count * ts.Length;
                return c;
            }


        }
        /// <summary>
        /// 比较张量直接形状是否相同
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool CompareShape(ref Tensor a,ref Tensor b)
        {
            if (a.Dimension != b.Dimension) return false;
            for(int i=0;i<a.Dimension;i++)
            {
                if (a.N[i] != b.N[i]) return false;
            }
            return true;
        }
        /// <summary>
        /// 创建一个新的张量与所给张量形状相同
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Tensor FromShape(Tensor a)
        {
            return new Tensor(a.N);
        }
        /// <summary>
        /// 创建所给张量的一个拷贝
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public Tensor Copy(Tensor a)
        {
            return new Tensor(a);
        }
        /// <summary>
        /// 返回一阶张量的第i个元素的引用，或将张量看成一阶，返回其索引位置第i个的元素的引用
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public ref double At(int i)
        {
            return ref X[i];
        }
        /// <summary>
        /// 访问二阶张量（矩阵）的第i行，第j列的位置
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public ref double At(int i,int j)
        {
            return ref X[i * N[1] + j];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a">位置</param>
        /// <returns></returns>
        public ref double At(params int[] a)
        {
            int ind = 0, step = 1;
            for(int i=a.Length-1;i>0;i--)
            {
                ind += step * a[i];
                step *= N[i - 1];
            }
            return ref X[ind];
        }
        /// <summary>
        /// 张量加法
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Tensor operator +(Tensor a,Tensor b)
        {
            if(a.Dimension==0&&b.Dimension==0)
            {
                return new Tensor(a.X[0] + b.X[0]);
            }
            if(a.Dimension==b.Dimension)
            {
                for (int i = 0; i < a.Dimension; i++)
                    if (a.N[i] != b.N[i]) throw new Exception("张量加法错误，相加的张量形状不同");
                Tensor c = FromShape(a);
                for (int i=0;i<a.Count;i++)
                {
                    c.X[i] =a.X[i]+ b.X[i];
                }
                return c;
            }
            else
            {
                throw new Exception("张量加法错误，相加的张量形状不同");
            }
        }
        public static Tensor operator -(Tensor a,Tensor b)
        {
            if (a.Dimension == 0 && b.Dimension == 0)
            {
                return new Tensor(a.X[0] + b.X[0]);
            }
            if (a.Dimension == b.Dimension)
            {
                for (int i = 0; i < a.Dimension; i++)
                    if (a.N[i] != b.N[i]) throw new Exception("张量减法错误，相加的张量形状不同");
                Tensor c = FromShape(a);
                for (int i = 0; i < a.Count; i++)
                {
                    c.X[i] = a.X[i] - b.X[i];
                }
                return c;
            }
            else
            {
                throw new Exception("张量减法错误，相加的张量形状不同");
            }
        }
        /// <summary>
        /// 张量数乘
        /// </summary>
        /// <param name="l"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Tensor operator *(double l,Tensor a)
        {
            Tensor b = new Tensor(a.N);
            for (int i = 0; i < b.Count; i++)
                b.X[i] = a.X[i] * l;
            return b;
        }
        
        public static Tensor operator *(Tensor a, Tensor b)
        {
            Tensor c;
            switch (a.Dimension)
            {
                case 0:
                    if (b.Dimension == 0)
                    {
                        return new Tensor(a.X[0] * b.X[0]);
                    }
                    else
                    {
                        c = FromShape(b);
                        for (int i = 0; i < c.Count; i++)
                            c.X[i] = b.X[i]*a.X[0];
                        return c;
                    }
                case 1:
                    if (b.Dimension == 0)
                    {
                        c = FromShape(a);
                        for (int i = 0; i < c.Count; i++)
                            c.X[i] =a.X[i]* b.X[0];
                        return c;
                    }
                    if (b.Dimension == 1)
                    {
                        if (a.N[0] == b.N[0])
                        {
                            c = new Tensor(a.N);
                            for (int i = 0; i < c.Count; i++)
                            {
                                c.X[i] = a.X[i] *b.X[i];
                            }
                            return c;
                        }
                        else
                            throw new Exception("张量乘法不合法");

                    }
                    if (b.Dimension == 2)
                    {
                        if (a.N[0] == b.N[0])
                        {
                            c = new Tensor(b);
                            for (int i = 0; i < c.N[0]; i++)
                            {
                                for (int j = 0; j < c.N[1]; j++)
                                {
                                    c.At(i, j) *= a.X[i];
                                }
                            }
                            return c;
                        }
                        else
                            throw new Exception("张量乘法不合法");
                    }
                    throw new Exception("不支持的维度的张量乘法");
                case 2:
                    switch (b.Dimension)
                    {
                        case 0:
                            c = new Tensor(a);
                            for (int i = 0; i < a.Count; i++)
                                c.X[i] =a.X[i] * b.X[0];
                            return c;
                        case 1:
                            if (a.N[1] == b.N[0])
                            {
                                c = new Tensor(a);
                                for (int i = 0; i < c.N[0]; i++)
                                {
                                    for (int j = 0; j < c.N[1]; j++)
                                    {
                                        c.At(i, j) *= b.X[j];
                                    }
                                }
                                return c;
                            }
                            else
                                throw new Exception("张量乘法不合法");

                        case 2:
                            if (a.N[1] == b.N[0])
                            {
                                int ind;
                                c = new Tensor(a.N[0], b.N[1]);
                                for (int i = 0; i < c.N[0]; i++)
                                {
                                    for (int j = 0; j < c.N[1]; j++)
                                    {
                                        ind = i * c.N[1] + j;
                                        for (int k = 0; k < a.N[1]; k++)
                                        {
                                            c.X[ind] += a.At(i, k) * b.At(k, j);
                                        }
                                    }
                                }
                                return c;
                            }
                            else
                                throw new Exception("张量乘法不合法");
                    }
                    throw new Exception("不支持的维度的张量乘法");
            }
            throw new Exception("不支持的维度的张量乘法");
        }
        public Vector3 ToVector3()
        {
            if (Dimension != 1 || Count != 3) throw new Exception("张量无法转化为Vector3");
            return new Vector3(X[0], X[1], X[2]);
        }
        /// <summary>
        /// 返回字符串形式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Dimension == 0)
            {
                return X[0].ToString();
            }
            else if (Dimension == 1) 
            {
                string s="{"+X[0].ToString();
                for(int i=1;i<Count;i++)
                {
                    s +="," +X[i].ToString();
                }
                s += "}";
                return s;
            }
            else if(Dimension==2)
            {
                string s = "{{";
                int t=0;
                for(int i=0;i<N[0];i++)
                {
                    s += X[t++].ToString();
                    for(int j=1;j<N[1];j++)
                    {
                        s +=","+X[t++].ToString();
                    }
                    if (i == N[0] - 1)
                        s += "}}";
                    else
                        s += "},{";
                }
                return s;
            }
            else
            {
                int AnyPos=0;
                return AnyToString(ref AnyPos, 0);
            }

        }
        /// <summary>
        /// 递归输出N阶张量各行各列的函数
        /// </summary>
        /// <param name="Pos"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private string AnyToString(ref int Pos, int c)
        {
            if (c == Dimension) return X[Pos++].ToString();
            string s = "{"+AnyToString(ref Pos,c+1);
           
            for(int i=1;i<N[c]; i++)
            {
               s += ","+AnyToString(ref Pos, c + 1);
            }
            return s;
        }
    }
    /// <summary>
    /// 张量的数学函数
    /// </summary>
    public static class MathT
    {
        public static void SwapRow(ref Tensor tensor, int i, int j)
        {
            double temp;
            for(int k=0;k<tensor.N[1];k++)
            {
                temp = tensor.At(i, k);
                tensor.At(i,k) = tensor.At(j, k);
                tensor.At(j, k) = temp;
            }
        }
        public static void SwapColumn(ref Tensor tensor, int i, int j)
        {
            double temp;
            for (int k = 0; k < tensor.N[0]; k++)
            {
                temp = tensor.At(k, i);
                tensor.At(k, i) = tensor.At(k, j);
                tensor.At(k, j) = temp;
            }
        }
        public static void AddToRow(ref Tensor tensor,int i,int j,double l)
        {
            for (int k = 0; k < tensor.N[1]; k++)
            {
                tensor.At(i, k)+= l * tensor.At(j, k);
            }
        }
        public static void AddToColumn(ref Tensor tensor, int i, int j,double l)
        {
            for (int k = 0; k < tensor.N[0]; k++)
            {
                tensor.At(k, i) += l * tensor.At(k, j);
            }
        }
        public static void MultiplyRow(ref Tensor tensor,int i, double l)
        {
            for (int k = 0; k < tensor.N[1]; k++)
            {
                tensor.At(i, k) += l;
            }
        }
        public static void MultiplyColumn(ref Tensor tensor, int i, double l)
        {
            for (int k = 0; k < tensor.N[0]; k++)
            {
                tensor.At(k, i) += l;
            }
        }
        public static Tensor Sin(Tensor tensor)
        {
            Tensor ret = new Tensor(tensor.N);
            for(int i=0;i<ret.Count;i++)
            {
                ret.X[i] = Math.Sin(tensor.X[i]);
            }
            return ret;
        }
        public static Tensor Cos(Tensor tensor)
        {
            Tensor ret = new Tensor(tensor.N);
            for (int i = 0; i < ret.Count; i++)
            {
                ret.X[i] = Math.Cos(tensor.X[i]);
            }
            return ret;
        }
        public static Tensor Exp(Tensor tensor)
        {
            Tensor ret = new Tensor(tensor.N);
            for (int i = 0; i < ret.Count; i++)
            {
                ret.X[i] = Math.Exp(tensor.X[i]);
            }
            return ret;
        }
        public static Tensor Tan(Tensor tensor)
        {
            Tensor ret = new Tensor(tensor.N);
            for (int i = 0; i < ret.Count; i++)
            {
                ret.X[i] = Math.Tan(tensor.X[i]);
            }
            return ret;
        }
        public static Tensor Cosh(Tensor tensor)
        {
            Tensor ret = new Tensor(tensor.N);
            for (int i = 0; i < ret.Count; i++)
            {
                ret.X[i] = Math.Cosh(tensor.X[i]);
            }
            return ret;
        }
        public static Tensor Sinh(Tensor tensor)
        {
            Tensor ret = new Tensor(tensor.N);
            for (int i = 0; i < ret.Count; i++)
            {
                ret.X[i] = Math.Sinh(tensor.X[i]);
            }
            return ret;
        }
        public static Tensor Tanh(Tensor tensor)
        {
            Tensor ret = new Tensor(tensor.N);
            for (int i = 0; i < ret.Count; i++)
            {
                ret.X[i] = Math.Tanh(tensor.X[i]);
            }
            return ret;
        }
        public static Tensor Log(Tensor tensor)
        {
            Tensor ret = new Tensor(tensor.N);
            for (int i = 0; i < ret.Count; i++)
            {
                ret.X[i] = Math.Log(tensor.X[i]);
            }
            return ret;
        }
        public static Tensor Sqrt(Tensor tensor)
        {
            Tensor ret = new Tensor(tensor.N);
            for (int i = 0; i < ret.Count; i++)
            {
                ret.X[i] = Math.Sqrt(tensor.X[i]);
            }
            return ret;
        }
        public static Tensor Max(Tensor tensor)
        {
            Tensor ret = new Tensor(double.MinValue);
            for (int i = 0; i < tensor.Count; i++)
            {
                if (tensor.X[i] > ret.X[0]) ret.X[0] = tensor.X[i];
            }
            return ret;
        }
        public static Tensor Min(Tensor tensor)
        {
            Tensor ret = new Tensor(double.MaxValue);
            for (int i = 0; i < tensor.Count; i++)
            {
                if (tensor.X[i] < ret.X[0]) ret.X[0] = tensor.X[i];
            }
            return ret;
        }
        public static Tensor Average(Tensor tensor)
        {
            Tensor ret = new Tensor(0);
            for (int i = 0; i < tensor.Count; i++)
            {
               ret.X[0]+= tensor.X[i];
            }
            ret.X[0] /= tensor.Count;
            return ret;
        }
        public static Tensor Asin(Tensor tensor)
        {
            Tensor ret = new Tensor(tensor.N);
            for(int i=0;i<ret.Count;i++)
            {
                ret.X[i] = Math.Asin(tensor.X[i]);
            }
            return ret;
        }
        public static Tensor Acos(Tensor tensor)
        {
            Tensor ret = new Tensor(tensor.N);
            for (int i = 0; i < ret.Count; i++)
            {
                ret.X[i] = Math.Acos(tensor.X[i]);
            }
            return ret;
        }
        public static Tensor Atan(Tensor tensor)
        {
            Tensor ret = new Tensor(tensor.N);
            for (int i = 0; i < ret.Count; i++)
            {
                ret.X[i] = Math.Atan(tensor.X[i]);
            }
            return ret;
        }
        public static Tensor Pow(Tensor tensor1, Tensor tensor2)
        {
            if (tensor2.Dimension == 0)
            {
                Tensor ret = new Tensor(tensor1.N);
                for(int i=0;i<ret.Count;i++)
                {
                    ret.X[i] = Math.Pow(tensor1.X[i], tensor2.X[0]);
                }
                return ret;
            }
            else
                throw new Exception("Pow函数的指数只能是标量");
        }
        public static Tensor MatrixInverse(Tensor tensor)
        {
            if (tensor.Dimension != 2 || tensor.N[0] != tensor.N[1])
            {
                throw new Exception("所求行列式不是方阵");
            }
            Tensor ret = Tensor.IdentityMatrix(tensor.N[0]);
            Tensor mat = new Tensor(tensor);
            int firstlinenot0, i, j;

            for (i = 0; i < tensor.N[0]; i++)
            {
                for (firstlinenot0 = i; firstlinenot0 < tensor.N[0] && 0 == mat.At(firstlinenot0, i); firstlinenot0++) ;
                if (firstlinenot0 >= tensor.N[0])
                {
                    throw new Exception("求逆的矩阵行列式为0");
                }
                if (firstlinenot0 != i)
                {
                    SwapRow(ref mat,i , firstlinenot0);
                    SwapRow(ref ret,i , firstlinenot0);
                }
                for (j = i + 1; j < tensor.N[0]; j++)
                {
                    if (mat.At(j, i) != 0)
                    {
                        AddToRow(ref mat, j, i, -mat.At(j, i) / mat.At(i, i));
                        AddToRow(ref ret, j, i, -mat.At(j, i) / mat.At(i, i));
                    }
                }
            }
            for (i = tensor.N[0]-1; i >= 0; i--)
            {
                MultiplyRow(ref ret, i, 1 / mat.At(i, i));
                for (j = i - 1; j >= 1; j--)
                {
                    if (mat.At(j, i) != 0)
                    {
                        AddToRow(ref mat, j, i, -mat.At(j, i));
                    }
                }
            }
            return ret;
        }
        public static Tensor MatrixDet(Tensor tensor)
        {
            if (tensor.Dimension!=2||tensor.N[0]!=tensor.N[1])
            {
                throw new Exception("所求行列式不是方阵");
            }
            Tensor mat=new Tensor(tensor);
            int firstlinenot0, i, j;

            bool sign = false;//是否取相反数

            for (i = 0; i < mat.N[0]; i++)
            {
                for (firstlinenot0 = i; firstlinenot0 < mat.N[0] && 0 == mat.At(firstlinenot0, i); firstlinenot0++) ;
                if (firstlinenot0 >= mat.N[0])
                    return new Tensor(0.0);

                if (firstlinenot0 != i)
                {
                    sign = !sign;
                    SwapRow(ref mat,i, firstlinenot0);
                }
                for (j = i + 1; j < mat.N[0]; j++)
                {
                    if (mat.At(j,i)!= 0)
                    {
                        AddToRow(ref mat, j, i, -mat.At(j, i) / mat.At(i, i));
                    }
                }
            }
            double ans = mat.At(0, 0);
            for (i = 1; i < mat.N[0]; i++)
            {
                ans *= mat.At(i, i);
            }

            if (sign) ans = -ans;
            return new Tensor(ans);
        }
        /// <summary>
        /// 返回张量的逆(0阶张量的逆即它的倒数，2阶张量是可逆方阵逆是其逆矩阵)
        /// </summary>
        /// <returns></returns>
        public static Tensor Inverse(Tensor tensor)
        {
            switch (tensor.Dimension)
            {
                case 0:
                    return new Tensor(1 / tensor.X[0]);
                case 2:
                    if (tensor.N[0] != tensor.N[1])
                    {
                        throw new Exception("求逆的矩阵不是方阵");
                    }
                    return MatrixInverse(tensor);
                default:
                    throw new Exception("暂不支持的求逆张量");
            }

        }
        /// <summary>
        /// 对函数f作单变量积分，start和end的第一个分量作为积分上限和下限
        /// </summary>
        /// <param name="f"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        /*public static Tensor Integrate(Functional.Function f, Tensor start, Tensor end)
        {

        }*/
    }
}
