using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MathCS
{
    // float because opengl
    /// <summary>
    /// Represents an RGB color with each value stored as a float from 0.0-1.0.
    /// </summary>
    public struct MColor
    {
        public float R;
        public float G;
        public float B;

        public MColor(float r = 0, float g = 0, float b = 0)
        {
            R = r;
            G = g;
            B = b;
        }

        public MColor Mix(MColor b)
        {
            return 0.5f * (this + b);
        }

        public static MColor operator* (MColor color, float x)
        {
            return new(color.R * x, color.G * x, color.B * x);
        }


        public static MColor operator* (float x, MColor color)
        {
            return new(color.R * x, color.G * x, color.B * x);
        }

        public static MColor operator+ (MColor color, MColor color2)
        {
            return new(color.R + color2.R, color.G + color2.G, color.B + color2.B);
        }

        public static MColor Mix(MColor a, MColor b)
        {
            return 0.5f * (a + b);
        }

        public static readonly MColor Black = new MColor(0, 0, 0);
        public static readonly MColor White = new MColor(1, 1, 1);
        public static readonly MColor Red = new MColor(1, 0, 0);
        public static readonly MColor Blue = new MColor(0, 0, 1);
        public static readonly MColor Green = new MColor(0, 1, 0);
    }
}
