using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCS.MShapes
{
    public class MRectangle : MGraphicsObject
    {
        public Vector2i Size;

        public MRectangle(MWindow window) : base(window) { }
        public override void Draw()
        {
            Rectangle(AnchoredPosition() - Size / 2, AnchoredPosition() + Size / 2, DrawColor);
        }

        private void Rectangle(Vector2i botLeft, Vector2i topRight, MColor color)
        {
            if (Window.IsOnScreen(botLeft) && Window.IsOnScreen(topRight))
            {
                for (int i = botLeft.X; i < topRight.X; i++)
                {
                    for (int j = botLeft.Y; j < topRight.Y; j++)
                    {
                        Window.Screen[i][j] = color;
                    }
                }
            }
            else
            {
                for (int i = botLeft.X; i < topRight.X; i++)
                {
                    for (int j = botLeft.Y; j < topRight.Y; j++)
                    {
                        if (Window.IsOnScreen(i, j))
                            Window.Screen[i][j] = color;
                    }
                }
            }
        }
    }
}
