using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathCS.MShapes;
using OpenTK.Mathematics;
using Graph2D.MShapes;

namespace MathCS
{
    public class CustomWindow : MWindow
    {
        Stopwatch stopwatch = new Stopwatch();
        double elapsed;
        List<string> times = new List<string>();
        MCircularRing ring;

        MPolygon polygon;
        MPolygon polygon2;
        MCircularRing ring2;
        //MRectangle rectangle;
        MGraph graph;

        double t0 = -0.349066;

        double theta = 0;

        double theta2 = 0;
        double omega = 30;
        double alpha = 0;

        double omega2 = 3;
        double alpha2 = 0;


        public CustomWindow() : base(800, 600, "window")
        {
            theta = theta2 = t0;

            stopwatch.Start();
            RenderFrequency = 144;
            UpdateFrequency = 144;
            
            ring = new MCircularRing(this, 0, 20);
            ring.DrawColor = MColor.Red;

            ring2 = new MCircularRing(this, 0, 20);
            ring2.DrawColor = MColor.Blue;

            polygon = new MPolygon(this, new Vector2i[] { new(0, 0), new(0, 0), new(0, 0), new(0, 0) });
            polygon.DrawColor = MColor.Black;

            polygon2 = new MPolygon(this, new Vector2i[] { new(0, 0), new(0, 0), new(0, 0), new(0, 0) });
            polygon2.DrawColor = MColor.Black;


            
            graph = new MGraph(this, new(0, 0), new(250, 250));
            graph.Anchor = MAnchor.Center;
            graph.DrawColor = MColor.Red;
            graph.Scale = 100;
            graph.PointDraw = (x, g) =>
            {
                MRectangle mRectangle = new(g.Window)
                {
                    Position = x,
                    Size = new(4, 4),
                    DrawColor = g.DrawColor
                };
                mRectangle.Draw();
                mRectangle.Destroy();
            };
            graph.GraphParametric((x) => new Vector2(MathF.Cos(x * (float)stopwatch.Elapsed.TotalSeconds), MathF.Sin(x)), (0, 2 * 3.14159f), 20000);
            graph.Visible = false;
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            elapsed += e.Time;
            times.Add((1.0/e.Time).ToString());

            double dt = e.Time;
            alpha = -Math.Sin(theta) - 0.3f * omega;
            omega += alpha * dt;
            theta += omega * dt;


            alpha2 = -Math.Sin(theta2);
            omega2 += alpha2 * dt;
            theta2 += omega2 * dt;

            ring.Position = new ((int)(200 * Math.Sin(theta)), -(int)(200 * Math.Cos(theta)));
            polygon.Vertices[0] = new((int)(5 * Math.Cos(theta)), (int)(5 * Math.Sin(theta)));
            polygon.Vertices[1] = -polygon.Vertices[0];

            polygon.Vertices[2] = ring.Position + new Vector2i((int)(5 * Math.Cos(theta)), (int)(5 * Math.Sin(theta)));
            polygon.Vertices[3] = ring.Position - new Vector2i((int)(5 * Math.Cos(theta)), (int)(5 * Math.Sin(theta)));

            ring2.Position = new((int)(200 * Math.Sin(theta2)), -(int)(200 * Math.Cos(theta2)));
            polygon2.Vertices[0] = new((int)(5 * Math.Cos(theta2)), (int)(5 * Math.Sin(theta2)));
            polygon2.Vertices[1] = -polygon2.Vertices[0];

            polygon2.Vertices[2] = ring2.Position + new Vector2i((int)(5 * Math.Cos(theta2)), (int)(5 * Math.Sin(theta2)));
            polygon2.Vertices[3] = ring2.Position - new Vector2i((int)(5 * Math.Cos(theta2)), (int)(5 * Math.Sin(theta2)));


            //double theta2 =
            //ring2.Position = new((int)(200 * Math.Cos(theta2)), (int)(200 * Math.Sin(theta2))); 

            var input = KeyboardState;
            if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnUnload()
        {
            times.ForEach(Console.WriteLine);
            base.OnUnload();
        }
    }
}
