using MathCS;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph2D.MShapes
{
    public class MPolygon : MGraphicsObject
    {
        public Vector2i[] Vertices { get; set; }

        public void DrawTriangle(Vector2i v1, Vector2i v2, Vector2i v3)
        {
            // Try to move as much out of the loop as possible

            // Get the axis-aligned bounding box
            int aabbx1 = (int)Math.Min(v1.X, Math.Min(v2.X, v3.X));
            int aabby1 = (int)Math.Min(v1.Y, Math.Min(v2.Y, v3.Y));
            int aabbx2 = (int)Math.Max(v1.X, Math.Max(v2.X, v3.X));
            int aabby2 = (int)Math.Max(v1.Y, Math.Max(v2.Y, v3.Y));

            Vector2i baDiff = v2 - v1;
            Vector2i cbDiff = v3 - v2;
            Vector2i caDiff = v3 - v1;

            // Used to calculate if point is in triangle
            float p1 = baDiff.X * v1.Y - baDiff.Y * v1.X;
            float p2 = caDiff.X * v1.Y - caDiff.Y * v1.X;
            float p3 = cbDiff.X * v2.Y - cbDiff.Y * v2.X;


            if (Window.IsOnScreen(v1.X, v1.Y) && Window.IsOnScreen(v2.X, v2.Y) && Window.IsOnScreen(v3.X, v3.Y))
            {
                for (int i = aabbx1; i < aabbx2 + 1; i++) 
                {
                    for (int j = aabby1; j <= aabby2; j++)
                    {

                        bool s_ab = baDiff.X * j - baDiff.Y * i > p1;

                        if (!(caDiff.X * j - caDiff.Y * i > p2 == s_ab || cbDiff.X * j - cbDiff.Y * i > p3 != s_ab))
                        {
                            Window[i, j] = DrawColor;

                        }

                    }
                }
                return;
            }


            // Same as above, just that we check if every fragment is inside of the screen.
            for (int i = aabbx1; i < aabbx2 + 1; i++)
            {
                for (int j = aabby1; j <= aabby2; j++)
                {
                    if (Window.IsOnScreen(i, j))
                    {
                        bool s_ab = baDiff.X * j - baDiff.Y * i > p1;

                        if (!(caDiff.X * j - caDiff.Y * i > p2 == s_ab || cbDiff.X * j - cbDiff.Y * i > p3 != s_ab))
                        {
                            Window[i, j] = DrawColor;
                        }
                    }
                }
            }

        }

        public MPolygon(MWindow window, Vector2i[] vertices) : base(window)
        {
            Vertices = vertices;
            Anchor = MAnchor.Center;
        }

        public override void Draw()
        {
            if (Vertices.Length == 3)
                DrawTriangle(Vertices[0] + AnchorPosition(), Vertices[1] + AnchorPosition(), Vertices[2] + AnchorPosition());
            else if (Vertices.Length == 4)
            {
                DrawTriangle(Vertices[0] + AnchorPosition(), Vertices[1] + AnchorPosition(), Vertices[2] + AnchorPosition());
                DrawTriangle(Vertices[3] + AnchorPosition(), Vertices[1] + AnchorPosition(), Vertices[2] + AnchorPosition());
            }
        }
    }
}
