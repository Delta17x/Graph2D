using MathCS.MShapes;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCS
{
    public class MGraph : MGraphicsObject
    {
        private delegate void DrawTask();
        public delegate void MPointDraw(Vector2i point, MGraph graph);

        public uint StepCount { get; set; }
        /// <summary>
        /// Where the position of whatever is drawn is relative to (i.e. MAnchor.Right would cause a rectangle centered at (0, 0) to be drawn at the right edge of the screen.)
        /// </summary>
        public MColor Background { get; set; }
        public Vector2i Size { get; set; }

        public float Scale { get; set; }

        private List<DrawTask> DrawTasks { get; set; } = new List<DrawTask>();

        /// <summary>
        /// What to draw at each point of the function.
        /// </summary>
        public MPointDraw PointDraw { get; set; } = (x, g) => 
        {
            MCircularRing ring = new(g.Window, 0, 2)
            {
                Position = x,
                DrawColor = g.DrawColor,
            };
            ring.Draw();
            ring.Destroy();
        };//w.Screen[x.X][x.Y] = c; };

        public MGraph(MWindow window, Vector2i position, Vector2i size) : base(window)
        {
            Position = position;
            Size = size;
        }

        public void GraphParametric(MFunction<Vector2, float> function, (float, float) tRange, uint stepCount = 200)
        {
            DrawTasks.Add(() =>
            {
                float t = tRange.Item1;
                float stepSize = (tRange.Item2 - tRange.Item1) / stepCount;
                while (t <= tRange.Item2)
                {
                    Vector2 vec2 = Scale * function(t);
                    // if point is on the graph
                    if (vec2.X > -Size.X * 0.5f && vec2.X < Size.X * 0.5f && 
                        vec2.Y > -Size.Y * 0.5f && vec2.Y < Size.X * 0.5f)
                    {
                        PointDraw(new Vector2i((int)vec2.X, (int)vec2.Y) + AnchoredPosition(), this);
                    }
                   

                    t += stepSize;
                }


            });
        }

        public override void Draw()
        {
            DrawTasks.ForEach(x => x.Invoke());
        }
    }
}
