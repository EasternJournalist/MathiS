using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScienceAndMath
{
    class FunctionView3:View3
    {
        public Functional.Function Func;

        public double[,] ValueData;
        public Mesh MeshGrid;
        public double XMax, XMin, YMax, YMin, ZMax, ZMin;
        public int ResoluteX, ResoluteY;
        public List<ParallelLight> AmbientLight;
        public Vector3 GetPaintSpaceVec(Vector3 DataSpaceVec)
        {
            return new Vector3((DataSpaceVec.X - XMin) / (XMax - XMin) * 800 - 400, (DataSpaceVec.Y - YMin) / (YMax - YMin) * 800 - 400, (DataSpaceVec.Z - ZMin) / (ZMax - ZMin) * 800 - 400);
        }
        public Vector3 GetPaintSpaceVec(double x, double y, double z)
        {
            return new Vector3((x - XMin) / (XMax - XMin) * 800 - 400, (y - YMin) / (YMax - YMin) * 800 - 400, (z - ZMin) / (ZMax - ZMin) * 800 - 400);
        }
        public FunctionView3(Functional.Function f, ref Bitmap img)
        {
            Func = f;
            G = Graphics.FromImage(img);
            G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            ScreenCenterX = img.Width / 2;
            ScreenCenterY = img.Height / 2;
        }
        /// <summary>
        /// 生成采样点数值
        /// </summary>
        public void GenerateValueData()
        {
            double[,] vData = new double[ResoluteX, ResoluteY];
            double stepx = (XMax - XMin) / (ResoluteX - 1), stepy = (YMax - YMin) / (ResoluteY - 1);
            double pX = XMin, pY = YMin;
            ValueData = new double[ResoluteX, ResoluteY];
            for (int i = 0; i < ResoluteX; i++)
            {
                pY = YMin;
                for (int j = 0; j < ResoluteY; j++)
                {
                    ValueData[i, j] = Func(new Tensor(pX, pY)).X[0];
                    pY += stepy;
                }
                pX += stepx;
            }
        }
        /// <summary>
        /// 生成网格
        /// </summary>
        public void GenerateMeshGrid()
        {
            MeshGrid = new Mesh();
            double stepx = (XMax - XMin) / (ResoluteX - 1), stepy = (YMax - YMin) / (ResoluteY - 1);
            for (int i = 0; i < ResoluteX; i++)
            {
                for (int j = 0; j < ResoluteY; j++)
                {
                    MeshGrid.Vertex.Add(GetPaintSpaceVec(XMin + i * stepx, YMin + j * stepy, ValueData[i, j]));
                }
            }
            for (int i = 0; i < ResoluteX - 1; i++)
            {
                for (int j = 0; j < ResoluteY - 1; j++)
                {
                    MeshGrid.Surface.Add(
                        new Mesh.PolygonSurfaceType(
                            new List<int> { i * ResoluteY + j, (i + 1) * ResoluteY + j, i * ResoluteY + j + 1 },
                            new RGB_D(1, 1, 1),
                            BRDFuncSet.TrowbridgeReitz,
                            ref MeshGrid.Vertex
                            )
                        );
                    MeshGrid.Surface.Add(
                        new Mesh.PolygonSurfaceType(
                            new List<int> { (i + 1) * ResoluteY + j, (i + 1) * ResoluteY + j + 1, i * ResoluteY + j + 1 },
                           new RGB_D(1, 1, 1),
                            BRDFuncSet.TrowbridgeReitz,
                            ref MeshGrid.Vertex
                            )
                        );
                }
            }
        }
        public void DrawValueDataGrid()
        {
            for (int i = 0; i < ResoluteX - 1; i++)
            {
                for (int j = 0; j < ResoluteY - 1; j++)
                {
                    DrawLine(Pens.Blue, MeshGrid.Vertex[i * ResoluteY + j], MeshGrid.Vertex[i * ResoluteY + j + 1]);
                    DrawLine(Pens.Blue, MeshGrid.Vertex[i * ResoluteY + j], MeshGrid.Vertex[(i + 1) * ResoluteY + j]);
                }
            }
            for (int i = 0; i < ResoluteX - 1; i++)
            {
                DrawLine(Pens.Blue, MeshGrid.Vertex[i * ResoluteY + ResoluteY - 1], MeshGrid.Vertex[(i + 1) * ResoluteY + ResoluteY - 1]);
            }
            for (int i = 0; i < ResoluteY - 1; i++)
            {
                DrawLine(Pens.Blue, MeshGrid.Vertex[(ResoluteX - 1) * ResoluteY + i], MeshGrid.Vertex[(ResoluteY - 1) * ResoluteY + i + 1]);
            }
        }
        public void DrawMesh(Mesh mesh)
        {
            int[] PaintSortList = new int[mesh.Surface.Count];
            for (int i = 0; i < PaintSortList.Length; i++)
            {
                PaintSortList[i] = i;
            }

            System.Array.Sort(//对mesh中的网格绘制顺序排序，先绘制远处，再绘制近处
                PaintSortList,
                delegate (int i, int j) {
                    return
                    (ToWatcher(mesh.Vertex[mesh.Surface[i].VIndex[0]]) + ToWatcher(mesh.Vertex[mesh.Surface[i].VIndex[1]]) + ToWatcher(mesh.Vertex[mesh.Surface[i].VIndex[2]])
                    >
                    ToWatcher(mesh.Vertex[mesh.Surface[j].VIndex[0]]) + ToWatcher(mesh.Vertex[mesh.Surface[j].VIndex[1]]) + ToWatcher(mesh.Vertex[mesh.Surface[j].VIndex[2]])) ? -1 : 1;
                }
                );
            RGB_D dcolor;
            for (int i = 0; i < PaintSortList.Length; i++)
            {
                dcolor = new RGB_D(0, 0, 0);
                foreach (ParallelLight amblight in AmbientLight)
                {
                    dcolor += WorldModelRender.GetSurfaceLight(
                        mesh.Surface[PaintSortList[i]], amblight,
                        (camera.OPoint - mesh.Surface[PaintSortList[i]].Axis.OPoint).UnitVector()
                        );
                }
                FillTriangle(dcolor.ToColorRGB(), mesh.Vertex[mesh.Surface[PaintSortList[i]].VIndex[0]], mesh.Vertex[mesh.Surface[PaintSortList[i]].VIndex[1]], mesh.Vertex[mesh.Surface[PaintSortList[i]].VIndex[2]]);
            }
        }
        public void SetIlluminantDefault()
        {
            AmbientLight = new List<ParallelLight>();
            AmbientLight.Add(new ParallelLight() { Direction = new Vector3(0, 0, -1), LightIntensity = new RGB_D(200, 200, 200) });
        }

    }
}
