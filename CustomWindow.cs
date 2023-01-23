using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathCS.MShapes;
using OpenTK.Mathematics;

namespace MathCS
{
    public class CustomWindow : MWindow
    {
        Stopwatch stopwatch = new Stopwatch();
        double elapsed;
        List<string> times = new List<string>();
        MLine line;
        MCircularRing ring;
        //MRectangle rectangle;
        MGraph graph;


        double theta = Math.PI/2;
        double omega = 1;
        double alpha = 0;

        public CustomWindow() : base(800, 600, "window")
        {
            stopwatch.Start();
            RenderFrequency = 144;
            UpdateFrequency = 144;
            ring = new MCircularRing(this, 0, 20);
            ring.DrawColor = MColor.Red;
            ring.Anchor = MAnchor.Center;

            line = new MLine(this, new(0, 0), new(100, 100));
            line.DrawColor = MColor.Black;
            line.Anchor = MAnchor.Center;
            line.Thickness = 10;

            /*
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
            */
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            elapsed += e.Time;
            times.Add((1.0/e.Time).ToString());

            double dt = e.Time;
            alpha = -Math.Cos(theta);
            omega += alpha * dt;
            theta += omega * dt;

            ring.Position = new ((int)(200 * Math.Cos(theta)), (int)(200 * Math.Sin(theta)));
            line.Position2 = ring.Position;

            var input = KeyboardState;
            if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnUnload()
        {
            //times.ForEach(Console.WriteLine);
            base.OnUnload();
        }
    }
}
