using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScienceAndMath
{

    /// <summary>
    /// 参数曲面
    /// </summary>
    class ParametricView3 : MeshView
    {
        /// <summary>
        /// 参数方程
        /// </summary>
        public Functional.Function ParametricSurfaceFunction;
        /// <summary>
        /// 参数u或者v的范围
        /// </summary>
        public double Para_u_Max, Para_v_Max, Para_u_Min, Para_v_Min;
        /// <summary>
        /// 空间的范围
        /// </summary>
        public double XMax, XMin, YMax, YMin, ZMax, ZMin;
        /// <summary>
        /// 对参数u和v的分辨率
        /// </summary>
        public int ResolutionU, ResolutionV;

        public Mesh meshGrid;
        public Vector3 GetPaintSpaceVec(Vector3 DataSpaceVec)
        {
            return new Vector3((DataSpaceVec.X - XMin) / (XMax - XMin) * 800 - 400, (DataSpaceVec.Y - YMin) / (YMax - YMin) * 800 - 400, (DataSpaceVec.Z - ZMin) / (ZMax - ZMin) * 800 - 400);
        }
        public Vector3 GetPaintSpaceVec(double x, double y, double z)
        {
            return new Vector3((x - XMin) / (XMax - XMin) * 800 - 400, (y - YMin) / (YMax - YMin) * 800 - 400, (z - ZMin) / (ZMax - ZMin) * 800 - 400);
        }
        public ParametricView3(ref System.Drawing.Bitmap img) : base(ref img)
        {

        }
        /// <summary>
        /// 生成网格
        /// </summary>
        public void GenerateMeshGrid()
        {
            double stepu=(Para_u_Max-Para_u_Min)/(ResolutionU-1), stepv=(Para_v_Max-Para_v_Min)/(ResolutionV-1);
            double u=Para_u_Min, v;

            meshGrid = new Mesh();
            
            for (int i=0;i<ResolutionU ;i++)
            {
                v = Para_v_Min;
                for(int j=0;j<ResolutionV;j++)
                {
                    meshGrid.Vertex.Add(ParametricSurfaceFunction(new Tensor(u, v)).ToVector3()); 
                    v += stepv;
                }
                u += stepu;
            }
            for (int i = 0; i < ResolutionU - 1; i++)
            {
                for (int j = 0; j < ResolutionV - 1; j++)
                {
                    meshGrid.Surface.Add(
                        new Mesh.PolygonSurfaceType(
                            new List<int> { i * ResolutionV + j, (i + 1) * ResolutionV + j, i * ResolutionV + j + 1 },
                            new RGB_D(1, 1, 1),
                            BRDFuncSet.TrowbridgeReitz,
                            ref meshGrid.Vertex
                            )
                        );
                    meshGrid.Surface.Add(
                        new Mesh.PolygonSurfaceType(
                            new List<int> { (i + 1) * ResolutionV + j, (i + 1) * ResolutionV + j + 1, i * ResolutionV + j + 1 },
                           new RGB_D(1, 1, 1),
                            BRDFuncSet.TrowbridgeReitz,
                            ref meshGrid.Vertex
                            )
                        );
                }
            }
        }
        
    }
}
