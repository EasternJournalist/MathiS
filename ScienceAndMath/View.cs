using System.Drawing;
using System.Collections.Generic;

namespace ScienceAndMath
{
    /// <summary>
    /// 提供3D视图
    /// </summary>
    class View3
    {
        /// <summary>
        /// 视角中心
        /// </summary>
        private Vector3 center;
        /// <summary>
        /// 观察者位置参数
        /// </summary>
        private double distance, longitude, latitude;
        /// <summary>
        /// 观察者到屏幕的距离
        /// </summary>
        private double watcherDistance;
        /// <summary>
        /// 摄像机坐标架
        /// </summary>
        public FrameofAxis camera;
        /// <summary>
        /// 绘制中心在显示窗口的中心坐标
        /// </summary>
        public float ScreenCenterX, ScreenCenterY;
        /// <summary>
        /// 绘图用Graphics
        /// </summary>
        public Graphics G;
        public SolidBrush TextBrush = new SolidBrush(Color.Black);
        /// <summary>
        /// 初始化一个默认视图，但其绘图输出对象未指定
        /// </summary>
        public View3()
        {
            watcherDistance = 2000;
            center = new Vector3(0,0,0);
            distance = 4000;
            longitude = 0;
            latitude = 0;
            ResetCamera();
        }
        /// <summary>
        /// 指定一个绘图输出位图对象，进行默认的视图初始化
        /// </summary>
        /// <param name="img">位图</param>
        public View3(ref Bitmap img)
        {
            SetDefaultCamera();
            G = Graphics.FromImage(img);
            ResetCamera();
        }
        /// <summary>
        /// 初始化一个看向中心的视图
        /// </summary>
        /// <param name="_center">中心位置</param>
        /// <param name="_distance">观察者到中心的距离</param>
        /// <param name="_longtitude">经度</param>
        /// <param name="_latitude">维度</param>
        /// <param name="_watcherDistance">观察者到屏幕的距离</param>
        /// <param name="img"></param>
        public View3(Vector3 _center,double _distance,double _longtitude, double _latitude, double _watcherDistance, ref Bitmap img)
        {
            watcherDistance = _watcherDistance;
            center = _center;
            distance = _distance;
            longitude = _longtitude;
            latitude = _latitude;
            G = Graphics.FromImage(img);
            ScreenCenterX = img.Width / 2;
            ScreenCenterY = img.Height / 2;
            ResetCamera();
        }
        /// <summary>
        /// 设置中心点
        /// </summary>
        public Vector3 Centre
        {
            get { return center; }
            set { center = value; ResetCamera(); }
        }
        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; ResetCamera(); }
        }
        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; ResetCamera(); }
        }
        public double Distance
        {
            get { return distance; }
            set { distance = value; ResetCamera(); }
        }
        /// <summary>
        /// 重新指定绘图输出位图对象
        /// </summary>
        /// <param name="img"></param>
        public void RedirectToBitmp(ref Bitmap img)
        {
            G = Graphics.FromImage(img);
            ScreenCenterX = img.Width / 2;
            ScreenCenterY = img.Height / 2;
        }
        /// <summary>
        /// 返回三维坐标p点在屏幕上应显示的位置
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public PointF TransVec(Vector3 p)
        {
            PointF ret = new PointF();
            Vector3 r1 = p - camera.OPoint;

            double s = -watcherDistance / (camera.ZAxis * r1);
            if (s < 0)
            {
                ret.X = float.NaN;
            }
            else
            {
                ret.X = (float)(r1 * camera.XAxis * s + ScreenCenterX);
                ret.Y = ScreenCenterY - (float)(r1 * camera.YAxis * s);
            }
            return ret;
        }
        /// <summary>
        /// 重设摄像机向量
        /// </summary>
        private void ResetCamera()//
        {
            camera = new FrameofAxis();
            camera.ZAxis = new Vector3(longitude, latitude);
            camera.OPoint = center + distance * new Vector3(longitude, latitude);
            camera.XAxis = Vector3.CrossProduct(new Vector3(0, 0, 1) , camera.ZAxis).UnitVector();
            camera.YAxis = Vector3.CrossProduct(camera.ZAxis , camera.XAxis);
        }
        /// <summary>
        /// 给定看向中心参数，设置摄像机
        /// </summary>
        /// <param name="Centre">中心</param>
        /// <param name="Distance">观察者到中心距离</param>
        /// <param name="Longitude">观察者所在经度</param>
        /// <param name="Latitude">观察者所在纬度</param>
        public void ResetCamera(Vector3 Centre, double Distance, double Longitude, double Latitude, double watcherdistance)
        {
            distance = Distance;
            longitude = Longitude;
            latitude = Latitude;
            center = Centre;
            watcherDistance = watcherdistance;
            ResetCamera();
        }

        public void SetDefaultCamera()
        {
            ResetCamera(new Vector3(0, 0, 0), 2000, 0.8, 0.4, 2000);
        }
        /// <summary>
        /// 给定观察者位置，视线方向，设置摄像机
        /// </summary>
        /// <param name="Watcher"></param>
        /// <param name="Direction"></param>
        /// <param name="WatcherDistance"></param>
        public void SetVision(Vector3 Watcher, Vector3 Direction)
        {
            camera.ZAxis = Direction.UnitVector();
            camera.XAxis = Vector3.CrossProduct(new Vector3(0, 0, 1) ,Direction).UnitVector();
            camera.YAxis = Vector3.CrossProduct(camera.ZAxis , camera.XAxis);
        }
        /// <summary>
        /// 返回P点到观察者的距离
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public double ToWatcher(Vector3 p)
        {
            return (p - camera.OPoint).Modulus();
        }
        /// <summary>
        /// 调用Vision3D内部的G直接绘制平面直线
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="P1">第一个电脑</param>
        /// <param name="P2"></param>
        public void DrawLine(Pen pen, PointF P1, PointF P2)//绘制2D直线
        {
            if ((P1.X != float.NaN) && (P2.X != float.NaN))
            {
                G.DrawLine(pen, P1, P2);
            }
        }
        /// <summary>
        /// 绘制三维直线
        /// </summary>
        /// <param name="pen">所用画笔</param>
        /// <param name="P1">端点</param>
        /// <param name="P2">端点</param>
        public void DrawLine(Pen pen, Vector3 P1, Vector3 P2)//绘制3D直线
        {
            PointF p1 = TransVec(P1), p2 = TransVec(P2);
            if ((p1.X != float.NaN) && (p2.X != float.NaN))
            {
                G.DrawLine(pen, p1, p2);
            }
        }
        public void DrawLines(Pen pen, List<Vector3> ps)
        {
            PointF p1, p2;
            for(int i=1;i<ps.Count;i++)
            {
                p1 = TransVec(ps[i - 1]);
                p2 = TransVec(ps[i]);
                if ((p1.X != float.NaN) && (p2.X != float.NaN))
                {
                    G.DrawLine(pen, p1, p2);
                }
            }
        }
        /// <summary>
        /// 绘制一个三维空间中的点（大致地绘制一个指定半径的球）
        /// </summary>
        /// <param name="brush">绘制用的图形笔刷</param>
        /// <param name="position">三维坐标</param>
        /// <param name="radius">三维空间中的半径</param>
        public void PaintPlot(Brush brush, Vector3 position, double radius)
        {
            PointF P1 = TransVec(position);
            if (P1.X != float.NaN)
            {
                float r = (float)(radius * watcherDistance / ToWatcher(position));
                G.FillEllipse(brush, P1.X - r, P1.Y - r, r * 2, r * 2);
            }
        }
        /// <summary>
        /// 绘制一个三维空间中的点（大致地绘制一个球，并绘制视图导向尺）
        /// </summary>
        /// <param name="brush">绘制用的图形笔刷</param>
        /// <param name="position">三维坐标</param>
        /// <param name="radius">三维空间中的半径</param>
        /// <param name="RulerPen">三维导向尺</param>
        public void PaintPlot(Brush brush, Vector3 position, double radius, Pen RulerPen)
        {
            PointF P1 = TransVec(position);
            if (P1.X != float.NaN)
            {
                float r = (float)(radius * watcherDistance / ToWatcher(position));
                PointF P2 = TransVec(new Vector3(position.X, position.Y, 0));
                DrawLine(RulerPen, P1, P2);
                DrawLine(RulerPen, TransVec(new Vector3(0, position.Y, 0)), P2);
                DrawLine(RulerPen, TransVec(new Vector3(position.X, 0, 0)), P2);
                G.FillEllipse(brush, P1.X - r, P1.Y - r, r * 2, r * 2);
            }
        }
        /// <summary>
        /// 使用指定的样式，绘制一个三维空间中的点
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="position"></param>
        /// <param name="style">
        /// style=0 普通的画点
        /// 
        /// style=1 画小圈
        /// style=2 画双层小圈
        /// style=3 画小叉
        /// style=4 画三角形
        /// style=5 画正方形
        /// </param>
        public void PaintPlot(Color color, Vector3 position, string style)
        {
            PointF P1 = TransVec(position); // P2 = TransVec(new Vector3(position.X, position.Y, 0));
            Pen pen = new Pen(color);
            PointF[] ps;
            switch (style)
            {
                case "Dot1":
                    Brush brush = new SolidBrush(color);
                    G.FillEllipse(brush, P1.X - 3, P1.Y - 3, 6, 6);
                    break;
                case "Dot2":
                    G.DrawEllipse(pen, P1.X - 4, P1.Y - 4, 8, 8);
                    break;
                case "Dot3":
                    G.DrawEllipse(pen, P1.X - 3, P1.Y - 3, 6, 6);
                    G.DrawEllipse(pen, P1.X - 5, P1.Y - 5, 10, 10);
                    break;
                case "Cross":
                    G.DrawLine(pen, P1.X - 4, P1.Y - 4, P1.X + 4, P1.Y + 4);
                    G.DrawLine(pen, P1.X - 4, P1.Y + 4, P1.X + 4, P1.Y - 4);
                    break;
                case "Triangle":
                    ps = new PointF[3];
                    ps[0] = new PointF(P1.X, P1.Y - 4);
                    ps[1] = new PointF((float)(P1.X - 3.76), P1.Y + 2);
                    ps[2] = new PointF((float)(P1.X + 3.76), P1.Y + 2);
                    G.DrawPolygon(pen, ps);
                    break;
                case "Squre":
                    ps = new PointF[4];
                    ps[0] = new PointF(P1.X - 4, P1.Y - 4);
                    ps[1] = new PointF(P1.X + 4, P1.Y - 4);
                    ps[2] = new PointF(P1.X + 4, P1.Y + 4);
                    ps[3] = new PointF(P1.X - 4, P1.Y + 4);
                    G.DrawPolygon(pen, ps);
                    break;
            }
        }
        public void DrawTriangle(ref Color color, Vector3 V1, Vector3 V2, Vector3 V3)
        {
            PointF[] ps = new PointF[3];
            ps[0] = TransVec(V1);
            if (ps[0].X != float.NaN)
            {
                ps[1] = TransVec(V2);
                if (ps[1].X != float.NaN)
                {
                    ps[2] = TransVec(V3);
                    if (ps[2].X != float.NaN)
                    {
                        G.FillPolygon(new SolidBrush(color), ps);
                    }
                }
            }
        }
        public void FillTriangle(Color color, Vector3 V1,Vector3 V2,Vector3 V3)
        {  
            PointF[] ps = new PointF[3];
            ps[0] = TransVec(V1);
            if(ps[0].X!=float.NaN)
            {
                ps[1] = TransVec(V2);
                if(ps[1].X!=float.NaN)
                {
                    ps[2] = TransVec(V3);
                    if(ps[2].X!=float.NaN)
                    {
                        G.FillPolygon(new SolidBrush(color), ps);
                    }
                }
            }
        }
    }

    class MeshView:View3
    {
        public MeshView()
        {

        }
        public MeshView(ref Bitmap img)
        {
            G = Graphics.FromImage(img);
            G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            ScreenCenterX = img.Width / 2;
            ScreenCenterY = img.Height / 2;
        }
        public List<ParallelLight> AmbientLight;
        /// <summary>
        /// 绘制网格
        /// </summary>
        /// <param name="mesh"></param>
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
        /// <summary>
        /// 设置默认光照
        /// </summary>
        public void SetIlluminantDefault()
        {
            AmbientLight = new List<ParallelLight>();
            AmbientLight.Add(new ParallelLight() { Direction = new Vector3(0, 0, -1), LightIntensity = new RGB_D(200, 200, 200) });
        }
        /// <summary>
        /// 画网格线
        /// </summary>
        /// <param name="color"></param>
        /// <param name="mesh"></param>
        public void DrawGird(Color color, Mesh mesh)
        {
            for(int i=0;i<mesh.Surface.Count;i++)
            {
                DrawTriangle(ref color, 
                    mesh.Vertex[mesh.Surface[i].VIndex[0]],
                    mesh.Vertex[mesh.Surface[i].VIndex[1]],
                    mesh.Vertex[mesh.Surface[i].VIndex[2]]);
            }
        }
    }
    
    /// <summary>
    /// 提供2D视图
    /// </summary>
    class View2
    {
        public Graphics G;
        public double MinX,MinY,MaxX, MaxY;
        
    }

    /// <summary>
    /// 提供弱投影视图，相较于View3有更快的速度，适合显示数据
    /// </summary>
    class View3F
    {
        public float ScreenCenterX,ScreenCenterY;
    }
}
