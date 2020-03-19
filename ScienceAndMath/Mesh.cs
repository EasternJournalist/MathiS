using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScienceAndMath
{
    /// <summary>
    /// 双向反射分布函数（BRDF）类型
    /// </summary>
    /// <param name="InDir">入射光方向</param>
    /// <param name="OutDir">出射光方向</param>
    /// <param name="Norm">法向</param>
    /// <returns>光强之比</returns>
    public delegate double BRDFunction(Vector3 InDir,Vector3 OutDir, Vector3 Norm);
    /// <summary>
    /// 预设的一些表面BRDF
    /// </summary>
    public class BRDFuncSet
    {
        /// <summary>
        /// 理想散射表面
        /// </summary>
        /// <param name="InDir"></param>
        /// <param name="OutDir"></param>
        /// <param name="Norm"></param>
        /// <returns></returns>
        public static double Lambertian(Vector3 InDir, Vector3 OutDir, Vector3 Norm)
        {
            return (InDir * Norm) * (OutDir * Norm) < 0 ? 1 : 0;
        }
        /// <summary>
        /// TrowbridgeReitz 模型
        /// </summary>
        /// <param name="InDir"></param>
        /// <param name="OutDir"></param>
        /// <param name="Norm"></param>
        /// <returns></returns>
        public static double TrowbridgeReitz(Vector3 InDir, Vector3 OutDir, Vector3 Norm)
        {
            if ((InDir * Norm) * (OutDir * Norm) > 0) return 0;
            double cosAng = (OutDir - InDir).UnitVector() * Norm;
            cosAng *= 12 *(1 - cosAng * cosAng) + 1;
            return 25 / cosAng / cosAng;
        }
        /// <summary>
        /// 透射的 TrowbridgeReitz 模型
        /// </summary>
        /// <param name="InDir"></param>
        /// <param name="OutDir"></param>
        /// <param name="Norm"></param>
        /// <returns></returns>
        public static double TrowbridgeReitz_Trans(Vector3 InDir, Vector3 OutDir, Vector3 Norm)
        {
            double cosAng = (OutDir - InDir).UnitVector() * Norm;
            cosAng *= 6.389 * cosAng * cosAng + 1;
            return 54.598 / cosAng / cosAng / 2;
        }
        
    }
   
    /// <summary>
    /// 每个分量都是double的RGB类型
    /// </summary>
    public struct RGB_D
    {
        public double R, G, B;
        public RGB_D(double _R,double _G,double _B)
        {
            R = _R;
            G = _G;
            B = _B;
        }
        public System.Drawing.Color ToColorRGB()
        {
            return System.Drawing.Color.FromArgb((int)(255  - 65025.0 / (R + 255.0)), (int)(255 - 65025.0 / (G + 255.0)), (int)(255 - 65025.0 / (B + 255.0)));
        }
        public static RGB_D operator +(RGB_D a,RGB_D b)
        {
            return new RGB_D(a.R + b.R, a.G + b.G, a.B + b.B);
        }
        public static RGB_D operator *(RGB_D a,RGB_D b)
        {
            return new RGB_D(a.R * b.R, a.G * b.G, a.B * b.B);
        }
        public static RGB_D operator *(RGB_D a, double b)
        {
            return new RGB_D(a.R * b, a.G * b, a.B * b);
        }
    }
    /// <summary>
    /// 平行光
    /// </summary>
    public struct ParallelLight
    {
        public Vector3 Direction;
        public RGB_D LightIntensity;
    }
    /// <summary>
    /// 网格类型
    /// </summary>
    public class Mesh
    {
        public class EdgeType
        {
            public int V1, V2;
            public EdgeType(int v1,int  v2)
            {
                V1 = v1;V2 = v2;
            }
        }
        public class PolygonSurfaceType
        {
            /// <summary>
            /// 面的顶点
            /// </summary>
            public int[] VIndex;
            /// <summary>
            /// 面的坐标架
            /// </summary>
            public FrameofAxis Axis;
            /// <summary>
            ///  面的光学特性
            /// </summary>
            public BRDFunction BRDFunc;
            /// <summary>
            /// 在渲染过程中，是否照亮(自主发出或反射足够强的光)
            /// </summary>
            public bool IsIlluminated;
            public RGB_D BaseColor;
            public PolygonSurfaceType(List<int> _vIndex,RGB_D _BaseColor, BRDFunction _BRDFunc,ref List<Vector3> _Vertex)
            {
                VIndex = new int[_vIndex.Count];
                _vIndex.CopyTo(VIndex, 0);
                Axis = new FrameofAxis(_Vertex[VIndex[0]], _Vertex[VIndex[1]], _Vertex[VIndex[2]]);
                BaseColor = _BaseColor;
                BRDFunc = _BRDFunc;
                IsIlluminated = false;

            }
        }
        public Mesh()
        {
            Vertex = new List<Vector3>();
            Edge = new List<EdgeType>();
            Surface = new List<PolygonSurfaceType>();
        }
        public List<Vector3> Vertex;
        public List<EdgeType> Edge;
        public List<PolygonSurfaceType> Surface;
    }

    public class WorldModelRender
    {
        /// <summary>
        /// 环境平行光
        /// </summary>
        public List<ParallelLight> AmbientLight;
        public Mesh ModelMesh;
        public static RGB_D GetSurfaceLight(Mesh.PolygonSurfaceType s, ParallelLight l,Vector3 Dir)
        {
            return l.LightIntensity *s.BaseColor* (s.BRDFunc(l.Direction, Dir, s.Axis.ZAxis)*Math.Abs(( l.Direction * s.Axis.ZAxis)));
        }
    }
}
