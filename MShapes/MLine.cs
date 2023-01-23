using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCS.MShapes
{
    public class MLine : MGraphicsObject
    {
        public Vector2i Position2 { get; set; }
        public int Thickness { get; set; }

        public MLine(MWindow window, Vector2i pos1, Vector2i pos2) : base(window)
        {
            Position = pos1;
            Position2 = pos2;   
        }

        void bline(int x1, int y1, int x2, int y2)
        {
            int xinc, yinc, x, y;
            int dx, dy, e;
            dx = Math.Abs(x2 - x1);
            dy = Math.Abs(y2 - y1);
            if (x1 < x2)
                xinc = 1;
            else
                xinc = -1;
            if (y1 < y2)
                yinc = 1;
            else
                yinc = -1;
            x = x1;
            y = y1;
            if (Window.IsOnScreen(new(x, y)))
            Window.Screen[x][y] = DrawColor;
            if (dx >= dy)
            {
                e = (2 * dy) - dx;
                while (x != x2)
                {
                    if (e < 0)
                    {
                        e += (2 * dy);
                    }
                    else
                    {
                        e += (2 * (dy - dx));
                        y += yinc;
                    }
                    x += xinc;
                    if (Window.IsOnScreen(new(x, y)))
                    Window.Screen[x][y] = DrawColor;
                }
            }
            else
            {
                e = (2 * dx) - dy;
                while (y != y2)
                {
                    if (e < 0)
                    {
                        e += (2 * dx);
                    }
                    else
                    {
                        e += (2 * (dx - dy));
                        x += xinc;
                    }
                    y += yinc;
                    if (Window.IsOnScreen(new(x, y)))
                    Window.Screen[x][y] = DrawColor;
                }
            }
        }

        public override void Draw()
        {
            int x1 = AnchoredPosition().X;
            int x2 = Position2.X + Window.PositionOfAnchor(Anchor, AnchorBorder).X;
            int y1 = AnchoredPosition().Y;
            int y2 = Position2.Y + Window.PositionOfAnchor(Anchor, AnchorBorder).Y;
            int dx = Math.Abs(x1 - x2);
            int dy = Math.Abs(y1 - y2);
            float L = MathF.Sqrt(dx * dx + dy * dy);

            bline(x1, y1, x2, y2);
            if (x2 - x1 != 0 && (y2 - y1) / (x2 - x1) < 1)
            {
                int wy = (int)((Thickness - 1) * L / (2 * dx));
                for (int i = 0; i < wy; i++)
                {
                    bline(x1, y1 - i, x2, y2 - i);
                    bline(x1, y1 + i, x2, y2 + i);
                }
            }
            else
            {
                int wx = (int)((Thickness - 1) * L / (2 * dy));
                for (int i = 0; i < wx; i++)
                {
                    bline(x1 - i, y1, x2 - i, y2);
                    bline(x1 + i, y1, x2 + i, y2);
                }
            }
        }
    }
}
