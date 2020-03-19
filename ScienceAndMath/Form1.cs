using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScienceAndMath
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        NumericSymbol NSCalculator = new NumericSymbol();
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        public int PicControlMouseDownX, PicControlMouseDownY;
        public bool MousePressing = false;
        Bitmap funcImg;
        FunctionView3 fview;
        ParametricView3 pview;


        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData==Keys.Enter)
            {
                List<string> paramlist = new List<string>();
                paramlist.Add("x");
                paramlist.Add("y");
                Functional.Function f;
                f =NSCalculator.AssembleFunction(textBox1.Text, ref paramlist);
                
                listBox1.Items.Add("Input:" + textBox1.Text + " . value =" +f(new Tensor(0.0,0.0)).ToString());
                
                /*fview.Func = f;
                fview.SetIlluminantDefault();
                fview.SetDefaultCamera();
                fview.G.Clear(Color.FromArgb(0));
                fview.GenerateValueData();
                fview.GenerateMeshGrid();
                fview.DrawValueDataGrid();
                fview.DrawMesh(fview.MeshGrid);
                pictureBox1.Image = funcImg;
                pictureBox1.Refresh();*

                pview.ParametricSurfaceFunction = f;
                pview.SetIlluminantDefault();
                pview.G.Clear(Color.FromArgb(0));
                pview.GenerateMeshGrid();
                pview.DrawGird(Color.FromArgb(255,255,0,0),pview.meshGrid);
                pview.DrawMesh(pview.meshGrid);
                pictureBox1.Refresh();*/
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(MousePressing)
            {
                
                int dx = e.X - PicControlMouseDownX;
                int dy = e.Y - PicControlMouseDownY;

                /*if (dy > 0 && fview.Latitude < 1.5) fview.Latitude += dy / 1500.0;
                if (dy < 0 && fview.Latitude > -1.5) fview.Latitude += dy / 1500.0;
                fview.Longitude -= dx / 1000.0;
                
                
                fview.G.Clear(Color.FromArgb(0));
                fview.DrawValueDataGrid();
                fview.DrawMesh(fview.MeshGrid);
                pictureBox1.Refresh();*/

                if (dy > 0 && pview.Latitude < 1.5) pview.Latitude += dy / 1500.0;
                if (dy < 0 && pview.Latitude > -1.5) pview.Latitude += dy / 1500.0;
                pview.Longitude -= dx / 1000.0;


                pview.G.Clear(Color.FromArgb(0));
                pview.DrawMesh(pview.meshGrid);
                pictureBox1.Refresh();
                PicControlMouseDownX = e.X;
                PicControlMouseDownY = e.Y;

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            funcImg = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Functional.Function f = delegate { return new Tensor(0.0); };
            /*fview = new FunctionView3(f, ref funcImg)
            {
                XMax = 50,
                YMax = 50,
                ZMax = 50,
                XMin = -50,
                YMin = -50,
                ZMin = -50,
                ResoluteX = 50,
                ResoluteY = 50
            };
            fview.G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            fview.SetIlluminantDefault();
            fview.SetDefaultCamera();*/
            pview = new ParametricView3(ref funcImg)
            {
                XMax = 50,
                YMax = 50,
                ZMax = 50,
                XMin = -50,
                YMin = -50,
                ZMin = -50,
                ResolutionU = 100,
                ResolutionV = 400,
                Para_u_Max = 2 * Constants.π,
                Para_u_Min = 0,
                Para_v_Max = Constants.π,
                Para_v_Min = 0
            };
            pview.G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            pview.SetIlluminantDefault();
            pview.SetDefaultCamera();
            pictureBox1.Image = funcImg;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            MousePressing = false;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            MousePressing = true;
            PicControlMouseDownX = e.X;
            PicControlMouseDownY = e.Y;
        }
    }

}
