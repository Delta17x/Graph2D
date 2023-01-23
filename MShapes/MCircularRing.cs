using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCS.MShapes
{
    public class MCircularRing : MGraphicsObject
    {
        public int InnerRadius { get; set; }
        public int OuterRadius { get; set; }

        public MCircularRing(MWindow window, int innerRadius, int outerRadius) : base(window)
        {
            InnerRadius = innerRadius;
            OuterRadius = outerRadius;
        }

        public override void Draw()
        {
            CircleRing(AnchoredPosition(), InnerRadius, OuterRadius, DrawColor);
        }

        private void CircleRing(Vector2i center, int innerRadius, int outerRadius, MColor color)
        {
            Vector2i botLeft = center - new Vector2i(outerRadius, outerRadius);
            Vector2i topRight = center + new Vector2i(outerRadius, outerRadius);
            if (Window.IsOnScreen(botLeft) && Window.IsOnScreen(topRight))
            {
                for (int i = botLeft.X; i < topRight.X; i++)
                {
                    for (int j = botLeft.Y; j < topRight.Y; j++)
                    {
                        float L = (center - new Vector2i(i, j)).EuclideanLength;
                        if (innerRadius <= L && L <= outerRadius)
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
                        float L = (center - new Vector2i(i, j)).EuclideanLength;
                        if (Window.IsOnScreen(new(i, j)) && innerRadius <= L && L <= outerRadius)
                            Window.Screen[i][j] = color;
                    }
                }
            }
        }
    }
}
