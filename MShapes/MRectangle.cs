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
        public Vector2i Position;


        public MRectangle(MWindow window) : base(window) 
        {
            Anchor = MAnchor.Center;

        }
        public override void Draw()
        {
            Rectangle(Position + AnchorPosition() - Size / 2, Position + AnchorPosition() + Size / 2, DrawColor);
        }

        private void Rectangle(Vector2i botLeft, Vector2i topRight, MColor color)
        {
            if (Window.IsOnScreen(botLeft) && Window.IsOnScreen(topRight))
            {
                for (int i = botLeft.X; i < topRight.X; i++)
                {
                    for (int j = botLeft.Y; j < topRight.Y; j++)
                    {
                        Window[i, j] = color;
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
                            Window[i, j] = color;
                    }
                }
            }
        }
    }
}
