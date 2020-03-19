using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScienceAndMath
{
    /// <summary>
    /// 坐标架
    /// </summary>
    public class FrameofAxis
    {
        public Vector3 OPoint;
        public Vector3 XAxis,YAxis,ZAxis;
        public FrameofAxis()
        {
            OPoint = new Vector3(0, 0, 0);
            XAxis = new Vector3(1, 0, 0);
            YAxis = new Vector3(0, 1, 0);
            YAxis = new Vector3(0, 0, 1);
        }
        public FrameofAxis(Vector3 _OPoint, Vector3 _XAxis, Vector3 _YAxis, Vector3 _ZAxis)
        {
            OPoint = _OPoint;
            XAxis = _XAxis;
            YAxis = _YAxis;
            ZAxis = _ZAxis;
        }
        public FrameofAxis(Vector3 _OPoint, Vector3 V1,Vector3 V2)
        {
            OPoint = _OPoint;
            XAxis = (V1 - _OPoint).UnitVector();
            ZAxis = Vector3.CrossProduct(XAxis, V2 - OPoint).UnitVector();
            YAxis = Vector3.CrossProduct(ZAxis, XAxis);
        }
        /// <summary>
        /// 返回一个向量在这个基（坐标架）下的坐标
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector3 TransIn(Vector3 v)
        {
            Vector3 p = v - OPoint;
            return new Vector3(p * XAxis, p * YAxis, p * ZAxis);
        }
    }
    public struct Segement
    {
        Vector3 Vertex, Directoin;
    }
    public struct Line
    {
        Vector3 V1, V2;
    }
    public struct Triangle
    {
        public Vector3[] Vertex;
        
    }
    public class Geometrics
    {
        /// <summary>
        /// 返回一个空间向量V绕X轴旋转角度a后的向量。
        /// </summary>
        /// <param name="V"></param>
        /// 一个向量
        /// <param name="a"></param>
        /// 旋转的弧度a，（根据右手定则）
        /// <returns></returns>
        public static Vector3 RotateByX(Vector3 V, double a)
        {
            return new Vector3(V.X, V.Y * Math.Cos(a) - V.Z * Math.Sin(a), V.Y * Math.Sin(a) + V.Z * Math.Cos(a));
        }
        /// <summary>
        /// 返回一个空间向量V绕X轴旋转角度a后的向量，但参数给出的是Sin(a)和Cos(a)。
        /// </summary>
        /// <param name="V"></param>
        /// <param name="sin">要旋转的角度的正弦值</param>
        /// <param name="cos">要旋转的角度的余弦值</param>
        /// <returns></returns>
        public static Vector3 RotateByX(Vector3 V, double sin, double cos)
        {
            return new Vector3(V.X, V.Y * cos - V.Z * sin, V.Y * sin + V.Z * cos);
        }

        /// <summary>
        /// 返回一个空间向量V绕Y轴旋转角度a后的向量。
        /// </summary>
        /// <param name="V"></param>
        /// 一个向量
        /// <param name="a"></param>
        /// 旋转的弧度a（根据右手定则）
        /// <returns></returns>
        public static Vector3 RotateByY(Vector3 V, double a)
        {
            return new Vector3(V.X * Math.Cos(a) + V.Z * Math.Sin(a), V.Y, V.Z * Math.Cos(a) - V.X * Math.Sin(a));
        }
        /// <summary>
        /// 返回一个空间向量V绕X轴旋转角度a后的向量，但参数给出的是Sin(a)和Cos(a)。
        /// </summary>
        /// <param name="V"></param>
        /// <param name="sin">要旋转的角度的正弦值</param>
        /// <param name="cos">要旋转的角度的余弦值</param>
        /// <returns></returns>
        public static Vector3 RotateByY(Vector3 V, double sin, double cos)
        {
            return new Vector3(V.X * cos + V.Z * sin, V.Y, V.Z * cos + V.X * sin);
        }

        /// <summary>
        /// 返回一个空间向量V绕Z轴旋转角度a后的向量。
        /// </summary>
        /// <param name="V"></param>
        /// 一个向量
        /// <param name="a"></param>
        /// 旋转的弧度a（根据右手定则）
        /// <returns></returns>
        public static Vector3 RotateByZ(Vector3 V, double a)
        {
            return new Vector3(V.X * Math.Cos(a) - V.Y * Math.Sin(a), V.Y * Math.Cos(a) + V.X * Math.Sin(a), V.Z);
        }
        /// <summary>
        /// 返回一个空间向量V绕Z轴旋转角度a后的向量，但参数给出的是Sin(a)和Cos(a)。
        /// </summary>
        /// <param name="V"></param>
        /// <param name="sin">要旋转的角度的正弦值</param>
        /// <param name="cos">要旋转的角度的余弦值</param>
        /// <returns></returns>
        public static Vector3 RotateByZ(Vector3 V, double sin, double cos)
        {
            return new Vector3(V.X * cos - V.Y * sin, V.Y * cos + V.X * sin, V.Z);
        }
    }
    
}
